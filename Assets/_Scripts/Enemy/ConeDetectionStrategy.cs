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
 
            if (player == null) return false;

            Vector3 directionToPlayer = (player.position - detector.position).normalized;
            float angleToPlayer = Vector3.Angle(detector.forward, directionToPlayer);

            if (angleToPlayer < detectionAngle / 2 && Vector3.Distance(detector.position, player.position) <= detectionRadius)
            {
                RaycastHit hit;
                Vector3 rayOrigin = detector.position + Vector3.up; // Adjusting the ray origin to eye level
                if (Physics.Raycast(rayOrigin, directionToPlayer, out hit, detectionRadius))
                {
                    Debug.DrawRay(rayOrigin, directionToPlayer * detectionRadius, Color.green, 1.0f);
                    // Debug.Log($"Raycast hit: {hit.transform.name}");
                    if (hit.transform.tag == "Player")
                    {
                        return true;
                    }
                }
                else
                {
                    Debug.DrawRay(rayOrigin, directionToPlayer * detectionRadius, Color.red, 1.0f);
                }
            }
            return false;
        
    }

}
