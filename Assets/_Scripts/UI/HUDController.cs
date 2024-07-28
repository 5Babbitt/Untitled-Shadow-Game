using UnityEngine;
using FiveBabbittGames;
using TMPro;

/// <summary>
/// HUDHandler
/// </summary>
public class HUDController : Singleton<HUDController>
{
    [SerializeField] private TMP_Text interactText;
    
    protected override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        
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
}
