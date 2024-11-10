using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialEureka : MonoBehaviour
{
    [SerializeField] public List<Button> buttons;
    [SerializeField] public List<Button> allButtons;
    [SerializeField] public Color highlightedColor;
    [SerializeField] public Color normalColor;
    //instructions
    [SerializeField] private List<string> instructions;
    [SerializeField] private GameObject curvedCanvas;
    [SerializeField] private TextMeshProUGUI instructionText;
    //fim do tutorial
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject darkPanel;
    [SerializeField] private Button exitButton;

    public int currentButtonIndex = 0;

    private void Start()
    {
        foreach (Button button in buttons){
            button.onClick.AddListener(ButtonPressed);
        }
        
        // Inicializa o tutorial, destacando o primeiro botão
        HighlightButton(currentButtonIndex);
        DisableOtherButtons();
    }

    public void ButtonPressed()
    {
        currentButtonIndex++;

        //aciona o dark panel para facilitar o foco do usuario
        if(currentButtonIndex == 1){
            darkPanel.SetActive(true);
        }

        if (currentButtonIndex < buttons.Count)
        {
            HighlightButton(currentButtonIndex);
            DisableOtherButtons();
        }
        else
        {
            // Tutorial completo, habilita todos os botões
            ShowExitButton();
        }
    }

    private void ShowExitButton()
    {
        instructionPanel.SetActive(true);
        instructionText.text = "Tutorial concluído!\n\nPressione 'SAIR' para retornar ao menu principal.";

        exitButton.gameObject.SetActive(true);
        curvedCanvas.gameObject.SetActive(false);
    }


    private void HighlightButton(int index)
    {
        // Destaca o botão atual e retorna os outros à cor normal
        foreach (Button button in buttons)
        {
            button.image.color = normalColor;
        }
        buttons[index].image.color = highlightedColor;
        instructionText.text = instructions[index];
    }

    private void DisableOtherButtons()
    {

        // Desabilita todos os botões encontrados
        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }

        // Habilita apenas o botão atual na lista de botões
        if (buttons.Count > 0 && currentButtonIndex >= 0 && currentButtonIndex < buttons.Count)
        {
            buttons[currentButtonIndex].interactable = true;
        }
    }

}