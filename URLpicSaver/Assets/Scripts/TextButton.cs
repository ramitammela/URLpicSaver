using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onClick;  // add callbacks in the inspector like for buttons
    public int searchType; // 0 tag  1 name

    private Text UIText;


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        onClick.Invoke();
    }

    public void ButtonClicked()
    {
        UIText = transform.GetComponent<Text>();

        if (UIText.text == "")
            return;

        //print("name: " + this.name);

        TextFileSaving textFileSaving = (TextFileSaving)FindObjectOfType(typeof(TextFileSaving));
        textFileSaving.FilterByClickedText(searchType, UIText.text);
    }
    public void ButtonClickedOpenURL()
    {
        UIText = transform.GetComponent<Text>();

        if (UIText.text == "")
            return;

        Application.OpenURL(UIText.text);
    }
}