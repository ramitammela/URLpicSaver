using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Linq;

public class TextFileSaving : MonoBehaviour
{
    public GameObject[] panels;

    public int collectionNumber;
    public Image collection1Selected, collection2Selected, collection3Selected;

    public InputField inputFieldCollection1Rename, inputFieldCollection2Rename, inputFieldCollection3Rename;

    [Header("Settings")]
    public List<string> lines = new List<string>();
    private string[] textFileLines;
    private string targetString;
    public Image[] images;
    public Image previewImageBig;
    public Text textTotal;  //  How many lines in text file
    public Text textTotalFiltered;  //  How many lines in filtered results
    public int existsInt = 0;

    public InputField inputFieldBookmark;
    public Button buttonBookmarks;
    public Image showBookmarksButtonCheck;

    [Header("Filtering")]
    public bool filterByTag;
    public Toggle tagToggle;
    public InputField inputfieldSearchTag;
    public List<string> filteredLines = new List<string>(); // Make new list for lines that contains the tag
    public string tagToSearch;

    [Header("Filtering - Names")]
    public bool filterByName;
    public Toggle nameToggle;
    public InputField inputfieldSearchName;
    public List<string> filteredLinesNames = new List<string>(); // Make new list for lines that contains the name
    public string nameToSearch;

    [Space(10)]

    public List<string> filteredLinesBoth = new List<string>(); // Make new list for lines that contains both the tag and name

    [Header("OnlineImages")]
    public int selectedNumber;
    public OnlineImage onlineImage1;
    public OnlineImage onlineImage2;
    public OnlineImage onlineImage3;
    public OnlineImage onlineImage4;
    public OnlineImage onlineImage5;

    [Header("Selected Photo")]
    public OnlineImage onlineImage;
    public string selectedURL;
    public string selectedURLClean;
    public string selectedTag;
    public string selectedName;

    [Header("Selected Photo Panel")]
    public GameObject selectedPhotoPanel;
    public Text selectedPanelURL;
    public Text selectedPanelTag;
    public Text selectedPanelName;

    [Header("Editing - Edit Panel")]
    public Text oldLine;
    public Text newLine;
    public string editedLine;

    [Space(10)]
    public Text oldURL;
    public Text newURL;
    public InputField inputFieldNewURL;
    [Space(5)]
    public Text oldTag;
    public Text newTag;
    public InputField inputFieldNewTag;
    [Space(5)]
    public Text oldName;
    public Text newName;
    public InputField inputFieldNewName;

    [Header("Error - Remove Panel")]
    public Text errorRemovePanelURL;
    public Text errorRemovePanelError;


    [Header("Copying")]
    public bool copyNames;
    public string copyString;
    public Text copyingDoneText;

    [Header("Button Tags")]
    public string[] buttonTags;
    public List<string> buttonTagsLines = new List<string>();

    public GameObject buttongTagPrefab;
    public Transform buttonTagsTarget;
    public InputField inputFieldButtonTag;

    [Header("Button Names")]
    public string[] buttonNames;
    public List<string> buttonNamesLines = new List<string>();

    public GameObject buttonNamePrefab;
    public Transform buttonNamesTarget;
    public InputField inputFieldButtonName;

    [Header("Bookmark URLs")]
    public string selectedBookmark;
    public string[] bookmarkURLS;
    public List<string> bookmarkULRLines = new List<string>();

    public GameObject bookmarkPrefab;
    public Transform bookmarksTarget;


    string newEditURL;
    string newEditTag;
    string newEditName;


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

        InputfieldEdit(0);

        // Button Tags
        string tagsTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Tags.txt";   // Text file path

        if (!File.Exists(tagsTxtFile))  //  Check if text file exist
        {
            File.WriteAllText(tagsTxtFile, "");     // Write text file
        }

        buttonTags = System.IO.File.ReadAllLines(tagsTxtFile);

        ButtongTagLoad();

        // Button Names
        string namesTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Names.txt";   // Text file path

        if (!File.Exists(namesTxtFile))  //  Check if text file exist
        {
            File.WriteAllText(namesTxtFile, "");     // Write text file
        }

        buttonNames = System.IO.File.ReadAllLines(namesTxtFile);

        ButtonNameLoad();

        // Bookmark URLS
        string bookmarksTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Bookmarks.txt";   // Text file path

        if (!File.Exists(bookmarksTxtFile))  //  Check if text file exist
        {
            File.WriteAllText(bookmarksTxtFile, "");     // Write text file
        }

