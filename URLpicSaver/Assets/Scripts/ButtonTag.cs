using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTag : MonoBehaviour
{
    public TextFileSaving textFileSaving;
    public string tag;

    private bool hover;


    public void Start()
    {
        transform.GetComponentInChildren<Text>().text = tag;
        textFileSaving = FindObjectOfType<TextFileSaving>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1) && hover)
        {
            textFileSaving.DeleteSelectedButtonTag(tag);
            Destroy(this.gameObject);
        }
    }

    public void ButtongTag()
    {
        textFileSaving.inputfieldSearchTag.text = tag;
        textFileSaving.tagToSearch = tag;
        textFileSaving.FilterByTagInputfieldEnd();
        textFileSaving.FilterButtonTest();
        textFileSaving.tagToggle.isOn = true;
        textFileSaving.filterByTag = true;
    }

    public void Hover(bool i)
    {
        hover = i;
    }
}