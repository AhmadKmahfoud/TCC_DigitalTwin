using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_matrixOp
{
    public Matrix4x4 Pose(float theta, float alpha, float a, float d){ //calculate the pose matrix
        float st = Mathf.Sin(theta);
        float ct = Mathf.Cos(theta);
        float sa = Mathf.Sin(alpha);
        float ca = Mathf.Cos(alpha);

        Matrix4x4 T = new Matrix4x4{
            m00 = ct  ,  m01 = -st*ca, m02 =  st*sa, m03 = a*ct ,
            m10 = st  ,  m11 =  ct*ca, m12 = -ct*sa, m13 = a*st ,
            m20 = 0.0f,  m21 =  sa   , m22 =  ca   , m23 = d    ,
            m30 = 0.0f,  m31 =  0.0f , m32 =  0.0f , m33 = 1.0f};
        return T;
    }
    
    #region Rotation Matrix
    public Matrix4x4 RotX(float q){ //rotation matrix around the X-axis
        float sq = Mathf.Sin(q);
        float cq = Mathf.Cos(q);

        Matrix4x4 r = new Matrix4x4{
            m00 = 1.0f, m01 = 0.0f,     m02 = 0.0f,     m03 = 0.0f,
            m10 = 0.0f, m11 = cq,       m12 = -sq,      m13 = 0.0f,
            m20 = 0.0f, m21 = sq,       m22 = cq,       m23 = 0.0f,
            m30 = 0.0f, m31 = 0.0f,     m32 = 0.0f,     m33 = 1.0f
        };
        return r;
    }

    public Matrix4x4 RotY(float q){ //rotation matrix around the Y-axis
        float sq = Mathf.Sin(q);
        float cq = Mathf.Cos(q);

        Matrix4x4 r = new Matrix4x4{
            m00 = cq,   m01 = 0.0f,     m02 = sq,       m03 = 0.0f,
            m10 = 0.0f, m11 = 1.0f,     m12 = 0.0f,     m13 = 0.0f,
            m20 = -sq,  m21 = 0.0f,     m22 = cq,       m23 = 0.0f,
            m30 = 0.0f, m31 = 0.0f,     m32 = 0.0f,     m33 = 1.0f
        };
        return r;
    }
    
    public Matrix4x4 RotZ(float q){ //rotation matrix around the Z-axis
        float sq = Mathf.Sin(q);
        float cq = Mathf.Cos(q);

        Matrix4x4 r = new Matrix4x4{
            m00 = cq,   m01 = -sq,      m02 = 0.0f,     m03 = 0.0f,
            m10 = sq,   m11 = cq,       m12 = 0.0f,     m13 = 0.0f,     
            m20 = 0.0f, m21 = 0.0f,     m22 = 1.0f,     m23 = 0.0f,
            m30 = 0.0f, m31 = 0.0f,     m32 = 0.0f,     m33 = 1.0f
        };
        return r;
    }
    #endregion
}
