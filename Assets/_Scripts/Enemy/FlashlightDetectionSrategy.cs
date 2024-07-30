using FiveBabbittGames;
using UnityEngine;

public class FlashlightDetectionStrategy : IDetectionStrategy
{
    readonly float detectionRadius;
    readonly Transform flashlightTransform;
    readonly Light flashlight;

    public FlashlightDetectionStrategy(float detectionRadius, Transform flashlightTransform)
    {
        this.detectionRadius = detectionRadius;
        this.flashlightTransform = flashlightTransform;
        this.flashlight = flashlightTransform.GetComponent<Light>();
    }

    public bool Execute(Transform player, Transform detector, CountdownTimer timer)
    {
        if (timer.IsRunning) return false;

        var directionToPlayer = player.position - flashlightTransform.position;

        if (directionToPlayer.magnitude > detectionRadius)
            return false;

        RaycastHit hit;
        if (Physics.SphereCast(flashlightTransform.position, flashlight.spotAngle / 2, flashlightTransform.forward, out hit, detectionRadius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                timer.Start();
                return true; // Player detected
            }
        }

        return false;
    }
}
