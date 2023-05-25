using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OnlineImage : MonoBehaviour
{
    public TextFileSaving textFileSaving;
    public bool addPanel;

    [Header("Image Data")]
    public string imageURL;
    public string imageURLClean;    //  Without TAG & NAME
    public string imageTag;
    public string imageName;
    public int imageNumber;

    [Header("Objects - Variables")]
    public Image targetImage;
    public Text errorText;

    public RectTransform rt;

    public bool loadOnActivate;

    public bool randomImage = true;

    public Image videoURLIcon;  //   if 'video' tag => show video icon

    private bool hover;


    void Update()
    {
        if (addPanel)
            return;

        if (Input.GetMouseButtonDown(1) && hover)
        {
            UpdateSelectionPanel();
        }
        if (Input.GetMouseButtonDown(2) && hover)
        {
            OpenInBrowser();
        }
        if (Input.GetKeyDown(KeyCode.Delete) && hover)
        {
            string previousSelected = textFileSaving.selectedURL;
            if (previousSelected == imageURL)
            {
                UpdateSelectionPanel();
                textFileSaving.DeleteSelected();

                //print ("Deleting: " + imageURL);

                //Reset Selection
                textFileSaving.SelectedNumber(0);
                textFileSaving.RefreshSelectPanel();
            }
        }
    }

    public void UpdateSelectionPanel()
    {
        textFileSaving.onlineImage = this;
        textFileSaving.selectedURL = imageURL;
        textFileSaving.selectedURLClean = imageURLClean;
        textFileSaving.selectedTag = imageTag;
        textFileSaving.selectedName = imageName;

        textFileSaving.selectedNumber = imageNumber;

        textFileSaving.RefreshSelectPanel();

        if (videoURLIcon)
        {
            if (imageTag == "Video") {  videoURLIcon.enabled = true;    }
            else                     {  videoURLIcon.enabled = false;   }
        }
    }


    int random123;
    IEnumerator GetTexture()
    {
        if (addPanel)
        {
            random123 = Random.Range(0, textFileSaving.lines.Count);
        }
        else
        {
            if (textFileSaving.filterByTag && !textFileSaving.filterByName) // Tag Filter
            {
                if (textFileSaving.filteredLines.Count > 0)
                {
                    random123 = Random.Range(0, textFileSaving.filteredLines.Count);
                    imageURL = textFileSaving.filteredLines[random123];
                }
            }
            else if (textFileSaving.filterByName && !textFileSaving.filterByTag)    //  Name Filter
            {
                if (textFileSaving.filteredLinesNames.Count > 0)
                {
                    random123 = Random.Range(0, textFileSaving.filteredLinesNames.Count);
                    imageURL = textFileSaving.filteredLinesNames[random123];
                }
            }
            else if (textFileSaving.filterByTag && textFileSaving.filterByName) //  Both filters
            {
                if (textFileSaving.filteredLinesBoth.Count > 0)
                {
                    random123 = Random.Range(0, textFileSaving.filteredLinesBoth.Count);
                    imageURL = textFileSaving.filteredLinesBoth[random123];
                }
            }
            else if (!textFileSaving.filterByTag && !textFileSaving.filterByName)   //  None filters
            {
                if (textFileSaving.lines.Count > 0)
                {
                    random123 = Random.Range(0, textFileSaving.lines.Count);
                    imageURL = textFileSaving.lines[random123];
                }
            }
        }

        if (randomImage && textFileSaving.lines.Count != 0)
        {
            //print("Updated Total: " + textFileSaving.lines.Count);

        #region Remove [TAG:] and then {NAME:}

            //int random123 = (Random.Range(0, textFileSaving.lines.Count));
            

            //Modified URL (without tag)
            string newImageURL = imageURL;
            int firstBracket = newImageURL.IndexOf('[');
            int lastBracket = newImageURL.LastIndexOf(']');
            int diff = lastBracket - firstBracket + 1;

                //print("firstbracket = " + firstBracket);
                //print("lastbracket = " + lastBracket);
                //print("diff = " + diff);

            if (firstBracket > 5) // if [TAG:] is added
            {

            #region Get Tag and Name

                // TAG
                
                string getImageTag = newImageURL;

                //Delete part before [
                getImageTag = getImageTag.Substring(firstBracket + 5);    

                //Delete part after ]
                int lastChar = getImageTag.IndexOf("]");
                if (lastChar >= 0)
                { getImageTag = getImageTag.Substring(0, lastChar); }

                    //print(getImageTag);
                imageTag = getImageTag;

                // NAME

                string getImageName = newImageURL;

                //Delete part before {
                int firstChar = newImageURL.IndexOf("{");
                getImageName = getImageName.Substring(firstChar + 6);

                //Delete part after }
                int lastChar2 = getImageName.IndexOf("}");
                if (lastChar2 >= 0)
                { getImageName = getImageName.Substring(0, lastChar2); }

                    //print(getImageName);
                imageName = getImageName;

                #endregion

                newImageURL = newImageURL.Remove(firstBracket, diff);
            }
               
            // print("without Tag test: " + newImageURL);

            //Modified URL (without name)
            string newImageURL2 = newImageURL;
            int firstBracket2 = newImageURL2.IndexOf('{');
            int lastBracket2 = newImageURL2.LastIndexOf('}');
            int diff2 = lastBracket2 - firstBracket2 + 1;

                //print("firstbracket2 = " + firstBracket2);
                //print("lastbracket2 = " + lastBracket2);
                //print("diff2 = " + diff2);

            if (firstBracket2 > 5) // if {NAME:} is added
            { 
                newImageURL2 = newImageURL2.Remove(firstBracket2, diff2);
            }

            //print("without tag & name test: " + newImageURL2);

            #endregion

            imageURLClean = newImageURL2;

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(newImageURL2);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                errorText.text = "" + www.error;

                targetImage.color = Color.red;
                print("error: " + imageURL);
            }
            else
            {
                Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(.5f, .5f));

                targetImage.color = Color.white;
                targetImage.sprite = newSprite;
                targetImage.type = Image.Type.Simple;
                targetImage.preserveAspect = true;
                errorText.text = "";
            }
        }
        else
        {
            if(imageURL != "")
            { 
                UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    errorText.text = "" + www.error;
                    targetImage.color = Color.red;
                }
                else
                {
                    Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

                    Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(.5f, .5f));

                    targetImage.color = Color.white;
                    targetImage.sprite = newSprite;
                    targetImage.type = Image.Type.Simple;
                    targetImage.preserveAspect = true;
                    errorText.text = "";
                }
            }
        }

        if (videoURLIcon)
        {
            if (imageTag == "Video") {  videoURLIcon.enabled = true;    }
            else                     {  videoURLIcon.enabled = false;   }
        }
    }


    
    //  EDITED
    string tempURL;
    public void LoadImageURL(string s)
    {
        tempURL = s;
        imageURL = tempURL + " " + "[TAG:" + imageTag + "]" + " {NAME:" + imageName + "}";

        textFileSaving.selectedURL = imageURL;
        textFileSaving.selectedTag = imageTag;
        textFileSaving.selectedName = imageName;
        textFileSaving.RefreshSelectPanel();

        StartCoroutine(LoadEditedURL());

        textFileSaving.selectedURL = imageURL;
        textFileSaving.RefreshSelectPanel();

        tempURL = s;
        imageURL = tempURL + " " + "[TAG:" + imageTag + "]" + " {NAME:" + imageName + "}";
        textFileSaving.selectedTag = imageTag;
        textFileSaving.selectedName = imageName;
        textFileSaving.RefreshSelectPanel();
    }
    IEnumerator LoadEditedURL()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(tempURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            errorText.text = "" + www.error;

            targetImage.color = Color.red;
            print("error: " + imageURL);
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;

            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(.5f, .5f));

            targetImage.color = Color.white;
            targetImage.sprite = newSprite;
            targetImage.type = Image.Type.Simple;
            targetImage.preserveAspect = true;
            errorText.text = "";

            UpdateSelectionPanel();
        }
    }


    public void LoadImage()
    {
        StartCoroutine(GetTexture());
    }

    public void OpenInBrowser ()
    {
        //Modified URL (without tag)
        string newImageURL = imageURL;
        int firstBracket = newImageURL.IndexOf('[');
        int lastBracket = newImageURL.LastIndexOf(']');
        int diff = lastBracket - firstBracket + 1;

        if (firstBracket > 5) // if [TAG:] is added
        {
            newImageURL = newImageURL.Remove(firstBracket, diff);
        }

        //Modified URL (without name)
        string newImageURL2 = newImageURL;
        int firstBracket2 = newImageURL2.IndexOf('{');
        int lastBracket2 = newImageURL2.LastIndexOf('}');
        int diff2 = lastBracket2 - firstBracket2 + 1;

        if (firstBracket2 > 5) // if {NAME:} is added
        {
            newImageURL2 = newImageURL2.Remove(firstBracket2, diff2);
        }

        Application.OpenURL(newImageURL2);
    }

    void OnEnable()
    {
        if (addPanel)
            return;

        textFileSaving.RefreshRead();
        targetImage.sprite = null;
        textFileSaving.selectedNumber = 0;
        StartCoroutine(GetTexture());
    }

    public void Hover(bool i)
    {
        hover = i;
    }

    public void Zoom(bool i)
    {
        if (i)
        {
            rt.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }
        else
        {
            rt.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}