using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IK_algoritmo : MonoBehaviour
{
    #region Variables
    public float[] DH_alpha, DH_a, DH_d, DH_theta; //DH parameters
    private float px, py, pz, roll, pitch, yaw; //input - desired positions and rotations

    public Transform referencia; //Base do Robo
    public Transform target;
    public List<Transform> joints = new List<Transform>();

    public float[] finalAngles;
    //Do target
    private Vector3 ultimaPosicao;
    private Quaternion ultimaRotacao;
    #endregion

    #region Start/Update
    void Start(){
        ultimaPosicao = target.transform.position;
        ultimaRotacao = target.transform.rotation;
    }

    void Update(){
        //Chama a IK se o 'target' se mexeu (mudar posicao ou rotacao)
        if (target.transform.position != ultimaPosicao || target.transform.rotation != ultimaRotacao)
        {
            px =  (target.position.x  -  referencia.position.x);
            py = -(target.position.z  -  referencia.position.z);
            pz =  (target.position.y  -  referencia.position.y);
            roll =   (target.rotation.eulerAngles.x  -  referencia.localRotation.x) * Mathf.Deg2Rad;
            pitch = -(target.rotation.eulerAngles.z  -  referencia.localRotation.z) * Mathf.Deg2Rad;
            yaw =    (target.rotation.eulerAngles.y  -  referencia.localRotation.y) * Mathf.Deg2Rad;

            // Chama o algoritmo de IK
            finalAngles = IK_Callable(px, py, pz, roll, pitch, yaw);
                    
            joints[0].localRotation = Quaternion.Euler(0 , 0  ,  finalAngles[0]);
            joints[1].localRotation = Quaternion.Euler(90, 0  , -finalAngles[1]);
            joints[2].localRotation = Quaternion.Euler(0 , 0  , -finalAngles[2]);
            joints[3].localRotation = Quaternion.Euler(0 , 90 ,  finalAngles[3]);
            joints[4].localRotation = Quaternion.Euler(0 , -90, -finalAngles[4]);
            joints[5].localRotation = Quaternion.Euler(0 , 90 ,  finalAngles[5]);

            // Atualiza a posição e a rotação para o próximo frame
            ultimaPosicao = target.transform.position;
            ultimaRotacao = target.transform.rotation;
        }
    }
    #endregion

    public float[] IK_Callable(float px, float py, float pz, float roll, float pitch, float yaw){
        IK_matrixOp ikM = new IK_matrixOp();
        IK_supportFuncs ikS = new IK_supportFuncs();

        Matrix4x4 T01 = ikM.Pose(DH_theta[0]    , 0            , 0        , 0       );
        Matrix4x4 T12 = ikM.Pose(DH_theta[1]    , DH_alpha[0]  , DH_a[0]  , DH_d[0] );
        Matrix4x4 T23 = ikM.Pose(DH_theta[2]    , DH_alpha[1]  , DH_a[1]  , DH_d[1] );
        Matrix4x4 T34 = ikM.Pose(DH_theta[3]    , DH_alpha[2]  , DH_a[2]  , DH_d[2] );
        Matrix4x4 T45 = ikM.Pose(DH_theta[4]    , DH_alpha[3]  , DH_a[3]  , DH_d[3] );
        Matrix4x4 T56 = ikM.Pose(DH_theta[5]    , DH_alpha[4]  , DH_a[4]  , DH_d[4] );
        Matrix4x4 T6g = ikM.Pose(0              , DH_alpha[5]  , DH_a[5]  , DH_d[5] );

        Matrix4x4 T03 = T01 * T12 * T23;
        Matrix4x4 R03 = new Matrix4x4(
            new Vector4(T03.m00, T03.m01, T03.m02, 0),
            new Vector4(T03.m10, T03.m11, T03.m12, 0),
            new Vector4(T03.m20, T03.m21, T03.m22, 0),
            new Vector4(0      , 0      , 0      , 1)
        );

        Matrix4x4 R03T = R03.transpose; // Transpose R03

        Matrix4x4 Rgu = (ikM.RotZ(Mathf.PI) * ikM.RotY(-Mathf.PI / 2));
        Matrix4x4 RguT = Rgu.transpose;
        
        Matrix4x4 R0u_eval = ikM.RotZ(yaw) * ikM.RotY(pitch) * ikM.RotX(roll); //Matrix4x4 euler_R = RotZ(alpha) * RotY(beta) * RotX(gamma)
        Matrix4x4 R0g_eval = R0u_eval * RguT; // R0g_eval * Rgu = R0u_eval

        Vector3 gripperPoint = new Vector3(px, py, pz);
        Vector3 wristCenter = ikS.GetWristCenter(gripperPoint, R0g_eval, DH_d[5]);

        Vector3 first3 = GetFirst3(wristCenter); // First three angles (DH_theta[0], DH_theta[1], DH_theta[2])

        Matrix4x4 R03_eval = new Matrix4x4(
            new Vector4( Mathf.Sin(first3.y + first3.z) * Mathf.Cos(first3.x), Mathf.Sin(first3.x) * Mathf.Sin(first3.y + first3.z),  Mathf.Cos(first3.y + first3.z), 0),
            new Vector4( Mathf.Cos(first3.x) * Mathf.Cos(first3.y + first3.z), Mathf.Sin(first3.x) * Mathf.Cos(first3.y + first3.z), -Mathf.Sin(first3.y + first3.z), 0),
            new Vector4(-Mathf.Sin(first3.x)                                 , Mathf.Cos(first3.x)                                 , 0                              , 0),
            new Vector4( 0                                                   , 0                                                   , 0                              , 1)
        );

        Matrix4x4 R03T_eval = R03_eval.transpose;
        Matrix4x4 R36_eval = R03T_eval * R0g_eval; // Evaluate R36

        Vector3 last3 = GetLast3(R36_eval); // Last three angles


        DH_theta[0] = Mathf.Rad2Deg * first3.x;
        DH_theta[1] = Mathf.Rad2Deg * first3.y;
        DH_theta[2] = Mathf.Rad2Deg * first3.z;
        DH_theta[3] = Mathf.Rad2Deg * last3.x;
        DH_theta[4] = Mathf.Rad2Deg * last3.y;
        DH_theta[5] = Mathf.Rad2Deg * last3.z;

        return DH_theta;
    }

    #region Angles Calculation
    public Vector3 GetFirst3(Vector3 wristCenter){ //calculate the first three angles
        IK_supportFuncs ikS = new IK_supportFuncs();

        float x = wristCenter.x;
        float y = wristCenter.y;
        float z = wristCenter.z;

        float l = ikS.GetHypotenuse(DH_d[3], DH_a[2]);
        float phi = Mathf.Atan2(DH_d[3], -DH_a[2]);

        float xPrime = ikS.GetHypotenuse(x, y);
        float mx = xPrime - DH_a[0];
        float my = z - DH_d[0];
        float m = ikS.GetHypotenuse(mx, my);
        float alpha = Mathf.Atan2(my, mx);
    
        float gamma = ikS.GetCosineLawAngle(l, DH_a[1], m);
        float beta = ikS.GetCosineLawAngle(m, DH_a[1], l);

        DH_theta[0] = Mathf.Atan2(y, x);
        DH_theta[1] = Mathf.PI/2 - beta - alpha;
        DH_theta[2] = -(gamma - phi);

        return new Vector3(DH_theta[0], DH_theta[1], DH_theta[2]);
    }

    public Vector3 GetLast3(Matrix4x4 R){ //calculate the last three angles
        float sin_q4 = R.m22; 
        float cos_q4 = -R.m02;

        float sin_q5 = Mathf.Sqrt(R.m02 * R.m02 + R.m22 * R.m22);
        float cos_q5 = R.m12;

        float sin_q6 = -R.m11;
        float cos_q6 = R.m10;

        DH_theta[3] = Mathf.Atan2(sin_q4, cos_q4);
        DH_theta[4] = Mathf.Atan2(sin_q5, cos_q5);
        DH_theta[5] = Mathf.Atan2(sin_q6, cos_q6);

        return new Vector3(DH_theta[3], DH_theta[4], DH_theta[5]);
    }
    #endregion
}