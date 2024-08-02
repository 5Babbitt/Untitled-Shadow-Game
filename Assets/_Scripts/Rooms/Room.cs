using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    [SerializeField]
    public struct Task
    {
        public enum TaskType
        {
            Reading, Crafting, Chemistry
        }
        public TaskType taskType;
        public Transform taskLocation;
        public string animationName;
    }

    public List<Task> tasks = new List<Task>();
    public List<Transform> potentialPlayerLocations = new List<Transform>();
    public Transform enemyEntrance;

    private Dictionary<Transform, float> playerPositionTimes = new Dictionary<Transform, float>();

    private void Start()
    {
        if (enemyEntrance == null)
        {
            GameObject entranceObject = new GameObject("Enemy Entrance");
            enemyEntrance = entranceObject.transform;
            entranceObject.transform.parent = transform; // Set parent to room for better organization
        }

        SetRandomEnemyEntrance();
    }

    private void SetRandomEnemyEntrance()
    {
        NavMeshHit hit;
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * 10f; // Random point within a radius
        if (NavMesh.SamplePosition(randomPosition, out hit, 10f, NavMesh.AllAreas))
        {
            enemyEntrance.position = hit.position;
            Debug.Log($"Enemy entrance set at: {enemyEntrance.position}");
        }
        else
        {
            Debug.LogError("Failed to set a valid enemy entrance position on the NavMesh.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.GetComponent<EnemySense>() != null)
        {
            other.GetComponent<EnemySense>().currentRoom = this;
            other.GetComponent<EnemySense>().inRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.GetComponent<EnemySense>() != null)
        {
            other.GetComponent<EnemySense>().inRoom = false;
            other.GetComponent<EnemySense>().currentRoom = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Transform playerTransform = playerController.transform;

                if (NavMesh.SamplePosition(playerTransform.position, out _, 1f, NavMesh.AllAreas))
                {
                    if (!playerPositionTimes.ContainsKey(playerTransform))
                    {
                        playerPositionTimes[playerTransform] = 0f;
                        potentialPlayerLocations.Add(playerTransform);
                    }
                    else
                    {
                        playerPositionTimes[playerTransform] += Time.deltaTime;
                    }
                }
            }
        }
    }

    public Transform GetMostStagnantPlayerPosition()
    {
        Transform longestStandingPosition = null;
        float maxTime = -1f;

        foreach (var kvp in playerPositionTimes)
        {
            if (kvp.Value > maxTime)
            {
                maxTime = kvp.Value;
                longestStandingPosition = kvp.Key;
            }
        }

        return longestStandingPosition;
    }
}
