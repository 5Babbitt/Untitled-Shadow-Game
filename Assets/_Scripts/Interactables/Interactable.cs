using UnityEngine;

/// <summary>
/// Interactable
/// </summary>
[RequireComponent (typeof(BoxCollider))]
public abstract class Interactable : MonoBehaviour
{
    public bool CanInteract => canInteract;
    protected bool canInteract = true;
    protected PlayerInteractor player;

    [Header("Interact Text Settings")]
    public string useText;

    public virtual void OnInteract(PlayerInteractor interactingPlayer)
    {
        if (!canInteract)
            return;

        player = interactingPlayer;
        Debug.Log(interactingPlayer + " interacted with " + name);
    }

    public virtual void OnFocus()
    {
        HUDController.Instance.SetInteractText(useText);
    }

    public virtual void OnLoseFocus()
    {
        HUDController.Instance.SetInteractText();
        player = null;
    }

    protected virtual void UpdateInteractText()
    {
        HUDController.Instance.SetInteractText(useText);
    }

    public virtual void Start()
    {
        gameObject.layer = 11;
    }
}
