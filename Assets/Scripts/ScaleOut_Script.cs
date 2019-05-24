using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScaleOut_Script : MonoBehaviour
{
    public float StartDelay;
    public float ShrinkTime;
    private TextMeshProUGUI _addedWordText;
    
    void Start()
    {
        _addedWordText = GetComponent<TextMeshProUGUI>();
        //Invoke("Shrink", StartDelay);
        transform.localScale = new Vector3(0,0,0);
    }

    public void SetText(string s)
    {
        _addedWordText.text = s;
        transform.DOComplete();
        DisplayAddedWord();
    }

    void Shrink()
    {
        transform.DOScale(0, ShrinkTime);
    }

    void DisplayAddedWord()
    {
        transform.localScale = new Vector3(1,1,1);
        Invoke("Shrink", StartDelay);
    }
}
