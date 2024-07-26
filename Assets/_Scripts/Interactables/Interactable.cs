using UnityEngine;

/// <summary>
/// Interactable
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    [Header("Interact Text Settings")]
    public string useText;

    protected PlayerInteractor player;

    public virtual void OnInteract(PlayerInteractor interactingPlayer)
    {
        player = interactingPlayer;
        Debug.Log(interactingPlayer + " interacted with " + name);
    }

    public virtual void OnFocus()
    {

    }

    public virtual void OnLoseFocus()
    {
        player = null;
    }

    public virtual void Start()
    {
        gameObject.layer = 11;
    }
}
