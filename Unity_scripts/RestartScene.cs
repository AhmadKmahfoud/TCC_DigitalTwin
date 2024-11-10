using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    public void Start()
    {
        exitButton.onClick.AddListener(RestartSceneFunc);
    }

    private void RestartSceneFunc()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
