using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTheme : MonoBehaviour
{
    public int selectedTheme;
    public enum ColorShade { Normal, Dark, ButtonText, Text, Text2, TextMessage, Background   };
    public ColorShade colorType;

    private Image image;
    private Text text;
    private Color newCol;

    //  Themes
    //  0   -   Dark
    //  1   -   Pink
    //  2   -   Blue
    //  3   -   Green
    //  4   -   Custom


    void Start()
    {
        image = transform.GetComponent<Image>();
        UpdateColor();
    }

    public void UpdateColor()
    {
        selectedTheme = PlayerPrefs.GetInt("SelectedTheme");

        if (colorType == ColorShade.Normal)
        {
            text = transform.GetComponent<Text>();

            if (selectedTheme == 0) {   image.color = new Color32(63, 63, 63, 255); }
            if (selectedTheme == 1) {   image.color = new Color32(255, 96, 126, 255); }
            if (selectedTheme == 2) {   image.color = new Color32(0, 97, 140, 255); }
            if (selectedTheme == 3) {   image.color = new Color32(90, 130, 74, 255); }
            if (selectedTheme == 4) 
            {
                string tempColorNormal = "#" + PlayerPrefs.GetString("UINormalColor");

                if (ColorUtility.TryParseHtmlString(tempColorNormal, out newCol))
                {
                    if (text)
                    {
                        text.color = newCol;
                    }
                    if (image)
                    {
                        image.color = newCol;
                    } 
                }
            }
        }
        if (colorType == ColorShade.Dark)
        {
            if (selectedTheme == 0) {   image.color = new Color32(32, 32, 32, 255); }
            if (selectedTheme == 1) {   image.color = new Color32(130, 32, 60, 255); }
            if (selectedTheme == 2) {   image.color = new Color32(33, 58, 99, 255); }
            if (selectedTheme == 3) {   image.color = new Color32(34, 100, 5, 255); }
            if (selectedTheme == 4)
            {
                string tempColorDark = "#" + PlayerPrefs.GetString("UIDarkColor");

                if (ColorUtility.TryParseHtmlString(tempColorDark, out newCol))
                {
                    image.color = newCol;
                }
            }
        }
        if (colorType == ColorShade.ButtonText)
        {
            text = transform.GetComponent<Text>();

            if (text)
            { 
                if (selectedTheme == 0) { text.color = Color.white; }
                if (selectedTheme == 1) { text.color = Color.white; }
                if (selectedTheme == 2) { text.color = Color.white; }
                if (selectedTheme == 3) { text.color = Color.white; }
            }
            if (selectedTheme == 4)
            {
                string tempColorText = "#" + PlayerPrefs.GetString("UIButtonTextColor");

                if (ColorUtility.TryParseHtmlString(tempColorText, out newCol))
                {
                    if (text)
                    {
                        text.color = newCol;
                    }
                    if (image)
                    {
                        image.color = newCol;
                    } 
                }
            }
        }
        if (colorType == ColorShade.Text)
        {
            text = transform.GetComponent<Text>();
            if (selectedTheme == 0) {   text.color = Color.white; }
            if (selectedTheme == 1) {   text.color = Color.white; }
            if (selectedTheme == 2) {   text.color = Color.white; }
            if (selectedTheme == 3) {   text.color = Color.white; }
            if (selectedTheme == 4)
            {
                string tempColorText = "#" + PlayerPrefs.GetString("UITextColor");

                if (ColorUtility.TryParseHtmlString(tempColorText, out newCol))
                {
                    text.color = newCol;
                }
            }
        }
        if (colorType == ColorShade.Text2)
        {
            text = transform.GetComponent<Text>();
            if (selectedTheme == 0) {   text.color = Color.white; }
            if (selectedTheme == 1) {   text.color = Color.white; }
            if (selectedTheme == 2) {   text.color = Color.white; }
            if (selectedTheme == 3) {   text.color = Color.white; }
            if (selectedTheme == 4)
            {
                string tempColorText = "#" + PlayerPrefs.GetString("UITextColor2");

                if (ColorUtility.TryParseHtmlString(tempColorText, out newCol))
                {
                    text.color = newCol;
                }
            }
        }
        if (colorType == ColorShade.TextMessage)
        {
            text = transform.GetComponent<Text>();
            if (selectedTheme == 0) { text.color = Color.green; }
            if (selectedTheme == 1) { text.color = Color.green; }
            if (selectedTheme == 2) { text.color = Color.green; }
            if (selectedTheme == 3) { text.color = Color.green; }
            if (selectedTheme == 4)
            {
                string tempColorMessageText = "#" + PlayerPrefs.GetString("UITextMessageColor");

                if (ColorUtility.TryParseHtmlString(tempColorMessageText, out newCol))
                {
                    text.color = newCol;
                }
            }
        }
        if (colorType == ColorShade.Background)
        {
            if (selectedTheme == 0) { image.color = new Color32(15, 15, 20, 255); }
            if (selectedTheme == 1) { image.color = new Color32(33, 0, 15, 255); }
            if (selectedTheme == 2) { image.color = new Color32(15, 15, 50, 255); }
            if (selectedTheme == 3) { image.color = new Color32(15, 35, 15, 255); }
            if (selectedTheme == 4)
            {
                string tempColorBackground = "#" + PlayerPrefs.GetString("UIBackgroundColor");

                if (ColorUtility.TryParseHtmlString(tempColorBackground, out newCol))
                {
                    image.color = newCol;
                }
            }
        }
    }

}