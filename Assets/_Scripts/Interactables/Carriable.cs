using UnityEngine;

/// <summary>
/// Carriable
/// </summary>
public class Carriable : Interactable
{
    public bool isCarried;
    public Collider interactableCollider;

    public override void OnInteract(PlayerInteractor interactingPlayer)
    {
        base.OnInteract(interactingPlayer);
        if (isCarried)
        {
            interactableCollider.enabled = false;
            transform.SetParent(null);
        }
        else
        {
            interactableCollider.enabled = true;
            transform.position = player.carrySlot.transform.position;
            transform.SetParent(player.carrySlot.transform);
        }
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
