using TMPro;
using UnityEngine;
using System.Collections;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private GameObject GameOverPanel;
    [SerializeField]
    private GameObject GameOverText;
    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private GameObject bt1;
    [SerializeField]
    private GameObject bt2;
    [SerializeField]
    private GameObject bt3;

    [ContextMenu("게임 오버 UI 띄우기")]
    public void StartGameOverUI(int score)
    {
        StartCoroutine(ShowGameOver(score));
    }

    private IEnumerator ShowGameOver(int score)
    {
        GameOverPanel.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        GameOverText.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        ScoreText.gameObject.SetActive(true);
        ScoreText.text = "Score: " + score;
        yield return new WaitForSeconds(0.3f);
        bt1.SetActive(true);
        bt2.SetActive(true);
        bt3.SetActive(true);
    }
}

