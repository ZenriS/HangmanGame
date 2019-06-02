using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMananger_Script : MonoBehaviour
{
    public GameObject GameOverGO;
    public GameObject MainGameUIGO;
    public GameObject CategoryUIGO;
    private TextMeshProUGUI _gameOverTitleText;
    private TextMeshProUGUI _gameOverText;
    private TextMeshProUGUI _scoreText;
    public bool Hard;
    public Toggle HardToggle;
    public TMP_InputField ManualWordInput;
    private GetWord_Script _getWordScript;
    [HideInInspector] public ScaleOut_Script _wordAdded;
    private WordImport_Script _wordImportScript;
    private TextMeshProUGUI _wordCountText;

    void Start()
    {
        _gameOverTitleText = GameOverGO.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _gameOverText = GameOverGO.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _scoreText = GameOverGO.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _getWordScript = GetComponent<GetWord_Script>();
        _wordAdded = CategoryUIGO.GetComponentInChildren<ScaleOut_Script>();
        _wordImportScript = GetComponent<WordImport_Script>();
        _wordCountText = CategoryUIGO.transform.GetChild(8).GetComponent<TextMeshProUGUI>();
        GameOverGO.SetActive(false);
        MainGameUIGO.SetActive(false);
        CategoryUIGO.SetActive(true);
        ToggleHard();
        UpdateWordCountText(0);
    }

    public void GameIsOver(string t, string mt, int sc)
    {
        MainGameUIGO.SetActive(false);
        GameOverGO.SetActive(true);
        _gameOverTitleText.text = t;
        _gameOverText.text = mt;
        _scoreText.text = "Score: " + sc;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleHard()
    {
        Hard = HardToggle.isOn;
    }

    public void SetManulWord(bool b)
    {
        if(Input.GetButtonDown("Submit") || b)
        {
            string s = ManualWordInput.text;
            if (s == "")
            {
                ManualWordInput.transform.DOShakePosition(1, 5);
                return;
            }
            //_getWordScript.WordList.Add(s);
            _wordImportScript.AddManualWord(s);
            ManualWordInput.text = "";
            _wordAdded.SetText("Word Added");
        }
    }

    public void StartGame()
    {
        _wordImportScript.SetWords();
        if (_getWordScript.WordList.Count == 0)
        {
            _wordAdded.SetText("No Words");
            return;
        }
        _getWordScript.ChooseWord();
        CategoryUIGO.gameObject.SetActive(false);
        MainGameUIGO.SetActive(true);
    }

    public void UpdateWordCountText(int i)
    {
        _wordCountText.text = "Word Count: \n" + i;
    }

    
}
