using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreMananger_Script : MonoBehaviour
{
    public int BaseValue;
    public int TotalScore;
    public TextMeshProUGUI ScoreText;
    public int Correct;
    public int Failed;

    void Start()
    {
        ScoreText.text = "Score: ";
    }

    public void AddScore(int s) //Score currently not in use, counting correct and wrong instead
    {
        TotalScore += s * BaseValue;
        ScoreText.text = "Score: " + TotalScore;
    }

    public void AddCorrect()
    {
        //TODO: Add effect and sounds
        Correct++;
    }

    public void AddFail()
    {
        //TODO: Add effect and sounds
        Failed++;
    }
}
