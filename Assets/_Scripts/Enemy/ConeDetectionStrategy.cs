using FiveBabbittGames;
using UnityEngine;

public class ConeDetectionStrategy : IDetectionStrategy
{
    readonly float detectionAngle;
    readonly float detectionRadius;
    readonly float innerDetectionRadius;
    readonly float rayHeightStep = 0.5f; // Distance between vertical raycasts
    readonly int numberOfRays = 5; // Number of vertical rays

    public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
    {
        this.detectionAngle = detectionAngle;
        this.detectionRadius = detectionRadius;
        this.innerDetectionRadius = innerDetectionRadius;
    }

    public bool Execute(Transform player, Transform detector, CountdownTimer timer)
    {
        if (timer.IsRunning) return false;
        if (player == null) return false;
        Debug.Log("Trying to detect player");
        Vector3 detectorPosition = detector.position;
        Vector3 playerPosition = player.position;

        // Project positions onto the XZ plane to ignore height differences
        Vector3 flatDetectorPosition = new Vector3(detectorPosition.x, 0, detectorPosition.z);
        Vector3 flatPlayerPosition = new Vector3(playerPosition.x, 0, playerPosition.z);

        Vector3 directionToPlayer = (flatPlayerPosition - flatDetectorPosition).normalized;
        float distanceToPlayer = Vector3.Distance(flatDetectorPosition, flatPlayerPosition);
        float angleToPlayer = Vector3.Angle(detector.forward, directionToPlayer);

        if (angleToPlayer < detectionAngle / 2 && distanceToPlayer <= detectionRadius)
        {
            bool playerDetected = false;

            // Cast multiple rays in a vertical line
            for (int i = 0; i < numberOfRays; i++)
            {
                float heightOffset = i * rayHeightStep;
                Vector3 rayOrigin = detectorPosition + Vector3.up * heightOffset;

                if (Physics.Raycast(rayOrigin, directionToPlayer, out RaycastHit hit, detectionRadius * 2.5f))
                {
                    Debug.DrawRay(rayOrigin, directionToPlayer * detectionRadius, Color.green, 1.0f);
                    if (hit.transform.root.CompareTag("Player"))
                    {
                        Debug.Log($"Hit player object called {hit.transform.gameObject.name} at height {heightOffset}");
                        playerDetected = true;
                        timer.Start();
                        break; // Stop further checks if player is detected
                    }
                }
                else
                {
                    Debug.DrawRay(rayOrigin, directionToPlayer * detectionRadius, Color.red, 1.0f);
                }
            }

            return playerDetected;
        }

        return false;
    }
}
    