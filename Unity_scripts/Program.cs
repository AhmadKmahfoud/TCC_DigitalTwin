using System.Collections.Generic;

[System.Serializable]
public class Program
{
    public string Name;
    public List<Point> Points = new List<Point>();

    public Program(string name)
    {
        Name = name;
        Points = new List<Point>();
    }

    public void AddPoint(Point point)
    {
        Points.Add(point);
    }
}
