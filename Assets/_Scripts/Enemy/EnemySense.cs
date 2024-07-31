using FiveBabbittGames;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemySense : MonoBehaviour
{
    [SerializeField] public float detectionAngle = 60f;
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float innerDetectionRadius = 5f;
    [SerializeField] float detectCooldown = 1f;
    [SerializeField] float loseSightCooldown = 3f; // Time to wait before losing sight of the player
    [SerializeField] float stoppingDistanceHuman = 2f; // Stopping distance when player is human
    [SerializeField] float stoppingDistanceShadow = 5f; // Stopping distance when player is shadow

    public Transform player;
    public CountdownTimer detectionTimer;
    private CountdownTimer loseSightTimer;
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
    public LayerMask obstructionMasks;
    [HideInInspector] public bool PlayerIsShadow;
    public Light enemyFlashlight;
    public NavMeshAgent agent; // Reference to the NavMeshAgent
    private bool canSeePlayer;

    private void Start()
    {
        detectionTimer = new CountdownTimer(detectCooldown);
        loseSightTimer = new CountdownTimer(loseSightCooldown);
        playerController = FindAnyObjectByType<PlayerController>();

        coneDetectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        flashlightDetectionStrategy = new FlashlightDetectionStrategy(detectionRadius, enemyFlashlight.transform);
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

    private void Update()
    {
        detectionTimer.Tick(Time.deltaTime);
        loseSightTimer.Tick(Time.deltaTime);

        player = playerController.CurrentActivePlayer.transform;

        PlayerIsShadow = playerController.CurrentActivePlayer is ShadowMovement;

        PlayerIsShadow = playerController.isShadow;

        bool playerDetected = currentDetectionStrategy.Execute(player, detector: transform, detectionTimer);

        if (playerDetected)
        {
            loseSightTimer.Stop(); // Reset lose sight timer
            canSeePlayer = true;

            // Track player with flashlight
            if(PlayerIsShadow)
            {
                Debug.Log("Moving flashlight");
                Vector3 directionToPlayer = player.position - enemyFlashlight.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                enemyFlashlight.transform.rotation = Quaternion.Slerp(enemyFlashlight.transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
           

            if(agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }
            // Set agent stopping distanc
            agent.stoppingDistance = PlayerIsShadow ? stoppingDistanceShadow : stoppingDistanceHuman;
            agent.SetDestination(player.position);

            // Spherecasting for shadow detection
            if (PlayerIsShadow)
            {
                float spherecastRadius = enemyFlashlight.spotAngle / 2;
                RaycastHit hit;
                if (Physics.SphereCast(enemyFlashlight.transform.position, spherecastRadius, enemyFlashlight.transform.forward, out hit, detectionRadius))
                {
                    if (hit.transform == player)
                    {
                        Debug.Log("Player detected in shadow form. Invoking example function.");
                        ExampleFunction(); // Call example function when player is detected
                    }
                }
            }
        }
        else if (canSeePlayer && !loseSightTimer.IsRunning)
        {
            loseSightTimer.Start();
        }

        // If lose sight timer has elapsed without seeing the player, set canSeePlayer to false
        if (!playerDetected && loseSightTimer.IsFinished)
        {
            canSeePlayer = false;
        }


        // Switch detection strategies based on PlayerIsShadow
        currentDetectionStrategy = PlayerIsShadow ? flashlightDetectionStrategy : coneDetectionStrategy;
    }


    public bool CanActuallyDetectPlayer()
    {
        return canSeePlayer;
    }


    public void InvokeSusOccurrence(Transform eventTransform, Room detectedRoom)
    {
        SusOccurance?.Invoke(eventTransform, detectedRoom);
    }

    public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.currentDetectionStrategy = detectionStrategy;

    private void ExampleFunction()
    {
        // Example function to be called when the player is detected in shadow form
        Debug.Log("Example function called.");
    }

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
        else
        {
            // Visualize cone detection
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.GameOver();
        }
    }
}
