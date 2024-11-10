using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using TMPro;
using System;

public class TPManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private JSONManipulation JM;

    [Space(5)][Header("TEACH PENDANT CANVAS")]
    [Space(5)][Header("Main Canvas Buttons")]    //Canvas Main Buttons
    public Button BtnReturnProgramList;
    public Button BtnOpenProgram, BtnKeyboard , playProgramButton, playFromJSONButton;

    [Space(10)][Header("Navigation Buttons")]    //Navigation Buttons
    public Button upLinePointButton;
    public Button downLinePointButton;
    public Button upLineProgramButton;
    public Button downLineProgramButton;

    [Space(10)][Header("Submit Canvas Buttons")]  //Canvas Submit Buttons
    public Button createPointButton;
    public Button ConfirmDeletePointButton, SubmitProgramButton;

    [Space(10)][Header("UI Elements")]
    public Slider SpeedSlider;
    public Toggle MovToggle;

    [Space(10)][Header("Canvas GameObjects")]
    public GameObject CanvasProgramList;
    public GameObject CanvasProgramEdit, CanvasMenu, CanvasOpPanel, CanvasSides, CanvasKeyboard;

    [HideInInspector] public int currentPointLine = 0;
    [HideInInspector] public int currentProgramLine = 0;
    [HideInInspector] public string[] linesPoint;
    [HideInInspector] public string[] linesProgram;
    #endregion

    #region Start/Update
    private void Start(){
        //Canvas Main Methods
        BtnReturnProgramList.onClick.AddListener(OpenCanvasProgramList);
        BtnOpenProgram.onClick.AddListener(OpenCanvasProgramEdit);
        BtnKeyboard.onClick.AddListener(OpenCanvasKeyboard);

        //Canvas Submit Buttons
        createPointButton.onClick.AddListener(JM.createPoint);
        // ConfirmDeletePointButton.onClick.AddListener(DeleteLine);
        // SubmitProgramButton.onClick.AddListener(JM.createProgram);

        //Navigation Methods
        upLinePointButton.onClick.AddListener(PrevLinePoint);
        downLinePointButton.onClick.AddListener(NextLinePoint);

        upLineProgramButton.onClick.AddListener(PrevLineProgram);
        downLineProgramButton.onClick.AddListener(NextLineProgram);

        playProgramButton.onClick.AddListener(JM.PlayProgram);
        playFromJSONButton.onClick.AddListener(JM.PlayProgramFromJSON);

        linesPoint = JM.TextPoints.text.Split('\n');
        linesProgram = JM.TextPrograms.text.Split('\n');
    }

    private void Update(){
        if(CanvasProgramList.activeSelf || CanvasProgramEdit.activeSelf || CanvasMenu.activeSelf || CanvasOpPanel.activeSelf){
            CanvasSides.SetActive(true);
        }else{
            CanvasSides.SetActive(false);
        }

        if(CanvasProgramList.activeSelf || CanvasProgramEdit.activeSelf || CanvasMenu.activeSelf){
            CanvasOpPanel.SetActive(false);
        }
    }
    #endregion

    #region TP Canvas Methods
    public void OpenCanvasProgramList(){
        CanvasProgramList.SetActive(true);
        CanvasProgramEdit.SetActive(false);
    }

    public void OpenCanvasProgramEdit(){
        if(currentProgramLine >=0 && currentProgramLine < JM.factory.Robots[JM.activeRobot].Programs.Count){
            Program currentProgram = JM.factory.Robots[JM.activeRobot].Programs[currentProgramLine];
            if (currentProgram.Name != null){
                CanvasProgramEdit.SetActive(true);
                CanvasProgramList.SetActive(false);
                UpdateTextPoints(currentProgramLine);
            }
        }
    }

    public void OpenCanvasKeyboard(){
        CanvasKeyboard.SetActive(true);
    }

    public void OpenCanvasSides(){
        CanvasSides.SetActive(true);
    }
    #endregion

    #region TP Navigation Methods
    public void PrevLinePoint(){
        currentPointLine--;
        if (currentPointLine < 0){
            currentPointLine = linesPoint.Length - 1;
        }
        JM.TextPoints.text = UpdateDisplay(JM.TextPoints.text, currentPointLine, linesPoint);
    }

    public void NextLinePoint(){
        currentPointLine++;
        if (currentPointLine >= linesPoint.Length){
            currentPointLine = 0;
        }
        JM.TextPoints.text = UpdateDisplay(JM.TextPoints.text, currentPointLine, linesPoint);
    }

    public void PrevLineProgram(){
        currentProgramLine--;
        if (currentProgramLine < 0){
            currentProgramLine = linesProgram.Length - 1;
        }
        JM.TextPrograms.text = UpdateDisplay(JM.TextPrograms.text, currentProgramLine, linesProgram);
    }

    public void NextLineProgram(){
        currentProgramLine++;
        if (currentProgramLine >= linesProgram.Length){
            currentProgramLine = 0;
        }
        JM.TextPrograms.text = UpdateDisplay(JM.TextPrograms.text, currentProgramLine, linesProgram);
    }

    //ainda tem PROBLEMA no DELETE POINT!! Esta perdendo o indice correto ao apagar um ponto
    public void DeleteLine(string TextToUpdate, int currentLine){
        // Remove the line at the current position from the TMP object
        linesPoint = TextToUpdate.Split('\n');
        if (currentLine >= 0 && currentLine < linesPoint.Length){
            linesPoint = linesPoint.Where((val, idx) => idx != currentLine).ToArray();
            JM.TextPoints.text = string.Join("\n", linesPoint).Trim();
            if (currentLine >= linesPoint.Length){
                currentLine = linesPoint.Length - 1;
            }
        }
        
        // Remove the line at the current position from the JSON file
        if (linesPoint.Length == 0 || currentLine < 0 || currentLine >= linesPoint.Length) return; // Check that there is a selected line
        
        Program currentProgram = JM.factory.Robots[JM.activeRobot].Programs[JM.factory.Robots[JM.activeRobot].Programs.Count - 1];

        Point pointToRemove = currentProgram.Points[currentLine];
        currentProgram.Points.Remove(pointToRemove); // Remove the point from the program

        // Serialize the Factory object back to JSON and write to file
        string json = JsonUtility.ToJson(JM.factory);
        File.WriteAllText(JM.filePath, json);

        JM.TextPoints.text = UpdateDisplay(JM.TextPoints.text, currentLine, linesPoint);
        //CanvasProgramEdit.SetActive(false);
    }

    public string UpdateDisplay(string TextToUpdate, int currentLine, string[] lines)
    {
        string TextPoints_raw = Regex.Replace(TextToUpdate, "<.*?>", string.Empty);
        lines = TextPoints_raw.Split('\n');

        StringBuilder displayString = new StringBuilder();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            string formattedLine = i == currentLine
                ? $"<b><color=#000000>{line}</color></b>"
                : line;

            displayString.AppendLine(formattedLine);
        }

        return displayString.ToString().TrimEnd('\n');
    }

    public void UpdateTextPoints(int programIndex)
    {
        Program currentProgram = JM.factory.Robots[JM.activeRobot].Programs[programIndex];
        StringBuilder pointDetails = new StringBuilder();

        if(currentProgram != null){
            foreach (Point point in currentProgram.Points){
                string linearStr = point.MovementType ? "MOVL  " : "MOVJ  ";
                pointDetails.AppendLine(point.Name + "\t" + linearStr + "\tv=" + point.Speed.ToString("0.00"));
            }
            JM.TextProgramName.text = "Programa " + programIndex;
        }
        JM.TextPoints.text = pointDetails.ToString();
    }

    public void StartTextPrograms()
    {   
        Robot currentRobot = JM.factory.Robots[JM.activeRobot];
        foreach (Program program in currentRobot.Programs)
        {
            DateTime currentDate = DateTime.Now;  // Get current date and time
            string dateStr = currentDate.ToString("dd-MM-yy");  // Format date
            string timeStr = currentDate.ToString("HH:mm:ss");  // Format time

            if(program.Name.Length < (14-8)){ // //se a string program.Name eh menor que 6 caracteres, da 2 tabs
            JM.TextPrograms.text = program.Name.PadRight(14) + "\t\t" + dateStr + "\t" + timeStr + "\n";
            }else if(program.Name.Length < 14){ // se tem entre 8 e 14 caracteres, da um tab
            JM.TextPrograms.text = program.Name.PadRight(14) + "\t" + dateStr + "\t" + timeStr + "\n";
            }else{ //se eh maior que 14, trunca ela string a ate o caractere 14
            JM.TextPrograms.text = program.Name.Substring(0, 14) + "\t" + dateStr + "\t" + timeStr + "\n";
            }
        }
    }

    public void UpdateTextPrograms(Program program)
    {
        DateTime currentDate = DateTime.Now;  // Get current date and time
        string dateStr = currentDate.ToString("dd-MM-yy");  // Format date
        string timeStr = currentDate.ToString("HH:mm:ss");  // Format time

        if(program.Name.Length < (14-8)){ // //se a string program.Name eh menor que 6 caracteres, da 2 tabs
        JM.TextPrograms.text += program.Name.PadRight(14) + "\t\t" + dateStr + "\t" + timeStr + "\n";
        }else if(program.Name.Length < 14){ // se tem entre 8 e 14 caracteres, da um tab
        JM.TextPrograms.text += program.Name.PadRight(14) + "\t" + dateStr + "\t" + timeStr + "\n";
        }else{ //se eh maior que 14, trunca ela string a ate o caractere 14
        JM.TextPrograms.text += program.Name.Substring(0, 14) + "\t" + dateStr + "\t" + timeStr + "\n";
        }
    }
    #endregion
}