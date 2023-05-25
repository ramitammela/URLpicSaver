using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerScript : MonoBehaviour
{
    public Image target;    // Preview
    public int selectedUIItem;

    //  Colors
    public Image previewNormal;
    public Image previewDark;
    public Image previewBackground;

    //  Text Colors
    public Image previewButtonText;
    public Image previewText;
    public Image previewText2;
    public Image previewMessageText;



    private Color newCol;


    void Start()
    {
        ChooseColorButtonClick();

        string tempColorNormal = "#" + PlayerPrefs.GetString("UINormalColor");
        string tempColorDark = "#" + PlayerPrefs.GetString("UIDarkColor");
        string tempButtonColorText = "#" + PlayerPrefs.GetString("UIButtonTextColor");
        string tempColorText = "#" + PlayerPrefs.GetString("UITextColor");
        string tempColorText2 = "#" + PlayerPrefs.GetString("UITextColor2");
        string tempColorMessageText = "#" + PlayerPrefs.GetString("UITextMessageColor");
        string tempColorBackground = "#" + PlayerPrefs.GetString("UIBackgroundColor");


        if (ColorUtility.TryParseHtmlString(tempColorNormal, out newCol))
        {
            previewNormal.color = newCol;
        }
        if (ColorUtility.TryParseHtmlString(tempColorDark, out newCol))
        {
            previewDark.color = newCol;
        }
        if (ColorUtility.TryParseHtmlString(tempButtonColorText, out newCol))
        {
            previewButtonText.color = newCol;
        }
        if (ColorUtility.TryParseHtmlString(tempColorText, out newCol))
        {
            previewText.color = newCol;
        }
        if (ColorUtility.TryParseHtmlString(tempColorText2, out newCol))
        {
            previewText2.color = newCol;
        }
        if (ColorUtility.TryParseHtmlString(tempColorMessageText, out newCol))
        {
            previewMessageText.color = newCol;
        }
        if (ColorUtility.TryParseHtmlString(tempColorBackground, out newCol))
        {
            previewBackground.color = newCol;
        }

        this.gameObject.SetActive(false);
    }

    public void ChangeColorFor(int c)
    {
        selectedUIItem = c;

        //1 - Normal/Color1
        //2 - Dark/Color2
        //3 - ButtonText
        //4 - Text
        //5 - Text 2
        //6 - MessageText
        //7 - Background
    }

    public void ChooseColorButtonClick()
    {
        ColorPicker.Create(target.color, "color", SetColor, ColorFinished, true);
    }
    private void SetColor(Color currentColor)
    {
        target.color = currentColor;
    }

    private void ColorFinished(Color finishedColor)
    {
        string htmlColor = ColorUtility.ToHtmlStringRGBA(finishedColor);
        string htmlValue = "#" + ColorUtility.ToHtmlStringRGBA(finishedColor);
        Color colorTest;

        if (ColorUtility.TryParseHtmlString(htmlValue, out colorTest))
        {
            if (selectedUIItem == 1)    //  Normal/Color1
            {
                previewNormal.color = colorTest;

                PlayerPrefs.SetString("UINormalColor", htmlColor);
                PlayerPrefs.Save();
            }
            if (selectedUIItem == 2)    //  Dark/Color2
            {
                previewDark.color = colorTest;

                PlayerPrefs.SetString("UIDarkColor", htmlColor);
                PlayerPrefs.Save();

            }
            if (selectedUIItem == 3)    //  Button Text
            {
                previewButtonText.color = colorTest;

                PlayerPrefs.SetString("UIButtonTextColor", htmlColor);
                PlayerPrefs.Save();
            }
            if (selectedUIItem == 4)    //  Text
            {
                previewText.color = colorTest;

                PlayerPrefs.SetString("UITextColor", htmlColor);
                PlayerPrefs.Save();
            }
            if (selectedUIItem == 5)    //  Text 2
            {
                previewText2.color = colorTest;

                PlayerPrefs.SetString("UITextColor2", htmlColor);
                PlayerPrefs.Save();
            }
            if (selectedUIItem == 6)    //  Message Text
            {
                previewMessageText.color = colorTest;

                PlayerPrefs.SetString("UITextMessageColor", htmlColor);
                PlayerPrefs.Save();
            }

            if (selectedUIItem == 7)    //  Background
            {
                previewBackground.color = colorTest;

                PlayerPrefs.SetString("UIBackgroundColor", htmlColor);
                PlayerPrefs.Save();
            }
        }
    }

    #region Copying

    public string copyString;
    public void CopySelectedColorCode(int selectedColor)
    {
        if (selectedColor == 1) //  Color 1
        { copyString = PlayerPrefs.GetString("UINormalColor"); }
        if (selectedColor == 2) //  Color 2
        { copyString = PlayerPrefs.GetString("UIDarkColor"); }
        if (selectedColor == 3) //  Button Text
        { copyString = PlayerPrefs.GetString("UIButtonTextColor"); }
        if (selectedColor == 4) //  Text
        { copyString = PlayerPrefs.GetString("UITextColor"); }
        if (selectedColor == 5) //  Text 2
        { copyString = PlayerPrefs.GetString("UITextColor2"); }
        if (selectedColor == 6) //  Message Text
        { copyString = PlayerPrefs.GetString("UITextMessageColor"); }
        if (selectedColor == 7) //  Background
        { copyString = PlayerPrefs.GetString("UIBackgroundColor"); }

        GetSomeString().CopyToClipboard();
    }
    public string GetSomeString()
    {
        return copyString;
    }

    #endregion

    public void CopyingButtonScaleUp (int i)
    {
        float newSize = 1.03f;

        if (i == 1) // previewNormal
        {
            previewNormal.rectTransform.localScale = new Vector3(newSize, newSize, newSize);
        }
        if (i == 2) // previewDark
        {
            previewDark.rectTransform.localScale = new Vector3(newSize, newSize, newSize);
        }
        if (i == 3) // ButtonText
        {
            previewButtonText.rectTransform.localScale = new Vector3(newSize, newSize, newSize);
        }
        if (i == 4) // previewText
        {
            previewText.rectTransform.localScale = new Vector3(newSize, newSize, newSize);
        }
        if (i == 5) // previewText2
        {
            previewText2.rectTransform.localScale = new Vector3(newSize, newSize, newSize);
        }
        if (i == 6) // previewMessageText
        {
            previewMessageText.rectTransform.localScale = new Vector3(newSize, newSize, newSize);
        }
        if (i == 7) // background
        {
            previewBackground.rectTransform.localScale = new Vector3(newSize, newSize, newSize);
        }
    }
    public void CopyingButtonScaleDown(int i)
    {
        if (i == 1) // previewDark
        {
            previewNormal.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (i == 2) // previewDark
        {
            previewDark.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (i == 3) // ButtonText
        {
            previewButtonText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (i == 4) // previewText
        {
            previewText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (i == 5) // previewText2
        {
            previewText2.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (i == 6) // previewMessageText
        {
            previewMessageText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (i == 7) // background
        {
            previewBackground.rectTransform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}