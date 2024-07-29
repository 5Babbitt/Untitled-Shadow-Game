using UnityEngine;

/// <summary>
/// MenuScreenHandler
/// </summary>
public class MenuScreenHandler : MonoBehaviour
{
    public Canvas mainMenu;
    public Canvas creditsScreen;

    private void Start()
    {
        OpenMenuScreen();
    }

    public void OpenMenuScreen()
    {
        creditsScreen.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void OpenCreditsScreen()
    {
        creditsScreen.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }
}
