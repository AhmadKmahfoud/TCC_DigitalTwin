using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Linq;
using TMPro;
using System.Text.RegularExpressions;

public class JSONManipulation : MonoBehaviour
{
    #region Variables
    //[SerializeField] public string filePath = @"C:\Users\NSPi\Documents\JSON_Unity\RobotsINFO.json";
    [SerializeField] public string filePath;
    [SerializeField] public string filePathFromJSON;
    [SerializeField] public string filePathFromJSON1;
    [SerializeField] public string filePathFromJSON2;
    [SerializeField] public string filePathFromJSON3;

    [SerializeField] public List<Transform> joints = new List<Transform>();
    [SerializeField] public Factory factory;

    [Space(10)][Header("Text Objects")]
    public TextMeshProUGUI TextPoints;
    public TextMeshProUGUI TextProgramName;
    public TextMeshProUGUI TextPrograms;

    [Space(5)][Header("Persistent Data Script")]
    public FilePersistentData PD;
    [Space(5)][Header("TP Manager Script")]
    public TPManager TP;
    [Header("SensorController Script")]
    public SensorController SC;

    private int currentOrder = 0;
    [HideInInspector] public int activeRobot = 0;

    public bool testePlayLastProgram = false;
    public bool testeCreatePoint = false;
    public bool testeCreateProgram = false;

    public bool testeUpPoint = false;
    public bool testeDownPoint = false;
    public bool testeUpProgram = false;
    public bool testeDownProgram = false;
    Program programTeste;
    #endregion

    #region Start/Update
    private void Start(){
        filePath = PD.PersistentPath;
        filePathFromJSON = PD.PersistentPathSaved;
        filePathFromJSON1 = PD.PersistentPathOp1;
        filePathFromJSON2 = PD.PersistentPathOp2;
        filePathFromJSON3 = PD.PersistentPathOp3;
        
        factory = new Factory("Célula de Personalização");

        //Create and add a new robot
        Robot robot = new Robot("RV-4FRL");
        factory.Robots.Add(robot);
        
        //Create and add a new program
        Program program = new Program("Program 0");
        factory.Robots[activeRobot].Programs.Add(program);
        int programIndex = factory.Robots[activeRobot].Programs.Count-1;

        //Create and add a new point
        Point startPoint = new Point("Point 0", true, 0.5f, 0f, 0f, 0f);
        factory.Robots[activeRobot].Programs[programIndex].Points.Add(startPoint);

        
        for(int j = 0; j < joints.Count; j++){
            JointPose jointPose = new JointPose(joints[j].localRotation);
            startPoint.Poses.Add(jointPose);
        }

        TP.linesPoint = TextPoints.text.Split('\n');
        TP.linesProgram = TextPrograms.text.Split('\n');

        TP.UpdateTextPoints(programIndex);
        TextPoints.text = TP.UpdateDisplay(TextPoints.text, TP.currentPointLine, TP.linesPoint);
        TP.StartTextPrograms();
        TextPrograms.text = TP.UpdateDisplay(TextPrograms.text, TP.currentProgramLine, TP.linesProgram);

        // Save the factory as JSON
        string json = JsonUtility.ToJson(factory);
        File.WriteAllText(filePath, json);
        Debug.Log("JSON saved to " + filePath);

        Program programTeste = factory.Robots[activeRobot].Programs[factory.Robots[activeRobot].Programs.Count - 1];
    }

    private void Update(){
        if(testeCreatePoint){
            createPoint();
            testeCreatePoint = false;
        }
        if(testePlayLastProgram){
            PlayLastProgram();
            testePlayLastProgram = false;
        }
        if(testeCreateProgram){
            createProgram();
            testeCreateProgram = false;
        }
        if(testeUpPoint){
            TP.PrevLinePoint();
            testeUpPoint = false;
        }
        if(testeDownPoint){
            TP.NextLinePoint();
            testeDownPoint = false;
        }
        if(testeUpProgram){
            TP.PrevLineProgram();
            testeUpProgram = false;
        }
        if(testeDownProgram){
            TP.NextLineProgram();
            testeDownProgram = false;
        }
    }
    #endregion

    #region JSON Manipulation
    public void createProgram(){
        Program program = new Program("Program 0");
        factory.Robots[activeRobot].Programs.Add(program);

        // Save the updated factory as JSON
        string json = JsonUtility.ToJson(factory);
        File.WriteAllText(filePath, json);

        TP.currentProgramLine = factory.Robots[activeRobot].Programs.Count-2;
        TP.linesProgram = TextPrograms.text.Split('\n');
        TextPrograms.text = TP.UpdateDisplay(TextPrograms.text, TP.currentProgramLine, TP.linesProgram);
        TP.UpdateTextPrograms(program);
    }

    public void createPoint(){_createPoint(TP.currentProgramLine);}

