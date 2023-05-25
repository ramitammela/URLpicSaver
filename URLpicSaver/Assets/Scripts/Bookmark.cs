using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bookmark : MonoBehaviour
{
    public TextFileSaving textFileSaving;
    public string bookmark;

    private Text txt;
    private bool hover;


    public void Start()
    {
        transform.GetComponentInChildren<Text>().text = bookmark;
        textFileSaving = FindObjectOfType<TextFileSaving>();

        txt = transform.GetComponentInChildren<Text>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1) && hover)
        {
            textFileSaving.DeleteSelectedBookmark(bookmark);
            Destroy(this.gameObject);
        }

        if (txt)
        {
            if(hover)
            {   txt.color = Color.cyan;     }
            else
            {   txt.color = Color.white;    }
        }  
    }

    public void ButtonBookmark()
    {
        textFileSaving.selectedBookmark = bookmark;
        Application.OpenURL(bookmark);
    }

    public void Hover(bool i)
    {
        hover = i;
    }
}