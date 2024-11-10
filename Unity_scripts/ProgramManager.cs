using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgramManager : MonoBehaviour
{
    [SerializeField] private Factory factory; // Reference to the Factory object
    [SerializeField] private Canvas programListCanvas; // Reference to the Canvas
    [SerializeField] private GameObject programListItemPrefab; // Prefab for each program list item
    [SerializeField] private GameObject robotGameObject;
    private Robo robo;

    private List<Program> programs; // Internal list of programs

    private void Awake()
    {
        programs = new List<Program>(); // Initialize the list on Awake
        robo = robotGameObject.GetComponent<Robo>();
    }

    #region Create Program
    public void CreateProgram(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Program name cannot be empty.");
            return;
        }

        // Create a new program
        Program newProgram = new Program(name);

        // Add the program to the factory's selected robot (if any)
        if (robo != null)
        {
            robo.AddProgram(newProgram);
        }

        // Add the program to the internal list
        programs.Add(newProgram);

        // Update the program list display (assuming you have a program to handle this)
        UpdateProgramListDisplay();
    }
    #endregion

    #region Read Program
    public List<Program> GetPrograms()
    {
        return programs; // Return the copy of the internal list
    }

    private void UpdateProgramListDisplay()
    {
        // Clear the existing program list items
        foreach (Transform child in programListCanvas.transform)
        {
            if (child.tag == "ProgramListItem")
            {
                Destroy(child.gameObject);
            }
        }

        // Iterate through the programs and create list items
        foreach (Program program in programs)
        {
            GameObject programListItem = Instantiate(programListItemPrefab, programListCanvas.transform);
            programListItem.tag = "ProgramListItem";

            // Assuming you have text elements on the programListItemPrefab:
            programListItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = program.Name;
        }
    }
    #endregion

    #region Update Program
    public void UpdateProgram(Program program, string newName)
    {
        if (string.IsNullOrEmpty(newName))
        {
            Debug.LogError("Program name cannot be empty.");
            return;
        }

        if (!programs.Contains(program))
        {
            Debug.LogError("Program to update not found.");
            return;
        }

        // Update the program name
        program.Name = newName;

        // Update the existing program list item (assuming you have a program to handle this)
        UpdateProgramListItem(program);
    }

    private void UpdateProgramListItem(Program program)
    {
        // Find the corresponding list item
        foreach (Transform child in programListCanvas.transform)
        {
            if (child.tag == "ProgramListItem" && child.GetChild(0).GetComponent<TextMeshProUGUI>().text == program.Name)
            {
                // Update the name text
                child.GetChild(0).GetComponent<TextMeshProUGUI>().text = program.Name;
                break;
            }
        }
    }
    #endregion

    #region Delete Program
    public void DeleteProgram(Program program)
    {
        if (!programs.Contains(program))
        {
            Debug.LogError("Program to delete not found.");
            return;
        }

        // Remove the program from the internal list
        programs.Remove(program);

        // Remove the program from the selected robot's list (if any)
        if (robo != null)
        {
            robo.Programs.Remove(program);
        }

        // Update the program list display (assuming you have a program to handle this)
        UpdateProgramListDisplay();
    }
    #endregion
}