    private void _createPoint(int programIndex){
            Program currentProgram = factory.Robots[activeRobot].Programs[programIndex];

            // Create a new point for the current program
            Point newPoint = new Point("Point 0", true, 0.5f, 0f, 0f, 0f);
            newPoint.Name = "Point " + (currentProgram.Points.Count);
            newPoint.Speed = TP.SpeedSlider.value;
            newPoint.MovementType = TP.MovToggle.isOn;

            // Save the updated factory as JSON
            string json = JsonUtility.ToJson(factory);
            File.WriteAllText(filePath, json);

            //save the current jointPoses to the new point
            for(int j = 0; j < joints.Count; j++){
                JointPose jointPose = new JointPose(joints[j].localRotation);
                newPoint.Poses.Add(jointPose);
            }

            currentProgram.Points.Add(newPoint);

            StringBuilder pointDetails = new StringBuilder(); // Create a StringBuilder object to store the point details
            pointDetails.AppendLine("Point\tMovementType\tSpeed"); // Add the header to the StringBuilder

            foreach (Point point in currentProgram.Points){ // Iterate over the points and append their details to the StringBuilder
                string linearStr = point.MovementType ? "MOVL  " : "MOVJ  ";
                pointDetails.AppendLine(point.Name + "\t" + linearStr + "\tV=" + point.Speed.ToString("0.00"));
            }

            // Assign the point details to the TMP object
            //TextPoints.text = pointDetails.ToString();

            TP.currentPointLine = currentProgram.Points.Count-1;
            TP.linesPoint = TextPoints.text.Split('\n');
            TP.UpdateTextPoints(programIndex);
            TextPoints.text = TP.UpdateDisplay(TextPoints.text, TP.currentPointLine, TP.linesPoint);
            //TP.CanvasToSubmitPoint.SetActive(false);
    }


    public void PlayLastProgram(){ //Para rodar apenas o ultimo programa do robo selecionado
        Robot robot = factory.Robots[activeRobot];

        if (robot.Programs.Count > 0){
            Program currentProgram = factory.Robots[activeRobot].Programs[factory.Robots[activeRobot].Programs.Count - 1];
            StartCoroutine(RunProgram(currentProgram));
        }
    }

    public void PlayProgram(){ //Para rodar um programa qualquer
        Program program = factory.Robots[activeRobot].Programs[TP.currentProgramLine];
        StartCoroutine(RunProgram(program));
    }

    public void PlayProgramFromJSON(){
        string jsonINFO = File.ReadAllText(filePathFromJSON);
        Factory factory = JsonUtility.FromJson<Factory>(jsonINFO);
        StartCoroutine(MoveRobotsSequentially(factory));
    }

    public void PlayProgramFromJSON1(){
        string jsonINFO = File.ReadAllText(filePathFromJSON1);
        Factory factory = JsonUtility.FromJson<Factory>(jsonINFO);
        StartCoroutine(MoveRobotsSequentially(factory));
    }
        public void PlayProgramFromJSON2(){
        string jsonINFO = File.ReadAllText(filePathFromJSON2);
        Factory factory = JsonUtility.FromJson<Factory>(jsonINFO);
        StartCoroutine(MoveRobotsSequentially(factory));
    }
        public void PlayProgramFromJSON3(){
        string jsonINFO = File.ReadAllText(filePathFromJSON3);
        Factory factory = JsonUtility.FromJson<Factory>(jsonINFO);
        StartCoroutine(MoveRobotsSequentially(factory));
    }
    #endregion

    #region Operating Sequence
    public IEnumerator RunProgram(Program program){
        Debug.Log($"Program {program.Name} running");
        for (int i = 0; i < program.Points.Count; i++){
            Point point = program.Points[i];
            float speed = point.Speed;
            bool movementType = point.MovementType;

            Quaternion[] startRots = new Quaternion[joints.Count];
            Quaternion[] endRots = new Quaternion[joints.Count];

            for (int j = 0; j < joints.Count; j++){
                startRots[j] = joints[j].localRotation;
                endRots[j] = point.Poses[j].JointPoseValues;
            }
            
            Debug.Log($"Running {point.Name}");
            
            float t = 0;
            while (t < 1){
                t += Time.deltaTime * speed;

                Quaternion[] currentRots = new Quaternion[joints.Count];
                for (int j = 0; j < joints.Count; j++){
                    currentRots[j] = Quaternion.Slerp(startRots[j], endRots[j], t);
                }

                for (int j = 0; j < joints.Count; j++){
                    joints[j].localRotation = currentRots[j];
                }

                yield return null;
            }

            for (int j = 0; j < joints.Count; j++){
                joints[j].localRotation = endRots[j];
            }
        }
        yield return new WaitForSeconds(0f); //add time to wait after programs
    }

    private IEnumerator MoveRobotsSequentially(Factory factory){
        Robot robot = factory.Robots[activeRobot];

        for (int i = 0; i < robot.Programs.Count; i++){ //iterando sobre os programas
            Program program = robot.Programs[i];
            for (int j = 0; j < program.Points.Count; j++){ //iterando sobre os pontos
                Debug.Log($"Moving robot: {robot.Name}\nExecuting program: {i}\nMoving to point: {j}");

                Point point = program.Points[j];
                float speed = point.Speed;
                bool movementType = point.MovementType;

                Quaternion[] startRots = new Quaternion[joints.Count];
                Quaternion[] endRots = new Quaternion[joints.Count];

                for (int k = 0; k < joints.Count; k++){ //iterando sobre as juntas
                    startRots[k] = joints[k].localRotation;
                    endRots[k] = point.Poses[k].JointPoseValues;
                }

                float t = 0;
                while (t < 1){  //faz efetivamente a movimentacao, de startRots[] para endRots[] em um tempo t
                    t += Time.deltaTime * speed;

                    Quaternion[] currentRots = new Quaternion[joints.Count];
                    for (int i2 = 0; i2 < joints.Count; i2++){
                        currentRots[i2] = Quaternion.Slerp(startRots[i2], endRots[i2], t);
                    }

                    for (int j2 = 0; j2 < joints.Count; j2++){
                        joints[j2].localRotation = currentRots[j2];
                    }
                    yield return null;
                }

                for (int k2 = 0; k2 < joints.Count; k2++){ //atribuir as rotacoes finais para as juntas, para garantir
                    joints[k2].localRotation = endRots[k2];
                }
                yield return new WaitForSeconds(0f); // Wait for a short duration before moving to the next point
            }
        }
        Debug.Log("All robots moved!");
        yield return new WaitForSeconds(0f);
    }
    #endregion
}