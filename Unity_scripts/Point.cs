using System.Collections.Generic;

[System.Serializable]
public class Point
{
    public string Name;
    public bool MovementType;
    public float Speed;
    public float X;
    public float Y;
    public float Z;
    public List<JointPose> Poses = new List<JointPose>();

    public Point(string name, bool movementType, float speed, float x, float y, float z)
    {
        Name = name;
        MovementType = movementType;
        Speed = speed;
        X = x;
        Y = y;
        Z = z;
    }
}
