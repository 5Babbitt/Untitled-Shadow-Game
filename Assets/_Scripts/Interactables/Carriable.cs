using UnityEngine;

/// <summary>
/// Carriable
/// </summary>
public class Carriable : Interactable
{
    public bool isCarried;
    public Collider interactableCollider;

    public KeyData KeyData;

    public override void Start()
    {
        base.Start();
        interactableCollider = GetComponent<Collider>();
    }

    public override void OnInteract(PlayerInteractor interactingPlayer)
    {
        if (interactingPlayer.GetCarriable() != null) 
            return;

        base.OnInteract(interactingPlayer);

        if (isCarried)
        {
            Drop(interactingPlayer);
        }
        else
        {
            PickUp(interactingPlayer);
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

    public void PickUp(PlayerInteractor interactingPlayer)
    {
        isCarried = true;
        interactableCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(player.carrySlot.carryTransform);
        transform.position = player.carrySlot.carryTransform.position;
        interactingPlayer.SetCarriable(this);
    }

    public void Drop(PlayerInteractor interactingPlayer)
    {
        isCarried = false;
        interactableCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.SetParent(null);
        interactingPlayer.SetCarriable(null);
    }
}
