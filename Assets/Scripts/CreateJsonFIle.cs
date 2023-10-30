using System;
using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.U2D.Aseprite;*/
using UnityEngine;
using System.IO;

public class CreateJsonFIle : MonoBehaviour
{
    public string filePath = "Assets/Json/tags.json"; // Sp�cifiez le chemin du fichier JSON.

    private void Start()
    {
        // V�rifiez si le fichier JSON existe d�j�.
        if (!File.Exists(filePath))
        {
            CreateTagsFile();
        }
        else
        {
            /*Debug.Log("Le fichier JSON existe d�j�.");*/
        }
    }

    public void CreateTagsFile()
    {
        TagData tagData = new TagData();
        tagData.tags = new List<string> { "Tag1", "Tag2", "Tag3" }; // Ajoutez les tags initiaux ici.

        string jsonData = JsonUtility.ToJson(tagData, true); // La deuxi�me argument est pour l'indentation.

        try
        {
            File.WriteAllText(filePath, jsonData);
            Debug.Log("Fichier JSON de tags cr�� avec succ�s !");
        }
        catch (Exception e)
        {
            Debug.LogError("Erreur lors de la cr�ation du fichier JSON : " + e.Message);
        }
    }
}
