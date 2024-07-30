using FiveBabbittGames;
using UnityEngine;

public class FlashlightDetectionStrategy : IDetectionStrategy
{
    readonly float detectionRadius;
    readonly Transform flashlightTransform;
    readonly Light flashlight;
    readonly LayerMask detectionLayerMask;
    readonly LayerMask obstructionLayerMasks;

    public FlashlightDetectionStrategy(float detectionRadius, Transform flashlightTransform, LayerMask detectionLayerMask, LayerMask obstructionLayerMasks)
    {
        this.detectionRadius = detectionRadius;
        this.flashlightTransform = flashlightTransform;
        this.flashlight = flashlightTransform.GetComponent<Light>();
        this.detectionLayerMask = detectionLayerMask;
        this.obstructionLayerMasks = obstructionLayerMasks;
    }

    public bool Execute(Transform player, Transform detector, CountdownTimer timer)
    {
        if (timer.IsRunning) return false;

        var directionToPlayer = player.position - flashlightTransform.position;

        if (directionToPlayer.magnitude > detectionRadius)
            return false;

        RaycastHit hit;
        if (Physics.SphereCast(flashlightTransform.position, flashlight.spotAngle / 2, flashlightTransform.forward, out hit, detectionRadius, detectionLayerMask | obstructionLayerMasks))
        {
            if (((1 << hit.transform.gameObject.layer) & detectionLayerMask) != 0)
            {
                timer.Start();
                return true; // Player detected
            }
            else if (((1 << hit.transform.gameObject.layer) & obstructionLayerMasks) != 0)
            {
                return false; // View is obstructed
            }
        }

        return false;
    }
}
