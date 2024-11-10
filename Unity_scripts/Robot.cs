using System.Collections.Generic;

[System.Serializable]
public class Robot
{
    public string Name;
    public List<Program> Programs = new List<Program>();

    public Robot(string name)
    {
        Name = name;
        Programs = new List<Program>();
    }
}
