using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeMovMode : MonoBehaviour
{
    public Button btnJog;
    public Button btnHand;
    public Button[] allButtons;

    private Color originalJogColor;
    private Color originalHandColor;

    public Color pressedBtnColor;

    void Start()
    {
        originalJogColor = btnJog.GetComponent<Image>().color;
        originalHandColor = btnHand.GetComponent<Image>().color;

        btnJog.onClick.AddListener(ActivateJogMode);
        btnHand.onClick.AddListener(ActivateHandMode);
    }

    void ActivateJogMode()
    {
        SetButtonColor(btnJog, pressedBtnColor);
        SetButtonColor(btnHand, originalHandColor);
        SetEventTriggerStates(true);
    }

    void ActivateHandMode()
    {
        SetButtonColor(btnJog, originalJogColor);
        SetButtonColor(btnHand, pressedBtnColor);
        SetEventTriggerStates(false);
    }

    void SetButtonColor(Button button, Color color)
    {
        button.GetComponent<Image>().color = color;
    }

    void SetEventTriggerStates(bool isJogMode)
    {
        foreach (Button button in allButtons)
        {
            EventTrigger[] eventTriggers = button.GetComponents<EventTrigger>();
            if (eventTriggers.Length >= 2)
            {
                eventTriggers[1].enabled = isJogMode;
                eventTriggers[0].enabled = !isJogMode;
            }
            else
            {
                Debug.LogWarning($"O botão {button.name} não tem dois EventTriggers configurados.");
            }
        }
    }
}