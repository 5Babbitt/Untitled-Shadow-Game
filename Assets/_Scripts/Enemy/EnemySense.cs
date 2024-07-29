using FiveBabbittGames;
using System;
using UnityEngine;

public class EnemySense : MonoBehaviour
{
    [SerializeField] float detectionAngle = 60f;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float innerDetectionRadius = 5f;
    [SerializeField] float detectCooldown = 1f;
    public Transform player;
    public CountdownTimer detectionTimer;
    private PlayerController playerController;
    IDetectionStrategy detectionStrategy;
    public Room currentRoom;
    public bool inRoom = false;
    public event Action<Transform, Room> SusOccurance;
    public float EventDetectionRange = 100f;
    public bool eventHeardOutOfRoom = false;
    public bool eventHeardInRoom = false;
    public bool RoomCheckComplete = false;

    private void Start()
    {
        detectionTimer = new CountdownTimer(detectCooldown);
        playerController = FindAnyObjectByType<PlayerController>();

        detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
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
    }
    public void InvokeSusOccurrence(Transform eventTransform, Room detectedRoom)
    {
        SusOccurance?.Invoke(eventTransform, detectedRoom);
    }
    public bool CanDetectPlayer()
    {
        return detectionTimer.IsRunning || detectionStrategy.Execute(player, detector: transform, detectionTimer);
    }

    public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;

    // Add gizmos to visualize detection area
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

        Gizmos.color = Color.green;
        Vector3 forward = transform.forward * detectionRadius;
        Vector3 left = Quaternion.Euler(0, -detectionAngle / 2, 0) * forward;
        Vector3 right = Quaternion.Euler(0, detectionAngle / 2, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + left);
        Gizmos.DrawLine(transform.position, transform.position + right);

        int rayCount = 10;
        for (int i = 0; i <= rayCount; i++)
        {
            float angle = -detectionAngle / 2 + i * (detectionAngle / rayCount);
            Vector3 rayDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
            Ray ray = new Ray(transform.position, rayDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionRadius))
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(ray.origin, hit.point);
                Gizmos.DrawSphere(hit.point, 0.2f);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, rayDirection * detectionRadius);
            }
        }
    }
}
