using UnityEngine;

/// <summary>
/// CarrySlot
/// </summary>
public class CarrySlot : MonoBehaviour
{
    public PlayerInteractor player;
    public Carriable carryItem;

    public Transform carryTransform;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerInteractor>();
    }

    public void Clear()
    {
        carryItem = null;
    }

    public void Drop()
    {
        carryItem.Drop(player);
    }
}
