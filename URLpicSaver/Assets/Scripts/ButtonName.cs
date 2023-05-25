using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonName : MonoBehaviour
{
    public TextFileSaving textFileSaving;
    public string name;

    private bool hover;


    public void Start()
    {
        transform.GetComponentInChildren<Text>().text = name;
        textFileSaving = FindObjectOfType<TextFileSaving>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1) && hover)
        {
            textFileSaving.DeleteSelectedButtonName(name);
            Destroy(this.gameObject);
        }
    }

    public void ButtongName()
    {
        textFileSaving.inputfieldSearchName.text = name;
        textFileSaving.nameToSearch = name;
        textFileSaving.FilterByNameInputfieldEnd();
        textFileSaving.FilterButtonTestNames();
        textFileSaving.nameToggle.isOn = true;
        textFileSaving.filterByName = true;
    }

    public void Hover(bool i)
    {
        hover = i;
    }
}