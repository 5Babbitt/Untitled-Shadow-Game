using UnityEngine;

/// <summary>
/// Door
/// </summary>
public class Door : Placeable
{
    [Header("Door Settings")]
    [SerializeField] private bool isLocked;
    [SerializeField] private Transform door;
    [SerializeField] private Vector3 rotateAxis = Vector3.up;
    [SerializeField] private float openAngle;
    [SerializeField] private bool isOpen;
    [SerializeField] private float doorTime;

    public bool IsLocked => isLocked;

    public override void Start()
    {

    }

    public override void OnInteract(PlayerInteractor interactingPlayer)
    {

        base.OnInteract(interactingPlayer);

        // If cannot interact or locked, return
        if (!canInteract || isLocked)
            return;

        if (isOpen)
            Close();
        else
            Open();
    }

    public override void OnFocus()
    {
        if (isOpen)
            useText = "close";
        else if (!isOpen)
            useText = "open";

        base.OnFocus();
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
    }

    void Open()
    {
        canInteract = false;
        isOpen = true;
        LeanTween.rotateAround(gameObject, rotateAxis, openAngle, doorTime).setOnComplete(() => OnTweenComplete());
    }

    void Close()
    {
        canInteract = false;
        Debug.Log($"Can Interact: {canInteract}");
        isOpen = false;
        LeanTween.rotateAround(gameObject, rotateAxis, -openAngle, doorTime).setOnComplete(() => OnTweenComplete());
    }

    public void Unlock()
    {
        isLocked = false;
    }

    void OnTweenComplete()
    {
        canInteract = true;
        Debug.Log($"Can Interact: {canInteract}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + rotateAxis.normalized);
        Gizmos.DrawSphere(transform.position + rotateAxis.normalized, 0.1f);
    }
}
