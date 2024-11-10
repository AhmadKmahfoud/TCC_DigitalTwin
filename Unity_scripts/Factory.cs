using System.Collections.Generic;

[System.Serializable]
public class Factory
{
    public string Name;
    public List<Robot> Robots = new List<Robot>();
    
    public Factory(string name)
    {
        Name = name;
        Robots = new List<Robot>();
    }

    public void AddRobot(Robot Robot)
    {
        Robots.Add(Robot);
    }
}
