using FiveBabbittGames;
using UnityEngine;



    public class ConeDetectionStrategy : IDetectionStrategy
    {
        readonly float detectionAngle;
        readonly float detectionRadius;
        readonly float innerDetectionRadius;

        public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
        {
            this.detectionAngle = detectionAngle;
            this.detectionRadius = detectionRadius;
            this.innerDetectionRadius = innerDetectionRadius;
        }

    public bool Execute(Transform player, Transform detector, CountdownTimer timer)
    {
        if (timer.IsRunning) return false;

        var directionToPlayer = player.position - detector.position;
        var angleToPlayer = Vector3.Angle(directionToPlayer, detector.forward);

        // If the player is not within the detection angle + outer radius (aka the cone in front of the enemy),
        // or is within the inner radius, return false
        if ((!(angleToPlayer < detectionAngle / 2f) || !(directionToPlayer.magnitude < detectionRadius))
            && !(directionToPlayer.magnitude < innerDetectionRadius))
            return false;

        // Cast rays to check if the view of the player is obstructed
        RaycastHit hit;
        Vector3[] raycastDirections = {
        directionToPlayer.normalized,
        (directionToPlayer + Vector3.up * 1f).normalized,
        (directionToPlayer - Vector3.up * 1f).normalized,
        (directionToPlayer + Vector3.right * 1f).normalized,
        (directionToPlayer - Vector3.right * 1f).normalized
    };

        foreach (var dir in raycastDirections)
        {
            if (Physics.Raycast(detector.position, dir, out hit, detectionRadius))
            {
                Debug.Log($"I see: {hit.transform.gameObject.name}");
                if (hit.transform.tag != "Player")
                {
                    return false; // View is obstructed
                }
                else
                {
                    timer.Start();
                    return true;
                }
            }
        }

        timer.Start();
        return true;
    }

}
