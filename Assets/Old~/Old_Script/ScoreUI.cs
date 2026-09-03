using TMPro;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;

public class ScoreUI : MonoBehaviour
{
    private const string COMBO_STR = "Combo : ";
    private const string MAX_COMBO_STR = "Max Combo : ";
    private const string SCORE_STR = "Score : ";
    [SerializeField]
    private TextMeshProUGUI txt_Score;


    [Header("Combo")]
    [SerializeField]
    private TextMeshProUGUI txt_Combo;
    [SerializeField]
    private TextMeshProUGUI txt_MaxCombo;

    private IScoreHandler scoreHandler;
    


    private void Awake()
    {

    }

    public void Initialize(IScoreHandler handler)
    {
        scoreHandler = handler;
    }
    private void Update()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        //UI 갱신
        txt_Score.text = SCORE_STR + scoreHandler.GetScore();
        txt_Combo.text = COMBO_STR + scoreHandler.GetCombo();
        txt_MaxCombo.text = MAX_COMBO_STR + scoreHandler.GetMaxCombo();
    }



   

}
