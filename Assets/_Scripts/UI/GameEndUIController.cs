using FiveBabbittGames;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameEndUIController
/// </summary>
public class GameEndUIController : MonoBehaviour
{
    public Button menuButton;

    public Canvas gameOver;
    public Canvas escaped;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        menuButton.onClick.AddListener(LoadMainMenu);

        gameOver.gameObject.SetActive(GameManager.Instance.IsGameOver);
        escaped.gameObject.SetActive(!GameManager.Instance.IsGameOver);
    }

    private void OnDisable()
    {
        menuButton.onClick.RemoveListener(LoadMainMenu);
    }

    void LoadMainMenu()
    {
        //await SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.Main_Menu, true);
        GameManager.Instance.LoadLevel("MainMenu");
    }
}
