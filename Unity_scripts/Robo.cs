using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class Robo
{
    #region Variables
    public string Name { get; set; }  // Contrutor 1
    public int JointNumber { get; set; } // Construtor 2
    public List<Program> Programs { get; set; } // Construtor 3

    [Header("Robo GameObjects")]
    public GameObject[] RoboModel;
    public GameObject[] Robo_Obj;

    [Space(10)]
    [Header("Robo DH Parameters")]
    public float[] DH_a;
    public float[] DH_alpha;
    public float[] DH_d;
    public float[] DH_theta;

    [Space(10)]
    [Header("Robo Angle Limits")]
    public float[] JointAngleLowerLimit;
    public float[] JointAngleUpperLimit;
    [Space(5)]
    public float[] JointAngle;
    private float[] HomeAngles;

    [Space(10)]
    [Header("Robo Materials")]
    public Material SelectedMaterial;
    public Material NormalMaterial;

    [Space(10)]
    [Header("Robo Speed Parameters")]
    public Slider speedSlider;
    public float jointSpeed = 200f;

    private int RoboJoint = 1;
    private string dirProgs = "";

    private bool thumbstickMovedRight = false;
    private bool thumbstickMovedLeft = false;
    #endregion

    //Classe para construir Robos Industriais
    public Robo(string name, int jointNumber)
    {
        Name = name;
        JointNumber = jointNumber + 1;
        Programs = new List<Program>();
    }

    //Executa Inicializacoes do Robo
    public void RoboInitiate(float[] HomeAngles, float[] LowerLimitAngles, float[] UpperLimitAngles)
    {
        JointAngle = HomeAngles;
        JointAngleLowerLimit = LowerLimitAngles;
        JointAngleUpperLimit = UpperLimitAngles;

        for (int i = 0; i <= 5; i++){
            RoboModel[i+1].transform.GetChild(0).GetComponent<MeshRenderer>().material = NormalMaterial;

            //Coloca o robo na posicao Home
            Quaternion currentRotation = Robo_Obj[i].transform.localRotation;
            currentRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y, HomeAngles[i]);
            Robo_Obj[i].transform.localRotation = currentRotation;
        }
        //RoboModel[1].transform.GetChild(0).GetComponent<MeshRenderer>().material = SelectedMaterial;
    }

    //Rotina de Animacao de Selecao de Junta
    public void Selection() // Select a Joint and Show in Green
    {
        float thumbstickX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        bool RoboControlMode = (Input.GetKey(KeyCode.LeftControl)) || (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) && (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger));

        if ((thumbstickX > 0.2 || (Input.GetKeyDown(KeyCode.UpArrow))) && RoboControlMode && thumbstickMovedRight == false)
        {
            thumbstickMovedRight = true; // Set the flag to indicate the thumbstick was moved to the right

            if (RoboJoint < 6)
            {
                RoboJoint++;
                RoboModel[RoboJoint].transform.GetChild(0).GetComponent<MeshRenderer>().material = SelectedMaterial;
                RoboModel[RoboJoint - 1].transform.GetChild(0).GetComponent<MeshRenderer>().material = NormalMaterial;
            }
        }
        else if ((thumbstickX < -0.2 || (Input.GetKeyDown(KeyCode.DownArrow))) && RoboControlMode && thumbstickMovedLeft == false)
        {
            thumbstickMovedLeft = true; // Set the flag to indicate the thumbstick was moved to the left

            if (RoboJoint > 1)
            {
                RoboJoint--;
                RoboModel[RoboJoint].transform.GetChild(0).GetComponent<MeshRenderer>().material = SelectedMaterial;
                RoboModel[RoboJoint + 1].transform.GetChild(0).GetComponent<MeshRenderer>().material = NormalMaterial;
            }
        }

        if (thumbstickX == 0 && thumbstickMovedRight == true)
        {
            thumbstickMovedRight = false;
        }
        if (thumbstickX == 0 && thumbstickMovedLeft == true)
        {
            thumbstickMovedLeft = false;
        }
    }

    // Rotina de Manipulacao de Junta Selecionada
    public void Manipulate() // Manipulate a Selected Joint
    {
        float thumbstickY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        bool RoboControlMode = (Input.GetKey(KeyCode.LeftControl)) || ((OVRInput.Get(OVRInput.Button.PrimaryHandTrigger)) && (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)));

        if ((thumbstickY > 0 || (Input.GetKey(KeyCode.RightArrow))) && RoboControlMode && JointAngle[RoboJoint-1] > JointAngleLowerLimit[RoboJoint-1])
        {
            if((Input.GetKey(KeyCode.RightArrow)))
            {
                JointAngle[RoboJoint-1] -= 0.1f;
                Robo_Obj[RoboJoint-1].transform.Rotate(-Vector3.forward * 0.1f);
            }
            else
            {
                JointAngle[RoboJoint-1] -= jointSpeed * speedSlider.value * Time.deltaTime * thumbstickY;
                Robo_Obj[RoboJoint-1].transform.Rotate(-Vector3.forward * jointSpeed * speedSlider.value * Time.deltaTime * thumbstickY);
            }
            
        }
        else if ((thumbstickY < 0 || (Input.GetKey(KeyCode.LeftArrow))) && RoboControlMode && JointAngle[RoboJoint-1] < JointAngleUpperLimit[RoboJoint-1])
        {
            if((Input.GetKey(KeyCode.LeftArrow)))
            {
                JointAngle[RoboJoint-1] += 0.1f;
                Robo_Obj[RoboJoint-1].transform.Rotate(Vector3.forward * 0.1f);
            }
            else
            {
                JointAngle[RoboJoint-1] += jointSpeed * speedSlider.value * Time.deltaTime * Mathf.Abs(thumbstickY);
                Robo_Obj[RoboJoint-1].transform.Rotate(Vector3.forward * jointSpeed * speedSlider.value * Time.deltaTime * Mathf.Abs(thumbstickY));
            }
        }
    }

    public void TP_Manipulate(int TPjointNumber, bool increment) //Manipulacao por botoes do TP
    {
        for(int i=1; i <= 6; i++){
            RoboModel[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = NormalMaterial;
        }
        RoboModel[TPjointNumber].transform.GetChild(0).GetComponent<MeshRenderer>().material = SelectedMaterial;

        if (increment && JointAngle[TPjointNumber-1] < JointAngleUpperLimit[TPjointNumber-1])
        {
            JointAngle[TPjointNumber - 1] += 0.5f;
            Robo_Obj[TPjointNumber - 1].transform.Rotate(-Vector3.forward * 0.5f);
        }
        else if(increment==false && JointAngle[TPjointNumber-1] > JointAngleLowerLimit[TPjointNumber-1])
        {
            JointAngle[TPjointNumber - 1] -= 0.5f;
            Robo_Obj[TPjointNumber - 1].transform.Rotate(Vector3.forward * 0.5f);
        }
    }
    
    public void AddProgram(Program program)
    {
        Programs.Add(program);
    }

    public void CreateProgram(string programName)
    {
        programName = dirProgs + programName;
        using (FileStream fs = File.Create(programName))
        {
            Byte[] title = new UTF8Encoding(true).GetBytes("");
            fs.Write(title, 0, title.Length);
        }
    }

    //Escreve a Junta atual no Console
    public string PrintJointPosition()
    {
        string Pos = "";
        string pos2file = "";

        for (int i = 1; i <= JointNumber; i++)
        {
            Pos = Pos + "   J" + (i).ToString() + ": " + JointAngle[i].ToString("n4") + "\n";
            if (i != JointNumber)
                pos2file = pos2file + JointAngle[i].ToString() + ";";
            else
                pos2file = pos2file + JointAngle[i].ToString();
        }
        return Pos;
    }
}
