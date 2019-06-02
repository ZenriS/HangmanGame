using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WordImport_Script : MonoBehaviour
{
    public string SubFolder;
    public string FileName;
    public List<string> Words;
    private bool _importDone;
    private GetWord_Script _getWordScript;
    private List<string> _activeWords;
    private List<string> _manualWords;
    public GameObject CatButtonPrefab;
    private Transform _catButtonHolder;
    private GameMananger_Script _gameManangerScript;
    private GameObject _loadingScreen;
    private List<Button> _catButtons;


    void Start()
    {
        _getWordScript = GetComponent<GetWord_Script>();
        _gameManangerScript = GetComponent<GameMananger_Script>();
        _catButtonHolder = _gameManangerScript.CategoryUIGO.transform.GetChild(2);
        _loadingScreen = _catButtonHolder.parent.parent.GetChild(3).gameObject;
        _loadingScreen.SetActive(true);
        StartCoroutine("loadStreamingAsset");
        _manualWords = new List<string>();
        _activeWords = new List<string>();
        _catButtons = new List<Button>();
    }

    IEnumerator loadStreamingAsset() //import csv file
    {
        //yield return new WaitForSeconds(2f); //for loading screen testing
        //just creates the file path.
        string filePath = Application.streamingAssetsPath;
        /*string filePath =
             System.IO.Path.Combine(Application.streamingAssetsPath, "Resources");*/
        if (SubFolder != "")
        {
            filePath = System.IO.Path.Combine(filePath, SubFolder);
        }
        filePath = System.IO.Path.Combine(filePath, FileName);

        string result; //string to hold the result of the downlaoded file.
        if (filePath.Contains("://") || filePath.Contains(":///")) //check if the file in on a webpage
        {
            yield return new WaitForSeconds(1f); //Safty wait

            UnityWebRequest www = UnityWebRequest.Get(filePath); //Creates the link to the file
            yield return www.SendWebRequest(); //downloads the file
            if (www.isNetworkError || www.isHttpError) //chekc for erros
            {
                Debug.Log(www.error);
            }
            else
            {
                result = www.downloadHandler.text; //creates a string out of all the text in the file
                ReadCSVFile(result); //sendts that string to the ReadCSVFile functions
            }
        }
        else
        {
            yield return new WaitForEndOfFrame(); //seems to be enought to ensure every is linked up and working
            result = System.IO.File.ReadAllText(filePath); //reads the local csv file.
            ReadCSVFile(result); //send the string to the ReadCSVFile functions
        }
    }

    void ReadCSVFile(string s) //reads imported csv file
    {
        StringReader sr = new StringReader(s);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            Words.Add(line);
        }
        _importDone = true;
        GetCategories();
    }

    void GetCategories() //creats category buttons
    {
        string[] cats = Words[1].Split(';');
        Words.RemoveAt(1);
        Words.RemoveAt(0);
        foreach (string s in cats)
        {
            Debug.Log(s);
            createButtons(s);
        }
        createButtons("All");
        _loadingScreen.SetActive(false);
        //CatButtonHolder.GetChild(0).SetAsLastSibling();
    }

    void createButtons(string s) //creates all category buttons
    {
        GameObject go = Instantiate(CatButtonPrefab, _catButtonHolder);
        Button bu = go.GetComponent<Button>();
        _catButtons.Add(bu);
        TextMeshProUGUI t = bu.GetComponentInChildren<TextMeshProUGUI>();
        bu.onClick.AddListener(() => GetWords(s, bu));
        t.text = s;
        go.transform.SetAsLastSibling();
    }

    public void GetWords(string c, Button bu) //get words from csv file based on category
    {
        //activeWords = new List<string>();
        foreach (string s in Words)
        {
            if (c == "All") //get all the words
            {
                Debug.Log(s);
                string[] t = s.Split(';');
                Debug.Log(t[0] + "/" + t[1]);
                _activeWords.Add(t[1]);
                ToggleCategoryButtons(false);
                _gameManangerScript._wordAdded.SetText("All Categories Added");
            }
            else if (s.Contains(c))
            {
                Debug.Log(s);
                string[] t = s.Split(';');
                Debug.Log(t[0] +"/" +t[1]);
                _activeWords.Add(t[1]);
                _gameManangerScript._wordAdded.SetText("Category Added");
            }
        }
        _gameManangerScript.UpdateWordCountText((_activeWords.Count + _manualWords.Count));

        bu.interactable = false;

        //_getWordScript.WordList = new List<string>(activeWords);
        //_getWordScript.ChooseWord();
        //CatButtonHolder.gameObject.SetActive(false);
        //_gameManangerScript.MainGameUIGO.SetActive(true);
    }

    public void AddManualWord(string s) //add manual word
    {
        _manualWords.Add(s);
        _gameManangerScript.UpdateWordCountText((_activeWords.Count + _manualWords.Count));
    }

    public void SetWords() //sets the active words
    {
        if (_activeWords != null && _manualWords != null)
        {
            _getWordScript.WordList = _activeWords.Concat(_manualWords).ToList();
            return;
        }
        if (_activeWords != null)
        {
            _getWordScript.WordList = _activeWords;
            return;
        }
        if (_manualWords != null)
        {
            _getWordScript.WordList = _manualWords;
            return;
        }
    }

    public void ResetWordList()
    {
        _activeWords.Clear();
        _manualWords.Clear();
        ToggleCategoryButtons(true);
        _gameManangerScript._wordAdded.SetText("Word List Cleard");
        _gameManangerScript.UpdateWordCountText(0);
    }

    void ToggleCategoryButtons(bool b)
    {
        foreach (Button bu in _catButtons)
        {
            bu.interactable = b;
        }
    }
}
