using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreMananger_Script : MonoBehaviour
{
    public int BaseValue;
    public int TotalScore;
    public TextMeshProUGUI ScoreText;

    void Start()
    {
        ScoreText.text = "Score: ";
    }
    public void AddScore(int s)
    {
        TotalScore += s * BaseValue;
        ScoreText.text = "Score: " + TotalScore;
    }
}
