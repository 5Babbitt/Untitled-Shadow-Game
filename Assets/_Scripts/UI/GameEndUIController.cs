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

    private void Awake()
    {
        menuButton.onClick.AddListener(() => SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.Main_Menu, true));
    }

    private void OnEnable()
    {
        gameOver.gameObject.SetActive(GameManager.Instance.IsGameOver);
        escaped.gameObject.SetActive(!GameManager.Instance.IsGameOver);
    }
}
