using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JointPose
{
    public Quaternion JointPoseValues;

    public JointPose(Quaternion jointPoseValues)
    {
        JointPoseValues = jointPoseValues;
    }
}