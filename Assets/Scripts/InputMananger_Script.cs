using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMananger_Script : MonoBehaviour
{

    public TMP_InputField LetterInputField;
    public TMP_InputField WordInputField;
    private GetWord_Script _getWordScript;
    public TextMeshProUGUI WrongLetterText;
    private GallowMananger_Script _gallowManangerScript;
    private GameMananger_Script _gameManangerScript;

    void Start()
    {
        _getWordScript = GetComponent<GetWord_Script>();
        _gallowManangerScript = GetComponent<GallowMananger_Script>();
        _gameManangerScript = GetComponent<GameMananger_Script>();
        WrongLetterText.text = "";
    }

    public void GuessLetter()
    {
        if (Input.GetButtonDown("Submit"))
        {
            string s = LetterInputField.text;
            bool b = _getWordScript.CheckLetter(s);
            
            if (b)
            {
                Debug.Log("Correct");
                _getWordScript.CheckLetterWord();
            }
            else
            {
                Debug.Log("Wrongs");
                WrongLetterText.text += s + "  ";
                _gallowManangerScript.UpdateGraphics();
            }
            LetterInputField.text = "";
        }
    }

    public void GuessWord()
    {
        if (Input.GetButtonDown("Submit"))
        {
            string s = WordInputField.text;
            _getWordScript.CheckWord(s);
        }
    }

    public void ClearInputFields()
    {
        EventSystem.current.SetSelectedGameObject(LetterInputField.gameObject);
        LetterInputField.text = "";
        WordInputField.text = "";
    }

    public void ToggelInputField(bool b)
    {
        LetterInputField.interactable = b;
        WordInputField.interactable = b;
    }
}
