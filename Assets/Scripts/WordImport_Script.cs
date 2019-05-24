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
    private bool importDone;
    private GetWord_Script _getWordScript;
    private List<string> activeWords;
    private List<string> manualWords;
    public GameObject CatButtonPrefab;
    private Transform _catButtonHolder;
    private GameMananger_Script _gameManangerScript;
    private GameObject _loadingScreen;


    void Start()
    {
        _getWordScript = GetComponent<GetWord_Script>();
        _gameManangerScript = GetComponent<GameMananger_Script>();
        _catButtonHolder = _gameManangerScript.CategoryUIGO.transform.GetChild(2);
        _loadingScreen = _catButtonHolder.parent.parent.GetChild(3).gameObject;
        _loadingScreen.SetActive(true);
        StartCoroutine("loadStreamingAsset");
        manualWords = new List<string>();
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
        importDone = true;
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
            GameObject go = Instantiate(CatButtonPrefab, _catButtonHolder);
            Button bu = go.GetComponent<Button>();
            TextMeshProUGUI t = bu.GetComponentInChildren<TextMeshProUGUI>();
            bu.onClick.AddListener(() =>GetWords(s));
            t.text = s;
            go.transform.SetAsFirstSibling();
        }
        _loadingScreen.SetActive(false);
        //CatButtonHolder.GetChild(0).SetAsLastSibling();
    }

    public void GetWords(string c) //get words from csv file based on category
    {
        activeWords = new List<string>();
        foreach (string s in Words)
        {
            if (s.Contains(c))
            {
                Debug.Log(s);
                string[] t = s.Split(';');
                Debug.Log(t[0] +"/" +t[1]);
                activeWords.Add(t[1]);
            }
        }
        _gameManangerScript._wordAdded.SetText("Category Set");
        //_getWordScript.WordList = new List<string>(activeWords);
        //_getWordScript.ChooseWord();
        //CatButtonHolder.gameObject.SetActive(false);
        //_gameManangerScript.MainGameUIGO.SetActive(true);
    }

    public void AddManualWord(string s) //add manual word
    {
        manualWords.Add(s);
    }

    public void SetWords() //sets the active words
    {
        if (activeWords != null && manualWords != null)
        {
            _getWordScript.WordList = activeWords.Concat(manualWords).ToList();
            return;
        }
        if (activeWords != null)
        {
            _getWordScript.WordList = activeWords;
            return;
        }
        if (manualWords != null)
        {
            _getWordScript.WordList = manualWords;
            return;
        }

    }
}
