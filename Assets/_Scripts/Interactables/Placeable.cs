using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// Placeable
/// </summary>
public class Placeable : Interactable
{
    [SerializeField] private GameEvent onKeyItemPlaced;
    [SerializeField] private Carriable carrySlot;
    [SerializeField] private Vector3 dimensions = Vector3.one;

    public KeyData KeyData;

    public override void OnInteract(PlayerInteractor interactingPlayer)
    {
        base.OnInteract(interactingPlayer);

        var playerCarry = interactingPlayer.GetCarriable();

        if (carrySlot != null || playerCarry == null) 
            return;

        if (!KeyDataMatch(playerCarry))
            return;

        carrySlot = interactingPlayer.GetCarriable();
        carrySlot.transform.SetParent(transform);
        carrySlot.transform.position = transform.position;
        onKeyItemPlaced.Raise();
        interactingPlayer.carrySlot.Clear();
    }

    public override void OnFocus()
    {
        base.OnFocus();

    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();

    }

    private bool KeyDataMatch(Carriable carriable)
    {
        if (KeyData == carriable.KeyData)
        {
            Debug.Log("KeyData Matches");
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, dimensions);
    }

    private void OnValidate()
    {
        var collider = GetComponent<BoxCollider>();
        collider.size = dimensions;
    }
}
