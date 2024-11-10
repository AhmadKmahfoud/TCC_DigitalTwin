using UnityEngine;
using System.IO;
using System;

public class FilePersistentData : MonoBehaviour
{
    public string PersistentPath { get; private set; }
    public string PersistentPathSaved { get; private set; }
    public string PersistentPathOp1 { get; private set; }
    public string PersistentPathOp2 { get; private set; }
    public string PersistentPathOp3 { get; private set; }

    void Awake()
    {
        InitializePaths();
        CreateDirectoriesAndFiles();
    }

    void InitializePaths()
    {
        PersistentPath = Path.Combine(Application.persistentDataPath, "UnityJSON_gameData", "RobotsINFO.json");
        PersistentPathSaved = Path.Combine(Application.persistentDataPath, "UnityJSON_gameData", "JsonSAVED.json");

        PersistentPathOp1 = Path.Combine(Application.persistentDataPath, "UnityJSON_gameData", "JsonOp1.json");
        PersistentPathOp2 = Path.Combine(Application.persistentDataPath, "UnityJSON_gameData", "JsonOp2.json");
        PersistentPathOp3 = Path.Combine(Application.persistentDataPath, "UnityJSON_gameData", "JsonOp3.json");
    }

    void CreateDirectoriesAndFiles()
    {
        string[] pathsToCreate = new string[]
        {
            Path.GetDirectoryName(PersistentPath),
            Path.GetDirectoryName(PersistentPathSaved),
            Path.GetDirectoryName(PersistentPathOp1),
            Path.GetDirectoryName(PersistentPathOp2),
            Path.GetDirectoryName(PersistentPathOp3)
        };

        foreach (var path in pathsToCreate)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Debug.Log($"Criou diretório: {path}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError($"Não foi possível criar o diretório {path}. Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erro ao criar diretório {path}: {ex.Message}");
            }
        }

        CreateEmptyFileIfNotExists(PersistentPath);
        CreateEmptyFileIfNotExists(PersistentPathSaved);
        CreateEmptyFileIfNotExists(PersistentPathOp1);
        CreateEmptyFileIfNotExists(PersistentPathOp2);
        CreateEmptyFileIfNotExists(PersistentPathOp3);
    }

    void CreateEmptyFileIfNotExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            try
            {
                File.Create(filePath).Close();
                Debug.Log($"Criou arquivo vazio: {filePath}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.LogError($"Não foi possível criar o arquivo {filePath}. Erro: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erro ao criar arquivo {filePath}: {ex.Message}");
            }
        }
    }
}
