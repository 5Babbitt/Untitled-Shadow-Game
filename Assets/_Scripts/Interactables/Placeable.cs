using FiveBabbittGames;
using UnityEngine;

/// <summary>
/// Placeable
/// </summary>
public class Placeable : Interactable
{
    [Header("Placeable Settings")]
    [SerializeField] protected GameEvent onKeyItemPlaced;
    [SerializeField] protected Carriable carrySlot;
    [SerializeField] protected Vector3 dimensions = Vector3.one;
    private BoxCollider placementCollider;

    public KeyData KeyData;

    public override void Start()
    {
        base.Start();
        placementCollider = GetComponent<BoxCollider>();
    }

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

    protected bool KeyDataMatch(Carriable carriable)
    {
        if (KeyData == carriable.KeyData)
        {
            Debug.Log("KeyData Matches");
            return true;
        }

        return false;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + placementCollider.center, dimensions);
    }

    protected virtual void OnValidate()
    {
        placementCollider = GetComponent<BoxCollider>();
        placementCollider.size = dimensions;
    }
}
