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

    public async void OnPlayButtonPressed()
    {
        await SceneLoader.Instance.LoadSceneGroup(1, true);
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
