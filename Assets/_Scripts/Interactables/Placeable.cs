using UnityEngine;

/// <summary>
/// Placeable
/// </summary>
public class Placeable : Interactable
{
    [SerializeField] private Vector3 dimensions = Vector3.one;

    [SerializeField] private Carriable carrySlot;

    public override void OnInteract(PlayerInteractor interactingPlayer)
    {
        base.OnInteract(interactingPlayer);
    }

    public override void OnFocus()
    {

    }

    public override void OnLoseFocus()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, dimensions);
    }
}
