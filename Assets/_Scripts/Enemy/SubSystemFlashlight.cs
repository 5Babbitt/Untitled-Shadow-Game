using UnityEngine;

public class SubSystemFlashlight : MonoBehaviour
{
    public bool FlashlightEnabled { get; set; }
    private Light flashlight;
    private Transform playerTransform;
    private float rotationSpeed = 5f; // Speed at which the flashlight rotates towards the player

    void Awake()
    {
        flashlight = GetComponent<Light>();
       // playerTransform = FindObjectOfType<PlayerController>().CurrentActivePlayer.transform; // Get the player's transform from PlayerController
        Debug.Log("Flashlight system online");
        if (flashlight == null)
        {
            Debug.LogError("No Light component found on the SubSystemFlashlight object.");
        }
    }

    void Update()
    {
        if (FlashlightEnabled && flashlight != null)
        {
            flashlight.enabled = true;
            playerTransform = FindObjectOfType<PlayerController>().CurrentActivePlayer.transform; // Get the player's transform from PlayerController
            if (playerTransform != null)
            {
                // Rotate the flashlight towards the player
                Vector3 directionToPlayer = playerTransform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else if (flashlight != null)
        {
            flashlight.enabled = false;
        }
    }
}
