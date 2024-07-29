using UnityEngine;
using FiveBabbittGames;
using TMPro;

/// <summary>
/// HUDHandler
/// </summary>
public class HUDController : Singleton<HUDController>
{
    [Header("HUD Settings")]
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private TMP_Text interactText;

    [Header("Snippet Settings")]
    [SerializeField] private Canvas SnippetCanvas;
    [SerializeField] private TMP_Text snippetText;
    
    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        CloseSnippet();
        SetInteractText();
    }

    /// <summary>
    /// Leave Empty to clear text
    /// </summary>
    public void SetInteractText(string text)
    {
        interactText.text = text;
    }

    /// <summary>
    /// Leave Empty to clear text
    /// </summary>
    public void SetInteractText()
    {
        interactText.text = null;
    }

    public void ViewSnippet(string text)
    {
        SnippetCanvas.gameObject.SetActive(true);
        HUDCanvas.gameObject.SetActive(false);

        snippetText.text = text;
    }

    public void CloseSnippet()
    {
        SnippetCanvas.gameObject.SetActive(false);
        HUDCanvas.gameObject.SetActive(true);

        snippetText.text = null;
    }
}
