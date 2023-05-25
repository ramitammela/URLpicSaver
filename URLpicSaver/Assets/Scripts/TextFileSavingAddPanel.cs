using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class TextFileSavingAddPanel : MonoBehaviour
{
    public int collectionNumber;

    [Header("UI")]
    public InputField inputFieldURL;
    public InputField inputFieldTag;
    public InputField inputFieldName;

    public Text previewText;
    public OnlineImage previewImage;

    public Text savingMessage;

    [Header("Settings")]
    public List<string> lines = new List<string>();
    private string[] textFileLines;
    private string targetString;

    private int existsInt = 0;

    void Awake()
    {
        collectionNumber = PlayerPrefs.GetInt("CollectionNumber");
        if (collectionNumber == 0)  { collectionNumber = 1;}
    }

    void Start()
    {
        #region folder

        if (!System.IO.Directory.Exists(Application.dataPath + "/../" + "Collection " + collectionNumber + "/"))   // Folder path
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/../Collection " + collectionNumber + "/");   //  Create Folder
        }

        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";   // Text file path

        if (!File.Exists(txtDocumentName))  //  Check if text file exist
        {
            File.WriteAllText(txtDocumentName, "");     // Write text file
        }

        textFileLines = System.IO.File.ReadAllLines(txtDocumentName);

        #endregion
    }

    public void RefreshRead()
    {
        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";    // PhotoWidget/CustomUI/

        textFileLines = System.IO.File.ReadAllLines(txtDocumentName);
        lines = new List<string>(textFileLines);
        lines.RemoveAll(s => s == "");   // Remove Empty Elements from list
    }


    //bool existCheck;
    public void AddToFile()
    {
        existsInt = 0;

        if (inputFieldURL.text == "")
        {
            return;
        }

        RefreshRead();

        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";    // PhotoWidget/CustomUI/

        string addText = inputFieldURL.text + " " + "[TAG:" + inputFieldTag.text + "]" + " {NAME:" + inputFieldName.text + "}";
        targetString = addText; // väliaikainen

        foreach (string x in lines)
        {
            if (x.Equals(targetString))
            {
                //existCheck = true;
                existsInt++;
            }
            else
            {
                //existCheck = false;
            }
        }

        if (existsInt > 0)
        {
            savingMessage.enabled = true;
            savingMessage.text = "Already Exists";
            Invoke("RemoveSavedText", 1.0f);
            return;

        }
        else
        {
            File.AppendAllText(txtDocumentName, addText + "\n");
            savingMessage.enabled = true;
            savingMessage.text = "Saved";
            Invoke("RemoveSavedText", 1.0f);
            RefreshRead();
        }
    }

    private void RemoveSavedText()
    {
        savingMessage.enabled = false;
    }

    public void UpdateString()
    {
        targetString = inputFieldURL.text + " " + "[TAG:" + inputFieldTag.text + "]" + " {NAME:" + inputFieldName.text + "}";

        previewText.text = targetString;
        previewImage.imageURL = inputFieldURL.text;
        
    }

    public void OpenFolder()
    {
        string folderPath = Application.dataPath + "/../" + "Collection " + collectionNumber + "/"; 
        Application.OpenURL(folderPath);
    }
}