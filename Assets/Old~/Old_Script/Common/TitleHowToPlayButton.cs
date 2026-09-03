using UnityEngine;
using UnityEngine.UI;

public class TitleHowToPlayButton : MonoBehaviour
{
    private bool isActive = false;

    [SerializeField]
    private Button howToPlayBtn;
    [SerializeField]
    private GameObject howToPlayPanel;
    void Start()
    {
        howToPlayBtn.onClick.AddListener(TogglePanel);
    }

    public void TogglePanel()
    {
        if (isActive)
        {
            howToPlayPanel.SetActive(false);
            isActive = false;
        }
        else
        {
            howToPlayPanel.SetActive(true);
            isActive = true;
        }
    }
}
