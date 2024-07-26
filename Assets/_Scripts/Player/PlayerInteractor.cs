using UnityEngine;

/// <summary>
/// PlayerInteractor
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    private PlayerController player;
    private Camera cam;

    [Header("Interaction Settings")]
    [SerializeField] private LayerMask interactionLayers;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private float interactOffset;
    [SerializeField] private float interactHeight;
    [SerializeField] private float interactRadius;

    [SerializeField] private Interactable currentInteractable;
    private Vector3 forwardVector;
    private Vector3 upVector;

    public CarrySlot carrySlot;

    void Awake()
    {
        player = transform.root.GetComponent<PlayerController>();
        cam = Camera.main;
    }

    private void Update()
    {
        forwardVector = (Application.isPlaying ? Vector3.ProjectOnPlane(cam.transform.forward, transform.up).normalized : transform.forward) * interactOffset;
        upVector = transform.up * interactHeight;

        var interactables = Physics.OverlapSphere(transform.position + forwardVector + upVector, interactRadius, interactionLayers);

        if (interactables.Length > 0)
        {
            var interactable = interactables[0];
            if (currentInteractable == null || interactable.gameObject != currentInteractable.gameObject)
            {
                interactable.TryGetComponent(out currentInteractable);

                if (currentInteractable)
                    currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable != null)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }

    void OnSwitch()
    {

    }

    public Carriable GetCarriable()
    {
        return carrySlot.carryItem;
    }

    public void SetCarriable(Carriable carriable)
    {
        carrySlot.carryItem = carriable;
    }

    public void ClearInteractable()
    {
        currentInteractable = null;
    }

    void HandleInteract()
    {
        if (currentInteractable == null)
        {
            if (carrySlot.carryItem != null)
            {
                carrySlot.Drop();
            }
            return;
        }

        if (player.CurrentActivePlayer is HumanMovement)
        {
            Debug.Log("Human Interact");

            currentInteractable.OnInteract(this);

        }
        else if (player.CurrentActivePlayer is ShadowMovement)
        {
            Debug.Log("Shadow Interact");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (!Application.isPlaying )
        {
            forwardVector = (Application.isPlaying ? Vector3.ProjectOnPlane(cam.transform.forward, transform.up).normalized : transform.forward) * interactOffset;
            upVector = transform.up * interactHeight;
        }

        Gizmos.DrawWireSphere(transform.position + forwardVector + upVector, interactRadius);
    }
}
