using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GetWord_Script : MonoBehaviour
{
    public List<string> WordList; //temp change to bd or csv later
    public string ActiveWord;
    public GameObject LetterSlotsHolder;
    private TextMeshProUGUI[] _letterSlotText;
    [HideInInspector] public List<GameObject> _activeSlots;
    private GallowMananger_Script _gallowManangerScript;
    private InputMananger_Script _inputManangerScript;
    private GameMananger_Script _gameManangerScript;
    public float LetterDelay;
    private int letterCount;
    private ScoreMananger_Script _scoreManangerScript;

    void Start()
    {
        _letterSlotText = LetterSlotsHolder.GetComponentsInChildren<TextMeshProUGUI>();
        _inputManangerScript = GetComponent<InputMananger_Script>();
        _gallowManangerScript = GetComponent<GallowMananger_Script>();
        _scoreManangerScript = GetComponent<ScoreMananger_Script>();
        _gameManangerScript = GetComponent<GameMananger_Script>();

        foreach (TextMeshProUGUI t in _letterSlotText)
        {
            t.transform.parent.gameObject.SetActive(false);
        }
        //ChooseWord();
    }

    public void ChooseWord()
    {
        _activeSlots.Clear();
        foreach (TextMeshProUGUI t in _letterSlotText)
        {
            t.transform.parent.gameObject.SetActive(false);
        }
        int r = Random.Range(0, WordList.Count);
        ActiveWord = WordList[r];
        //WordList.Remove(ActiveWord);
        int l = ActiveWord.Length;
        Debug.Log(l);
        for (int i = 0; i < l; i++)
        {
            _letterSlotText[i].transform.parent.gameObject.SetActive(true);
            if (ActiveWord[i].ToString() == " ")
            {
                _letterSlotText[i].text = "";
                _letterSlotText[i].transform.parent.GetComponent<Image>().enabled = false;
                continue;
            }
            _activeSlots.Add(_letterSlotText[i].gameObject);
            _letterSlotText[i].text = ActiveWord[i].ToString();
            _letterSlotText[i].gameObject.SetActive(false);
        }
        letterCount = _activeSlots.Count;
        _gallowManangerScript.SetUpGraphics();
    }

    public bool CheckLetter(string s)
    {
        foreach (TextMeshProUGUI t in _letterSlotText) //to prevent usd letter triggering correct again
        {
            if (t.text == s)
            {
                if (t.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }
        }
        int c = 0; //nr of correct letters
        bool b = false; //create return value
        List<GameObject> gos = new List<GameObject>();
        Debug.Log("CheckLetter");
        foreach (TextMeshProUGUI t in _letterSlotText)
        {
            if (t.transform.parent.gameObject.activeInHierarchy)
            {
                int i = String.Compare(t.text, s, true);
                //Debug.Log("Index: " + i);
                if (i == 0)
                {
                    //t.gameObject.SetActive(true);
                    gos.Add(t.gameObject);
                    c++;
                }
            }
        }
        if (c > 0)
        {
            b = true;
        }
        StartCoroutine(DisplayLetters(gos));
        return b;
    }

    IEnumerator DisplayLetters(List<GameObject> gos)
    {
        _inputManangerScript.ToggelInputField(false);
        foreach (GameObject go in gos)
        {
            go.SetActive(true);
            letterCount--;
            yield return new WaitForSeconds(LetterDelay);
        }
        if (letterCount <= 0)
        {
            _scoreManangerScript.AddScore(letterCount+1);
            yield return new WaitForSeconds(1f);
            CheckWordList(true);
        }
        Debug.Log("Letters Left: " +letterCount);
        _inputManangerScript.ToggelInputField(true);
        _inputManangerScript.ClearInputFields();
    }

    public bool CheckLetterWord()
    {
        bool b = false;
        Debug.Log("CheckLetterWord");
        foreach (GameObject go in _activeSlots)
        {
            if (!go.activeInHierarchy)
            {
                b = false;
                break;
            }
            else
            {
                b = true;
            }
        }
        return b;
    }

    public void CheckWord(string s)
    {
        int i = string.Compare(s, ActiveWord, true);
        Debug.Log("CheckWord");
        if (i == 0)
        {
            Debug.Log("Word Correct");
            //_getWordScript.DisplayWord();
            StartCoroutine("DisplayWord");
        }
        else
        {
            Debug.Log("Word is Wrong");
            _gallowManangerScript.UpdateGraphics();
            _inputManangerScript.ClearInputFields();

        }
    }

    public IEnumerator DisplayWord()
    {
        _inputManangerScript.ToggelInputField(false);
        foreach (GameObject go in _activeSlots)
        {
            go.SetActive(true);
            yield return new WaitForSeconds(LetterDelay);
        }
        _scoreManangerScript.AddScore(letterCount);
        yield return new WaitForSeconds(1f);
        _inputManangerScript.ToggelInputField(true);
    }

    void CheckWordList(bool b)
    {
        if (b)
        {
            Debug.Log("Word Completet");
            WordList.Remove(ActiveWord);
            if (WordList.Count <= 0)
            {
                Debug.Log("No more words");
                _gameManangerScript.GameIsOver("Victory", "You saved everyone, grats.. they where all killers..", _scoreManangerScript.TotalScore);
                return;
            }
            ChooseWord();
        }
        else
        {
            Debug.Log("Word not complete");
        }
        _inputManangerScript.ClearInputFields();
    }
}
