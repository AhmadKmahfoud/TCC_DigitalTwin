using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class JSONsaveFile : MonoBehaviour
{
    [Space(5)][Header("Persistent Data Script")]
    public FilePersistentData PD;
    public Button SaveAndCloseButton;

    [SerializeField] public string originalFilePath;
    [SerializeField] public string saveFilePath;


    private void Start(){
        originalFilePath = PD.PersistentPath;
        saveFilePath = PD.PersistentPathSaved;
        SaveAndCloseButton.onClick.AddListener(SaveJSON);
    }

    public void SaveJSON()
    {
        if (string.IsNullOrEmpty(originalFilePath) || string.IsNullOrEmpty(saveFilePath))
        {
            Debug.LogError("Caminhos dos arquivos não foram definidos.");
            return;
        }

        try
        {
            File.Copy(originalFilePath, saveFilePath, true);
            Debug.Log($"Arquivo salvo com sucesso: {saveFilePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao salvar arquivo: {ex.Message}");
        }
    }
}
