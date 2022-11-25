using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveController : MonoBehaviour
{
    string saveFile = "";

    private void Awake()
    {
        //saveFile = Application.persistentDataPath + "/Data.txt";
    }

    public string ReadFile()
    {
        var result = File.ReadAllText(saveFile);
        return result;
    }

    public void WriteFile(string jsonString, string fileName)
    {
        saveFile = Application.persistentDataPath + "/" + AddExtension(fileName);

        File.WriteAllText(saveFile, jsonString);
    }
    
    public string WriteFile(Texture2D texture)
    {
        var imageBytes = texture.EncodeToPNG();

        if (imageBytes.Length == 0)
        {
            return "";
        }

        var filename = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".png";
        var filepath = Path.Combine(Application.persistentDataPath, filename);

        try
        {
            File.WriteAllBytes(filepath, imageBytes);
            return filepath;
        }
        catch (Exception e)
        {
            return "";
        }
    }

    private string AddExtension(string filename)
    {
        return filename + ".txt";
    }
}
