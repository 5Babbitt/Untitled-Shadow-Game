using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// CreditsUI
/// </summary>
public class CreditsController : MonoBehaviour
{
    [SerializeField] Button backButton;

    private void OnEnable()
    {
        backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnDisable()
    {
        backButton.onClick.RemoveListener(OnBackButtonPressed);
    }

    public void OnBackButtonPressed()
    {
        transform.root.GetComponent<MenuScreenHandler>().OpenMenuScreen();
    }
}
