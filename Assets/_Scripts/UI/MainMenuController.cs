using FiveBabbittGames;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenuController
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button quitButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonPressed);
        creditsButton.onClick.AddListener(OnCreditsButtonPressed);
        quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayButtonPressed);
        creditsButton.onClick.RemoveListener(OnCreditsButtonPressed);
        quitButton.onClick.RemoveListener(OnQuitButtonPressed);
    }

    public async void OnPlayButtonPressed()
    {
        await SceneLoader.Instance.LoadSceneGroup((int)ESceneGroupIndex.Gameplay, true);
    }

    public void OnCreditsButtonPressed() 
    {
        transform.root.GetComponent<MenuScreenHandler>().OpenCreditsScreen();
    }

    public void OnQuitButtonPressed() 
    { 
        Application.Quit();
    }
}
