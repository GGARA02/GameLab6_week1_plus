using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleUI : MonoBehaviour
{
    private const string GAME_SCENE_NAME = "GameScene";
    private const string TITLE_SCENE_NAME = "Title";

    public void GameStartButton()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }

    public void TitleButton()
    {
        SceneManager.LoadScene(TITLE_SCENE_NAME);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}

