using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class JSONSaveOps : MonoBehaviour
{
    [Space(5)][Header("Persistent Data Script")]
    public FilePersistentData PD;
    public Button SaveOp1Button;
    public Button SaveOp2Button;
    public Button SaveOp3Button;

    [SerializeField] public string originalFilePath;
    [SerializeField] public string saveOp1Path;
    [SerializeField] public string saveOp2Path;
    [SerializeField] public string saveOp3Path;


    private void Start(){
        originalFilePath = PD.PersistentPath;
        saveOp1Path = PD.PersistentPathOp1;
        saveOp2Path = PD.PersistentPathOp2;
        saveOp3Path = PD.PersistentPathOp3;
        SaveOp1Button.onClick.AddListener(SaveOp1);
        SaveOp2Button.onClick.AddListener(SaveOp2);
        SaveOp3Button.onClick.AddListener(SaveOp3);
    }

    public void SaveOp1()
    {
        if (string.IsNullOrEmpty(originalFilePath) || string.IsNullOrEmpty(saveOp1Path))
        {
            Debug.LogError("Caminhos dos arquivos não foram definidos.");
            return;
        }

        try
        {
            File.Copy(originalFilePath, saveOp1Path, true);
            Debug.Log($"Arquivo salvo com sucesso: {saveOp1Path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao salvar arquivo: {ex.Message}");
        }
    }

    public void SaveOp2()
    {
        if (string.IsNullOrEmpty(originalFilePath) || string.IsNullOrEmpty(saveOp2Path))
        {
            Debug.LogError("Caminhos dos arquivos não foram definidos.");
            return;
        }

        try
        {
            File.Copy(originalFilePath, saveOp2Path, true);
            Debug.Log($"Arquivo salvo com sucesso: {saveOp2Path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao salvar arquivo: {ex.Message}");
        }
    }

    public void SaveOp3()
    {
        if (string.IsNullOrEmpty(originalFilePath) || string.IsNullOrEmpty(saveOp3Path))
        {
            Debug.LogError("Caminhos dos arquivos não foram definidos.");
            return;
        }

        try
        {
            File.Copy(originalFilePath, saveOp3Path, true);
            Debug.Log($"Arquivo salvo com sucesso: {saveOp3Path}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao salvar arquivo: {ex.Message}");
        }
    }
}
