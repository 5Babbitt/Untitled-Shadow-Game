using FiveBabbittGames;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightDetectionStrategy : IDetectionStrategy
{
    readonly float detectionRadius;
    readonly Transform flashlightTransform;
    readonly Light flashlight;
    readonly LayerMask obstructionMasks;
    public EnemySense sensor;

    public FlashlightDetectionStrategy(float detectionRadius, Transform flashlightTransform, LayerMask obstructionMasks,EnemySense sensor)
    {
        this.detectionRadius = detectionRadius;
        this.flashlightTransform = flashlightTransform;
        this.flashlight = flashlightTransform.GetComponent<Light>();
        this.obstructionMasks = obstructionMasks;
        this.sensor = sensor;
    }

    public bool Execute(Transform player, Transform detector, CountdownTimer timer)
    {
        if (timer.IsRunning) return false;

        Vector3 directionToPlayer = player.position - flashlightTransform.position;

        if (directionToPlayer.magnitude > detectionRadius)
            return false;

        RaycastHit hit;

        // Include obstruction mask in the SphereCast
        bool isHit = Physics.SphereCast(flashlightTransform.position, flashlight.spotAngle / 2, flashlightTransform.forward, out hit, detectionRadius, ~obstructionMasks);

        // Visualize the ray and spherecast
        Debug.DrawRay(flashlightTransform.position, flashlightTransform.forward * detectionRadius, Color.blue, 0.5f);
        Debug.DrawRay(flashlightTransform.position, directionToPlayer, Color.red, 0.5f);

        if (isHit)
        {
            if (hit.transform.CompareTag("Player"))
            {
                timer.Start();
                Debug.Log($"Detected {hit.transform.name} and it is the player");
                Vector3 hitLocation = player.position - flashlightTransform.position;
                // Visualize the hit with a line and sphere at the hit point
                Debug.DrawLine(flashlightTransform.position, hit.point, Color.green, 0.5f);
                Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.green, 0.5f); // Small line to indicate hit point
                //sensor.enemyFlashObject.GetComponent<SubSystemFlashlight>().FlashlightEnabled = true;
                Quaternion targetRotation = Quaternion.LookRotation(hitLocation);
               //sensor.enemyFlashObject.transform.rotation = Quaternion.Slerp(sensor.enemyFlashObject.transform.rotation, targetRotation, Time.deltaTime * 10f);

                return true; // Player detected
            }
            else
            {
                Debug.Log($"Detected {hit.transform.name} but it is not the player. Obstructed by: {hit.transform.tag}");
                //sensor.enemyFlashObject.GetComponent<SubSystemFlashlight>().FlashlightEnabled = false;
                // Visualize the obstruction with a line and sphere at the hit point
                Debug.DrawLine(flashlightTransform.position, hit.point, Color.red, 0.5f);
                Debug.DrawRay(hit.point, Vector3.up * 0.5f, Color.red, 0.5f); // Small line to indicate obstruction point
            }
        }

        return false;
    }
}
