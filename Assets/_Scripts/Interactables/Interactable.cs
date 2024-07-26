using TMPro;
using UnityEngine;

/// <summary>
/// Interactable
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [Header("Interact Text Settings")]
    public TMP_Text textDisplay;
    public string useText;

    protected PlayerInteractor player;

    public virtual void OnInteract(PlayerInteractor interactingPlayer)
    {
        player = interactingPlayer;
        Debug.Log(interactingPlayer + " interacted with" + name);
    }

    public virtual void OnFocus()
    {
        textDisplay.text = useText;
    }

    public virtual void OnLoseFocus()
    {
        textDisplay.text = null;
        player = null;
    }

    public virtual void Start()
    {
        gameObject.layer = 11;

        textDisplay = GetComponentInChildren<TMP_Text>();
        textDisplay.text = null;
    }
}
