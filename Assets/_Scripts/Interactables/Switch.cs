using FiveBabbittGames;
using UnityEngine;

public class Switch : Interactable
{
    [Header("Switch Settings")]
    public GameEvent switchEvent;

    [SerializeField] protected bool singleUse = false;

    public bool IsActive => isActive;
    protected bool isActive;

    public override void OnInteract(PlayerInteractor interactingPlayer)
    {
        if (!canInteract)
            return;

        base.OnInteract(interactingPlayer);

        isActive = !isActive;
        switchEvent.Raise(this, isActive);

        if (singleUse)
            canInteract = false;
    }

    public override void OnFocus()
    {
        base.OnFocus();
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
    }
}
