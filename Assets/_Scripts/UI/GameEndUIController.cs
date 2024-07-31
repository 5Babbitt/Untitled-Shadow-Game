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
        menuButton.onClick.AddListener(() => SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.Main_Menu, true));

        gameOver.gameObject.SetActive(GameManager.Instance.IsGameOver);
        escaped.gameObject.SetActive(!GameManager.Instance.IsGameOver);
    }

    private void OnDisable()
    {
        menuButton.onClick.RemoveListener(() => SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.Main_Menu, true));
    }
}
