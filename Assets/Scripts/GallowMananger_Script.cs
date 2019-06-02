using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GallowMananger_Script : MonoBehaviour
{

    public GameObject MainGallowGO;
    private SpriteRenderer[] _gallowSprits;
    private int _gallowCount;
    private SpriteRenderer[] _characterSprites;
    private int _characterCount;
    private GameMananger_Script _gameManangerScript;
    private ScoreMananger_Script _scoreManangerScript;
    
    void Start()
    {
        _gameManangerScript = GetComponent<GameMananger_Script>();
        _gallowSprits = MainGallowGO.transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        _characterSprites = MainGallowGO.transform.GetChild(1).GetComponentsInChildren<SpriteRenderer>();
        _scoreManangerScript = GetComponent<ScoreMananger_Script>();
    }

    public void SetUpGraphics()
    {
        foreach (SpriteRenderer go in _gallowSprits)
        {
            if (!_gameManangerScript.Hard)
            {
                go.gameObject.SetActive(false);
            }
            else
            {
                go.gameObject.SetActive(true);
                _gallowCount = 10;
            }
        }
        _gallowSprits[_gallowSprits.Length - 1].gameObject.SetActive(true);
        foreach (SpriteRenderer go in _characterSprites)
        {
            go.gameObject.SetActive(false);
        }
    }

    public void UpdateGraphics()
    {
        
        if (_gallowCount < _gallowSprits.Length-1 && !_gameManangerScript.Hard)
        {
            _gallowSprits[_gallowCount].gameObject.SetActive(true);
            _gallowCount++;
            return;
        }
        if (_characterCount < _characterSprites.Length)
        {
            _characterSprites[_characterCount].gameObject.SetActive(true);
            _characterCount++;
        }
        if (_gallowCount >= _gallowSprits.Length-1 && _characterCount >= _characterSprites.Length)
        {
            _gameManangerScript.GameIsOver("Game Over","You failed, wanna try another word?");
            Debug.Log("No More Grahics, Game Over");
            _scoreManangerScript.AddFail();
        }
    }
}
