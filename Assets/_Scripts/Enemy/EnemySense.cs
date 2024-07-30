using FiveBabbittGames;
using System;
using UnityEngine;

public class EnemySense : MonoBehaviour
{
    [SerializeField] public float detectionAngle = 60f;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float innerDetectionRadius = 5f;
    [SerializeField] float detectCooldown = 1f;
    public Transform player;
    public CountdownTimer detectionTimer;
    private PlayerController playerController;
    IDetectionStrategy coneDetectionStrategy;
    IDetectionStrategy flashlightDetectionStrategy;
    IDetectionStrategy currentDetectionStrategy;
    public Room currentRoom;
    public bool inRoom = false;
    public event Action<Transform, Room> SusOccurance;
    public float EventDetectionRange = 100f;
    public bool eventHeardOutOfRoom = false;
    public bool eventHeardInRoom = false;
    public bool RoomCheckComplete = false;
    public LayerMask humanMask;
    public LayerMask shadowMask;
    public LayerMask obbstructionMasks;
    [HideInInspector] public bool PlayerIsShadow;
    public Light enemyFlashlight;

    private void Start()
    {
        detectionTimer = new CountdownTimer(detectCooldown);
        playerController = FindAnyObjectByType<PlayerController>();

        coneDetectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        flashlightDetectionStrategy = new FlashlightDetectionStrategy(detectionRadius, enemyFlashlight.transform,shadowMask, obstructionLayerMasks: obbstructionMasks);
        currentDetectionStrategy = coneDetectionStrategy; // Start with cone detection

        SusOccurance += OnTransformReceived;
    }

    private void OnDestroy()
    {
        SusOccurance -= OnTransformReceived;
    }

    private void OnTransformReceived(Transform eventTransform, Room detectedRoom)
    {
        var distanceToTarget = Vector3.Distance(transform.position, eventTransform.position);

        // If the target is within the detection radius, start the timer
        if (distanceToTarget < EventDetectionRange)
        {
            if (currentRoom != detectedRoom || currentRoom == null)
            {
                currentRoom = detectedRoom;
                eventHeardOutOfRoom = true;
            }
            else
            {
                currentRoom = detectedRoom;
                eventHeardInRoom = true;
            }
        }
    }

    public void ResetBools()
    {
        eventHeardInRoom = false;
        eventHeardOutOfRoom = false;
    }

    public void Update()
    {
        detectionTimer.Tick(Time.deltaTime);
        player = playerController.CurrentActivePlayer.transform;
        PlayerIsShadow = playerController.isPlayerShadow();

        // Switch detection strategies based on PlayerIsShadow
        if (CanDetectPlayer())
        {
            currentDetectionStrategy = PlayerIsShadow ? flashlightDetectionStrategy : coneDetectionStrategy;
        }
        else
        {
            currentDetectionStrategy = coneDetectionStrategy;
        }
    }

    public void InvokeSusOccurrence(Transform eventTransform, Room detectedRoom)
    {
        SusOccurance?.Invoke(eventTransform, detectedRoom);
    }

    public bool CanDetectPlayer()
    {
        Debug.Log($"current detection strategy: {currentDetectionStrategy}");
        return detectionTimer.IsRunning || currentDetectionStrategy.Execute(player, detector: transform, detectionTimer);
    }

    public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.currentDetectionStrategy = detectionStrategy;

    // Add gizmos to visualize detection area
    private void OnDrawGizmos()
    {
        if (PlayerIsShadow)
        {
            // Visualize spherecasting for flashlight detection
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(enemyFlashlight.transform.position, detectionRadius);

            Vector3 forward = enemyFlashlight.transform.forward * detectionRadius;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(enemyFlashlight.transform.position, forward);

            RaycastHit hit;
            if (Physics.SphereCast(enemyFlashlight.transform.position, enemyFlashlight.spotAngle / 2, enemyFlashlight.transform.forward, out hit, detectionRadius))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(enemyFlashlight.transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.2f);
            }
        }
        
    }
}