        bookmarkURLS = System.IO.File.ReadAllLines(bookmarksTxtFile);

        BookmarksLoad();

        int x = PlayerPrefs.GetInt("ShowBookmarksButton");
        if (x == 0)
        {   ShowBookmarksButton(false); }
        else if (x == 1)
        {   ShowBookmarksButton(true);  }


        // Close Panels
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);

            if (panel.transform.name == "Panel_Add")    {   panel.SetActive(true);   }
        }

        // Collections Panel
        string collection1Name = PlayerPrefs.GetString("Collection1Name");   
        string collection2Name = PlayerPrefs.GetString("Collection2Name");   
        string collection3Name = PlayerPrefs.GetString("Collection3Name");   
        if (collection1Name != "")  { inputFieldCollection1Rename.transform.Find("Placeholder").transform.GetComponent<Text>().text = collection1Name;  }
        if (collection2Name != "")  { inputFieldCollection2Rename.transform.Find("Placeholder").transform.GetComponent<Text>().text = collection2Name;  }
        if (collection3Name != "")  { inputFieldCollection3Rename.transform.Find("Placeholder").transform.GetComponent<Text>().text = collection3Name;  }
        

        #endregion
    }

    void Update()
    {
        #region selections

        if (previewImageBig.gameObject.activeInHierarchy == false)
        {
            if (selectedNumber == 1)
            {
                onlineImage1.transform.GetComponent<Outline>().enabled = true;
                onlineImage2.transform.GetComponent<Outline>().enabled = false;
                onlineImage3.transform.GetComponent<Outline>().enabled = false;
                onlineImage4.transform.GetComponent<Outline>().enabled = false;
                onlineImage5.transform.GetComponent<Outline>().enabled = false;
                selectedPhotoPanel.SetActive(true);
            }
            else if (selectedNumber == 2)
            {
                onlineImage1.transform.GetComponent<Outline>().enabled = false;
                onlineImage2.transform.GetComponent<Outline>().enabled = true;
                onlineImage3.transform.GetComponent<Outline>().enabled = false;
                onlineImage4.transform.GetComponent<Outline>().enabled = false;
                onlineImage5.transform.GetComponent<Outline>().enabled = false;
                selectedPhotoPanel.SetActive(true);
            }
            else if (selectedNumber == 3)
            {
                onlineImage1.transform.GetComponent<Outline>().enabled = false;
                onlineImage2.transform.GetComponent<Outline>().enabled = false;
                onlineImage3.transform.GetComponent<Outline>().enabled = true;
                onlineImage4.transform.GetComponent<Outline>().enabled = false;
                onlineImage5.transform.GetComponent<Outline>().enabled = false;
                selectedPhotoPanel.SetActive(true);
            }
            else if (selectedNumber == 4)
            {
                onlineImage1.transform.GetComponent<Outline>().enabled = false;
                onlineImage2.transform.GetComponent<Outline>().enabled = false;
                onlineImage3.transform.GetComponent<Outline>().enabled = false;
                onlineImage4.transform.GetComponent<Outline>().enabled = true;
                onlineImage5.transform.GetComponent<Outline>().enabled = false;
                selectedPhotoPanel.SetActive(true);
            }
            else if (selectedNumber == 5)
            {
                onlineImage1.transform.GetComponent<Outline>().enabled = false;
                onlineImage2.transform.GetComponent<Outline>().enabled = false;
                onlineImage3.transform.GetComponent<Outline>().enabled = false;
                onlineImage4.transform.GetComponent<Outline>().enabled = false;
                onlineImage5.transform.GetComponent<Outline>().enabled = true;
                selectedPhotoPanel.SetActive(true);
            }
            else if (selectedNumber == 0)
            {
                onlineImage1.transform.GetComponent<Outline>().enabled = false;
                onlineImage2.transform.GetComponent<Outline>().enabled = false;
                onlineImage3.transform.GetComponent<Outline>().enabled = false;
                onlineImage4.transform.GetComponent<Outline>().enabled = false;
                onlineImage5.transform.GetComponent<Outline>().enabled = false;
                selectedPhotoPanel.SetActive(false);
            }
        }

        #endregion

        if (!filterByName && !filterByTag)
        {
            textTotalFiltered.text = ""; 
        }
        else if (filterByName && filterByTag)
        {
            textTotalFiltered.text = "Filtered Total: " + filteredLinesBoth.Count;
        }
        else if (filterByName && !filterByTag)
        {
            textTotalFiltered.text = "Filtered Total: " + filteredLinesNames.Count;
        }
        else if (!filterByName && filterByTag)
        {
            textTotalFiltered.text = "Filtered Total: " + filteredLines.Count;
        }


        //  Image Viewer - change image with Arrow keys

        if (previewImageBig.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedNumber > 1) 
            { 
                selectedNumber -=1;
                SelectedNumber(selectedNumber); 
                ShowBigImage(selectedNumber);
                //print ("changed selected image: " + selectedNumber);
            }
            
        }
        if (previewImageBig.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedNumber < 5) 
            { 
                selectedNumber +=1;
                SelectedNumber(selectedNumber); 
                ShowBigImage(selectedNumber);
                //print ("changed selected image: " + selectedNumber);
            }
        }


    }


    #region Collections


    public void ButtonSelectCollection(int i)
    {
        PlayerPrefs.SetInt("CollectionNumber",i);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CollectionsCheck()
    {
        collectionNumber = PlayerPrefs.GetInt("CollectionNumber");

        if (collectionNumber == 1)
        {
            collection1Selected.enabled = true;
            collection2Selected.enabled = false;
            collection3Selected.enabled = false;
        }
        else if (collectionNumber == 2)
        {
            collection1Selected.enabled = false;
            collection2Selected.enabled = true;
            collection3Selected.enabled = false;
        }
        else if (collectionNumber == 3)
        {
            collection1Selected.enabled = false;
            collection2Selected.enabled = false;
            collection3Selected.enabled = true;
        }
    }

    public void CollectionSaveRename(int i)
    {
        if (i == 1 && inputFieldCollection1Rename.text != "")      {   PlayerPrefs.SetString("Collection1Name", inputFieldCollection1Rename.text); }
        else if (i == 2 && inputFieldCollection2Rename.text != "") {   PlayerPrefs.SetString("Collection2Name", inputFieldCollection2Rename.text); }
        else if (i == 3 && inputFieldCollection3Rename.text != "") {   PlayerPrefs.SetString("Collection3Name", inputFieldCollection3Rename.text); }
    }

    #endregion


    public void ButtonUpdateColorTheme()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ButtonChangeColorTheme(int newColor)
    {
        PlayerPrefs.SetInt("SelectedTheme", newColor);
    }


    #region Bookmarks


    //  Saved Bookmarks
    public void BookmarksLoad()
    {
        foreach (string x in bookmarkURLS)
        {
            GameObject y = Instantiate(bookmarkPrefab, transform.position, transform.rotation);
            y.transform.SetParent(bookmarksTarget);
            y.transform.localScale = new Vector3(1f, 1f, 1f);
            y.transform.GetComponent<Bookmark>().bookmark = x.ToString();
        }
    }

    bool bookmarkExists;
    public void AddBookmarkToFile()
    {
        bookmarkExists = false;

        if (inputFieldBookmark.text == "")
        {
            return;
        }

        string bookmarksTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Bookmarks.txt"; 

        string addText = inputFieldBookmark.text;

        foreach (string x in bookmarkURLS)
        {
            if (x.Equals(addText))
            {
                bookmarkExists = true;
            }
            else
            {
                bookmarkExists = false;
            }
        }

        if (bookmarkExists)
        {}
        else
        {
            File.AppendAllText(bookmarksTxtFile, addText + "\n");
            NewBookmarkSaved();

            bookmarkURLS = System.IO.File.ReadAllLines(bookmarksTxtFile);
        }
    }
    public void NewBookmarkSaved()
    {
        GameObject y = Instantiate(bookmarkPrefab, transform.position, transform.rotation);
        y.transform.SetParent(bookmarksTarget);
        y.transform.localScale = new Vector3(1f, 1f, 1f);
        y.transform.GetComponent<Bookmark>().bookmark = inputFieldBookmark.text;
    }

    public void DeleteSelectedBookmark(string bookmark)
    {
        // 1. Clear 
        string bookmarksTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Bookmarks.txt";   // Text file path

        if (File.Exists(bookmarksTxtFile))  //  Check if text file exist
        {
            File.WriteAllText(bookmarksTxtFile, "");     // Write text file
        }

        // 2. Add All, except Selected
        foreach (string x in bookmarkURLS)     //  Add into text file
        {
            if (x != bookmark)
            {
                File.AppendAllText(bookmarksTxtFile, x + "\n");
            }
        }

        bookmarkURLS = System.IO.File.ReadAllLines(bookmarksTxtFile);
    }

    //  UI  -----------------------------------------------------------------

    public void ButtonShowBookmarksButton()
    {
        int x = PlayerPrefs.GetInt("ShowBookmarksButton");

        if (x == 0)
        {
            PlayerPrefs.SetInt("ShowBookmarksButton", 1); 
            ShowBookmarksButton(true); 
        }
        else if (x == 1)
        {
            PlayerPrefs.SetInt("ShowBookmarksButton", 0); 
            ShowBookmarksButton(false); 
        }
    }
    public void ShowBookmarksButton (bool show)
    {
        if (show)
        {
            showBookmarksButtonCheck.enabled = true;
            buttonBookmarks.transform.GetComponent<Image>().enabled = true;
            buttonBookmarks.transform.GetComponent<LayoutElement>().ignoreLayout = false;
            buttonBookmarks.transform.Find("Icon").transform.GetComponent<Image>().enabled = true;
        }
        else
        {
            showBookmarksButtonCheck.enabled = false;
            buttonBookmarks.transform.GetComponent<Image>().enabled = false;
            buttonBookmarks.transform.GetComponent<LayoutElement>().ignoreLayout = true;
            buttonBookmarks.transform.Find("Icon").transform.GetComponent<Image>().enabled = false;
        }
    }

    #endregion



    #region Button Tags & Button Names

    //  Saved Tags
    public void ButtongTagLoad()
    {
        foreach (string x in buttonTags)
        {
            GameObject y = Instantiate(buttongTagPrefab, transform.position, transform.rotation);
            y.transform.SetParent(buttonTagsTarget);
            y.transform.localScale = new Vector3(1f, 1f, 1f);
            y.transform.GetComponent<ButtonTag>().tag = x.ToString();
        }
    }


    bool tagExists;
    public void AddTagToFile()
    {
        tagExists = false;

        if (inputFieldButtonTag.text == "")
        {
            return;
        }

        string tagsTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Tags.txt"; 

        string addText = inputFieldButtonTag.text;

        foreach (string x in buttonTags)
        {
            if (x.Equals(addText))
            {
                tagExists = true;
            }
            else
            {
                tagExists = false;
            }
        }

        if (tagExists)
        {}
        else
        {
            File.AppendAllText(tagsTxtFile, addText + "\n");
            ButtonTagSavedNew();

            buttonTags = System.IO.File.ReadAllLines(tagsTxtFile);
        }
    }

    public void ButtonTagSavedNew()
    {
        GameObject y = Instantiate(buttongTagPrefab, transform.position, transform.rotation);
        y.transform.SetParent(buttonTagsTarget);
        y.transform.localScale = new Vector3(1f, 1f, 1f);
        y.transform.GetComponent<ButtonTag>().tag = inputFieldButtonTag.text;
    }


    public void DeleteSelectedButtonTag(string tag)
    {
        // 1. Clear 
        string tagsTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Tags.txt";   // Text file path

        if (File.Exists(tagsTxtFile))  //  Check if text file exist
        {
            File.WriteAllText(tagsTxtFile, "");     // Write text file
        }

        // 2. Add All, except Selected
        foreach (string x in buttonTags)     //  Add into text file
        {
            if (x != tag)
            {
                File.AppendAllText(tagsTxtFile, x + "\n");
            }
        }

        buttonTags = System.IO.File.ReadAllLines(tagsTxtFile);
    }


    //  Saved Names
    public void ButtonNameLoad()
    {
        foreach (string x in buttonNames)
        {
            GameObject y = Instantiate(buttonNamePrefab, transform.position, transform.rotation);
            y.transform.SetParent(buttonNamesTarget);
            y.transform.localScale = new Vector3(1f, 1f, 1f);
            y.transform.GetComponent<ButtonName>().name = x.ToString();
        }
    }


    bool nameExists;
    public void AddNameToFile()
    {
        nameExists = false;

        if (inputFieldButtonName.text == "")
        {
            return;
        }

        string namesTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Names.txt"; 

        string addText = inputFieldButtonName.text;

        foreach (string x in buttonNames)
        {
            if (x.Equals(addText))
            {
                nameExists = true;
            }
            else
            {
                nameExists = false;
            }
        }

        if (nameExists)
        {}
        else
        {
            File.AppendAllText(namesTxtFile, addText + "\n");
            ButtonNameSavedNew();

            buttonNames = System.IO.File.ReadAllLines(namesTxtFile);
        }
    }

    public void ButtonNameSavedNew()
    {
        GameObject y = Instantiate(buttonNamePrefab, transform.position, transform.rotation);
        y.transform.SetParent(buttonNamesTarget);
        y.transform.localScale = new Vector3(1f, 1f, 1f);
        y.transform.GetComponent<ButtonName>().name = inputFieldButtonName.text;
    }


    public void DeleteSelectedButtonName(string name)
    {
        // 1. Clear 
        string namesTxtFile = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Names.txt";   // Text file path

        if (File.Exists(namesTxtFile))  //  Check if text file exist
        {
            File.WriteAllText(namesTxtFile, "");     // Write text file
        }

        // 2. Add All, except Selected
        foreach (string x in buttonNames)     //  Add into text file
        {
            if (x != name)
            {
                File.AppendAllText(namesTxtFile, x + "\n");
            }
        }

        buttonNames = System.IO.File.ReadAllLines(namesTxtFile);
    }


    #endregion


    #region Filtering


    //  Filter By TAG

    public void FilterTagToggle()
    {
        filterByTag = tagToggle.isOn;
    }

    public void FilterButtonTest()
    {
        FilterByTagString(tagToSearch);
    }

    public void FilterByTagString (string x)
    {
        foreach (string y in lines)
        {
            if (y.Contains("[TAG:" + x + "]"))
            {
                filteredLines.Add(y);
            }
        }
    }
    public void FilterByTagInputfieldEnd()
    {
        if (inputfieldSearchTag.text != "")
        {
            filteredLines.Clear();
            tagToSearch = inputfieldSearchTag.text;
        }

        //  if Both settings are set On
        if (filterByTag & filterByName) // BOTH
        {
            filteredLinesBoth.Clear();

            foreach (string y in lines)
            {
                if (y.Contains("[TAG:" + tagToSearch + "]") && y.Contains("{NAME:" + nameToSearch + "}"))
                {
                    filteredLinesBoth.Add(y);
                }
            }
        }
    }

    // -------------------------------------------------------------------------------------

    //  Filter By Name

    public void FilterNameToggle()
    {
        filterByName = nameToggle.isOn;
    }

    public void FilterButtonTestNames()
    {
        FilterByNameString(nameToSearch);
    }

    public void FilterByNameString(string x)
    {
        foreach (string y in lines)
        {
            if (y.Contains("{NAME:" + x + "}"))
            {
                filteredLinesNames.Add(y);
            }
        }
    }
    public void FilterByNameInputfieldEnd()
    {
        if (inputfieldSearchName.text != "")
        {
            filteredLinesNames.Clear();
            nameToSearch = inputfieldSearchName.text;
        }

        //  if Both settings are set On
        if (filterByTag & filterByName) // BOTH
        {
            filteredLinesBoth.Clear();

            foreach (string y in lines)
            {
                if (y.Contains("[TAG:" + tagToSearch + "]") && y.Contains("{NAME:" + nameToSearch + "}"))
                {
                    filteredLinesBoth.Add(y);
                }
            }
        }
    }

    // -------------------------------------------------------------------------------------

    //  Updating if deleted/edited/added

    public void FilteredUpdate()
    {
        RefreshRead();
        FilterByTagInputfieldEnd();
        FilterByNameInputfieldEnd();
        
        FilterButtonTest();
        FilterButtonTestNames();
    }

    //  Filter by Text Button

    public void FilterByClickedText(int type, string searchString)
    {
        //print("Filtering by: " + type + " , " + searchString);

        if (type == 0)  //  Tag
        {
            if(searchString == tagToSearch) { filterByTag = false; tagToggle.isOn = false; tagToSearch = ""; inputfieldSearchTag.text = ""; }
            else
            { 
                filterByTag = true;
                tagToggle.isOn = true;
                inputfieldSearchTag.text = searchString;
                FilterByTagInputfieldEnd();
                FilterButtonTest();
            }
        }
        if (type == 1)  //  Name
        {
            if (searchString == nameToSearch) { filterByName = false; nameToggle.isOn = false; nameToSearch = ""; inputfieldSearchName.text = ""; }
            else
            {
                filterByName = true;
                nameToggle.isOn = true;
                inputfieldSearchName.text = searchString;
                FilterByNameInputfieldEnd();
                FilterButtonTestNames();
            }
        }
    }

    #endregion

    #region Editing

    public void StartEditing()
    {
        oldURL.text = selectedURLClean;
        oldTag.text = selectedTag;
        oldName.text = selectedName;
        
        newLine.text = selectedURL;
        ClearEdit();
    }


    public void ClearEdit()
    {
        editedLine = selectedURL;

        newURL.text = "";
        newTag.text = "";
        newName.text = "";

        inputFieldNewURL.text = "";
        inputFieldNewTag.text = "";
        inputFieldNewName.text = "";

        InputfieldEdit(0);

        newLine.text = editedLine;
    }

    public void ClearSelectedPart (int x)
    {
        if (x == 1)
        {
            newEditURL = "";
            editedLine = newEditURL + " " + "[TAG:" + newEditTag + "]" + " {NAME:" + newEditName + "}";
        }
        if (x == 2)
        {
            newEditURL = newEditURL.Replace(" ", "");

            newEditTag = "";
            editedLine = newEditURL + " " + "[TAG:" + newEditTag + "]" + " {NAME:" + newEditName + "}";
        }
        if (x == 3)
        {
            newEditURL = newEditURL.Replace(" ", "");

            newEditName = "";
            editedLine = newEditURL + " " + "[TAG:" + newEditTag + "]" + " {NAME:" + newEditName + "}";
        }

        newLine.text = editedLine;
    }


    public void InputfieldEdit(int x)
    {
        if (x == 1)
        {
            newURL.text = inputFieldNewURL.text;
            newEditURL = newURL.text;

            if (newURL.text == "") { newEditURL = selectedURLClean; newURL.text = newEditURL;  }
        }
        if (x == 2) 
        { 
            newTag.text = inputFieldNewTag.text;
            newEditTag = newTag.text;
        }
        if (x == 3)
        {
            newName.text = inputFieldNewName.text;
            newEditName = newName.text;
        }
        if (x == 0)
        {
            newURL.text = inputFieldNewURL.text;
            newEditURL = newURL.text;
            newTag.text = inputFieldNewTag.text;
            newEditTag = newTag.text;
            newName.text = inputFieldNewName.text;
            newEditName = newName.text;
        }

        EditAdd();
    }

    public void EditAdd()
    {
        if (newEditURL == "") { newEditURL = selectedURLClean; }
        if (newEditTag == "") { newEditTag = selectedTag; }
        if (newEditName == "") { newEditName = selectedName; }

        newEditURL = newEditURL.Replace(" ", "");

        editedLine = newEditURL + " " + "[TAG:" + newEditTag + "]" + " {NAME:" + newEditName + "}";

        newLine.text = editedLine;
    }


    public void SaveNewEdit()
    {
        // 1. Clear 
        ClearAll();

        // 2. Add All, except Selected
        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";

        File.AppendAllText(txtDocumentName, editedLine + "\n");

        foreach (string x in lines)     //  Add into text file
        {
            if (x != selectedURL)
            {
                File.AppendAllText(txtDocumentName, x + "\n");
            }
        }

        // 3. Update
        RefreshRead();
        ButtonReloadSelectedEditedImage();

        RefreshSelectPanel();
        RefreshRead();
    }


    public void ButtonReloadSelectedEditedImage()
    {
        RefreshSelectPanel();
        RefreshRead();

        if (selectedNumber == 1)
        {
            onlineImage1.LoadImageURL(newEditURL); SelectedNumber(1);

            if (newEditURL != "")
            {
                onlineImage1.imageURLClean = newEditURL;
            }

            onlineImage1.imageTag = newEditTag;
            onlineImage1.imageName = newEditName;
        }
        
        if (selectedNumber == 2)
        {
            onlineImage2.LoadImageURL(newEditURL); SelectedNumber(2);

            if (newEditURL != "")
            {
                onlineImage2.imageURLClean = newEditURL;
            }

            onlineImage2.imageTag = newEditTag;
            onlineImage2.imageName = newEditName;
        }
        if (selectedNumber == 3)
        {
            onlineImage3.LoadImageURL(newEditURL); SelectedNumber(3);

            if (newEditURL != "")
            {
                onlineImage3.imageURLClean = newEditURL;
            }

            onlineImage3.imageTag = newEditTag;
            onlineImage3.imageName = newEditName;
        }
        if (selectedNumber == 4)
        {
            onlineImage4.LoadImageURL(newEditURL); SelectedNumber(4);

            if (newEditURL != "")
            {
                onlineImage4.imageURLClean = newEditURL;
            }

            onlineImage4.imageTag = newEditTag;
            onlineImage4.imageName = newEditName;
        }
        if (selectedNumber == 5)
        {
            onlineImage5.LoadImageURL(newEditURL); SelectedNumber(5);

            if (newEditURL != "")
            {
                onlineImage5.imageURLClean = newEditURL;
            }

            onlineImage5.imageTag = newEditTag;
            onlineImage5.imageName = newEditName;
        }

        if (newEditURL != "") 
        { 
            selectedURLClean = newEditURL;
        }

        //RefreshSelectPanel();
        //RefreshRead();
    }

    #endregion


    public void DeleteSelected()
    {
        // 1. Clear 
        ClearAll();

        // 2. Add All, except Selected
        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";

        foreach (string x in lines)     //  Add into text file
        {
            if (x != selectedURL)
            {
                File.AppendAllText(txtDocumentName, x + "\n");
            }
        }

        // 3. Update
        RefreshRead();
        ButtonReloadSelectedImage();
        selectedNumber = 0;

        FilteredUpdate();
        
    }

    public void RefreshRead()
    {
        UnityEngine.Resources.UnloadUnusedAssets();   // Clear previous textures

        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";   // PhotoWidget/CustomUI/

        textFileLines = System.IO.File.ReadAllLines(txtDocumentName);
        lines = new List<string>(textFileLines);
        lines.RemoveAll(s => s == "");   // Remove Empty Elements from list

        if (textTotal != null) { textTotal.text = "Total: " + lines.Count; }
    }


    public void RefreshSelectPanel()
    {
        if (selectedNumber != 0)
        { 
            selectedPanelURL.text = selectedURLClean;
            selectedPanelTag.text = selectedTag;
            selectedPanelName.text = selectedName;
        }
        else
        {
            selectedPanelURL.text = "URL";
            selectedPanelTag.text = "Tag";
            selectedPanelName.text = "Name";
            
            selectedURL = "";
            selectedURLClean = "";
            selectedTag = "";
            selectedName = "";
        }
    }

    public void ShowBigImage(int i)
    {
        previewImageBig.sprite = images[i - 1].sprite; 
        previewImageBig.type = Image.Type.Simple;
        previewImageBig.preserveAspect = true;
    }


    public void SelectedNumber(int selected)
    {
        selectedNumber = selected;

        if (images.Length <= 0)
        {
            previewImageBig.sprite = images[selected - 1].sprite;
            previewImageBig.type = Image.Type.Simple;
            previewImageBig.preserveAspect = true;
        }

        RefreshSelectPanel();
    }

    public void SelectionNumberChangeOnly(int selected)
    {
        selectedNumber = selected;
    }

    public void ButtonReloadSelectedImage()
    {
        if (selectedNumber == 1) 
        { 
            onlineImage1.LoadImage(); SelectedNumber(1);

            selectedURL = onlineImage1.imageURL;
            selectedURLClean = onlineImage1.imageURLClean;
            selectedTag = onlineImage1.imageTag;
            selectedName = onlineImage1.imageName;
        }
        if (selectedNumber == 2) 
        { 
            onlineImage2.LoadImage(); SelectedNumber(2);

            selectedURL = onlineImage2.imageURL;
            selectedURLClean = onlineImage2.imageURLClean;
            selectedTag = onlineImage2.imageTag;
            selectedName = onlineImage2.imageName;
        }
        if (selectedNumber == 3) 
        { 
            onlineImage3.LoadImage(); SelectedNumber(3);

            selectedURL = onlineImage3.imageURL;
            selectedURLClean = onlineImage3.imageURLClean;
            selectedTag = onlineImage3.imageTag;
            selectedName = onlineImage3.imageName;
        }
        if (selectedNumber == 4) 
        { 
            onlineImage4.LoadImage(); SelectedNumber(4);

            selectedURL = onlineImage4.imageURL;
            selectedURLClean = onlineImage4.imageURLClean;
            selectedTag = onlineImage4.imageTag;
            selectedName = onlineImage4.imageName;
        }
        if (selectedNumber == 5) 
        { 
            onlineImage5.LoadImage(); SelectedNumber(5);

            selectedURL = onlineImage5.imageURL;
            selectedURLClean = onlineImage5.imageURLClean;
            selectedTag = onlineImage5.imageTag;
            selectedName = onlineImage5.imageName;
        }

        selectedPanelURL.text = onlineImage.imageURLClean;
        selectedPanelTag.text = onlineImage.imageTag;
        selectedPanelName.text = onlineImage.imageName;
    }

    #region Copying

    public void ButtonCopySelectedToClipboard()
    {
        if (selectedName != "")
        {
            copyString = selectedName + " - " + selectedURLClean;
        }
        else
        {
            copyString = "" + selectedURLClean;
        }

        GetSomeString().CopyToClipboard();
    }

    string y1; string y2; string y3; string y4; string y5;
    public void ButtonCopyToClipboard()
    {
        string x1 = onlineImage1.imageURLClean;
        string x2 = onlineImage2.imageURLClean;
        string x3 = onlineImage3.imageURLClean;
        string x4 = onlineImage4.imageURLClean;
        string x5 = onlineImage5.imageURLClean;

        if (copyNames)
        {
            if (onlineImage1.imageName != "") { y1 = "" + onlineImage1.imageName + " - " + x1; } else { y1 = onlineImage1.imageURLClean; }
            if (onlineImage2.imageName != "") { y2 = "" + onlineImage2.imageName + " - " + x2; } else { y2 = onlineImage2.imageURLClean; }
            if (onlineImage3.imageName != "") { y3 = "" + onlineImage3.imageName + " - " + x3; } else { y3 = onlineImage3.imageURLClean; }
            if (onlineImage4.imageName != "") { y4 = "" + onlineImage4.imageName + " - " + x4; } else { y4 = onlineImage4.imageURLClean; }
            if (onlineImage5.imageName != "") { y5 = "" + onlineImage5.imageName + " - " + x5; } else { y5 = onlineImage5.imageURLClean; }

            if (onlineImage2.gameObject.active)
            {
                copyString = "" + y1 + "\n" + y2 + "\n" + y3 + "\n" + y4 + "\n" + y5;
            }
            else
            {
                copyString = "" + y1;
            }
        }
        else
        {
            if (onlineImage2.gameObject.active)
            {
                copyString = "" + x1 + "\n" + x2 + "\n" + x3 + "\n" + x4 + "\n" + x5;
            }
            else
            {
                copyString = "" + x1;
            }
        }

        GetSomeString().CopyToClipboard();

        copyingDoneText.enabled = true;
        Invoke("RemoveCopyText", 0.5f);
    }

    public string GetSomeString()
    {
        return copyString;
    }
    private void RemoveCopyText()
    {
        copyingDoneText.enabled = false;
    }

    #endregion

    public void OpenFolder()
    {
        string folderPath = Application.dataPath + "/../" + "Collection " + collectionNumber + "/";
        Application.OpenURL(folderPath);
    }

    public void BackUp()
    {
        string dateAndTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");   // 2022-12-21_16-57-56
        print(dateAndTime);

        RefreshRead();

        #region folder

        if (!System.IO.Directory.Exists(Application.dataPath + "/../" + "Collection " + collectionNumber + "/Backups/"))     // Folder path
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/../Collection " + collectionNumber + "/Backups/");  //  Create Folder
        }
        #endregion

        #region create file

        string backupFileName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Backups/Collection - "+ lines.Count + " - " + dateAndTime + ".txt";    // Text file path

        if (!File.Exists(backupFileName))   //  Check if text file exist
        {
            File.WriteAllText(backupFileName, "");  // Write empty text file
        }
        #endregion

        foreach (string x in lines)     //  Add List into text file
        {
            File.AppendAllText(backupFileName, x + "\n");
        }

        string folderPath = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Backups";
        Application.OpenURL(folderPath);    //  Open Folder
    }

    
    

    public void ClearAll()
    {
        //  Clear Text File
        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";   // Text file path

        if (File.Exists(txtDocumentName))  //  Check if text file exist
        {
            File.WriteAllText(txtDocumentName, "");     // Write text file
        }
    }

    public void ClearAllFiles()
    {
        //  Clear Collection Text File
        string txtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Collection.txt";

        if (File.Exists(txtDocumentName))
        {
            File.WriteAllText(txtDocumentName, "");
        }

        //  Clear Tags Text File
        string tagsTxtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Tags.txt"; 

        if (File.Exists(tagsTxtDocumentName))
        {
            File.WriteAllText(tagsTxtDocumentName, "");  
        }

        //  Clear Names Text File
        string namesTxtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Names.txt";

        if (File.Exists(namesTxtDocumentName))
        {
            File.WriteAllText(namesTxtDocumentName, "");
        }

        //  Clear Bookmarks Text File
        string bookmarksTxtDocumentName = Application.dataPath + "/../" + "Collection " + collectionNumber + "/Bookmarks.txt";

        if (File.Exists(bookmarksTxtDocumentName))
        {
            File.WriteAllText(bookmarksTxtDocumentName, "");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}