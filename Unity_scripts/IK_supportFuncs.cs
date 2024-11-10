using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_supportFuncs
{
    #region Support Funcs
    public Vector3 GetWristCenter(Vector3 gripperPoint, Matrix4x4 R0g, float dg){ //calculate the wrist center
        float xu = gripperPoint.x;
        float yu = gripperPoint.y;
        float zu = gripperPoint.z;

        float nx = R0g.m02;
        float ny = R0g.m12;
        float nz = R0g.m22;

        float xw = xu - dg * nx;
        float yw = yu - dg * ny;
        float zw = zu - dg * nz;

        return new Vector3(xw, yw, zw);
    }

    public float GetHypotenuse(float a, float b){ //calculate the hypotenuse of a right triangle
        return Mathf.Sqrt(a*a + b*b);
    }

    public float GetCosineLawAngle(float a, float b, float c){ //calculate the angle using the cosine law
        float cosGamma = (a*a + b*b - c*c) / (2*a*b);
        float sinGamma = Mathf.Sqrt(1 - cosGamma*cosGamma);
        float gamma = Mathf.Atan2(sinGamma, cosGamma);
        return gamma;
    }
    #endregion
}
