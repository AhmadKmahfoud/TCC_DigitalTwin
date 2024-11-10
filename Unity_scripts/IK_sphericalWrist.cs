using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IK_sphericalWrist : MonoBehaviour
{
    #region Variables
    public float[] DH_alpha, DH_a, DH_d, DH_theta; //DH parameters
    private float px, py, pz, roll, pitch, yaw; //input - desired positions and rotations

    public Transform referencia; //Base do Robo
    public Transform target;
    public List<Transform> joints = new List<Transform>();

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
            IK_Callable(px, py, pz, roll, pitch, yaw);

            // Atualiza a posição e a rotação para o próximo frame
            ultimaPosicao = target.transform.position;
            ultimaRotacao = target.transform.rotation;
        }
    }
    #endregion
    
    public void IK_Callable(float px, float py, float pz, float roll, float pitch, float yaw){
        IK_matrixOp ikM = new IK_matrixOp();
        IK_supportFuncs ikS = new IK_supportFuncs();

        Matrix4x4 tT = new Matrix4x4(
            new Vector4(1, 0, 0, px),
            new Vector4(0, 1, 0, py),
            new Vector4(0, 0, 1, pz),
            new Vector4(0, 0, 0,  1)
        );

        Matrix4x4 RT = ikM.RotZ(yaw) * ikM.RotY(pitch) * ikM.RotX(roll);

        float Wx = px - DH_d[5]*RT.m02;
        float Wy = py - DH_d[5]*RT.m12;
        float Wz = pz - DH_d[5]*RT.m22;
        Vector3 Wrist = new Vector3(Wx, Wy, Wz);

        Vector3 first3 = GetFirst3(Wrist);
        Matrix4x4 R03 = ikM.RotZ(DH_theta[2]) * ikM.RotY(DH_theta[1]) * ikM.RotX(DH_theta[0]);;
        Matrix4x4 R03T = R03.transpose;
        Matrix4x4 R36 = R03T * RT;

        Vector3 last3 = GetLast3(R36);

        DH_theta[0] = Mathf.Rad2Deg * first3.x;
        DH_theta[1] = Mathf.Rad2Deg * first3.y;
        DH_theta[2] = Mathf.Rad2Deg * first3.z;
        DH_theta[3] = Mathf.Rad2Deg * last3.x;
        DH_theta[4] = Mathf.Rad2Deg * last3.y;
        DH_theta[5] = Mathf.Rad2Deg * last3.z;
        
        joints[0].localRotation = Quaternion.Euler(0 , 0  ,  DH_theta[0]);
        joints[1].localRotation = Quaternion.Euler(90, 0  , -DH_theta[1]);
        joints[2].localRotation = Quaternion.Euler(0 , 0  , -DH_theta[2]);
        joints[3].localRotation = Quaternion.Euler(0 , 90 ,  DH_theta[3]);
        joints[4].localRotation = Quaternion.Euler(0 , -90, -DH_theta[4]);
        joints[5].localRotation = Quaternion.Euler(0 , 90 ,  DH_theta[5]);
    }

    #region Angles Calculation
    public Vector3 GetFirst3(Vector3 wristCenter){ //calculate the first three angles
        IK_supportFuncs ikS = new IK_supportFuncs();

        float x = wristCenter.x;
        float y = wristCenter.y;
        float z = wristCenter.z;

        float zNew = z - DH_d[0];

        float r = ikS.GetHypotenuse(y, x);
        float s = ikS.GetHypotenuse(zNew, r);
        float l5 = ikS.GetHypotenuse(DH_a[2], DH_d[3]);

        float beta = Mathf.Acos((DH_a[1]*DH_a[1] + s*s - l5*l5)/(2*DH_a[1]*s));
        float alpha = Mathf.Atan2(zNew, r);

        float gamma = Mathf.Acos((DH_a[1]*DH_a[1] + l5*l5 - s*s)/(2*DH_a[1]*l5));
        float phi = Mathf.Atan2(DH_d[3], DH_a[2]);;

        DH_theta[0] = Mathf.Atan2(y, x);
        DH_theta[1] = Mathf.PI/2 - beta - alpha;
        DH_theta[2] = Mathf.PI -gamma -phi;

        return new Vector3(DH_theta[0], DH_theta[1], DH_theta[2]);
    }

    public Vector3 GetLast3(Matrix4x4 R){ //calculate the last three angles


        DH_theta[4] = Mathf.Asin(R.m22);

        if(DH_theta[4] != 0 && DH_theta[4] != Mathf.PI && DH_theta[4] != -Mathf.PI && DH_theta[4] != 2*Mathf.PI && DH_theta[4] != -2*Mathf.PI && DH_theta[4] != 3*Mathf.PI && DH_theta[4] != -3*Mathf.PI){      
            DH_theta[3] = Mathf.Atan2(R.m12, R.m02);
            DH_theta[5] = Mathf.Atan2(-R.m21, R.m20);
        }else{
            DH_theta[3] = Mathf.Atan2(R.m10, R.m00);
            DH_theta[5] = 0.0f;
        }

        return new Vector3(DH_theta[3], DH_theta[4], DH_theta[5]);
    }
    #endregion
}
