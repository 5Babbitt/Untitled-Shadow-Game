using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Room
/// </summary>

public class Room : MonoBehaviour
{
    [SerializeField]
    public struct Task
    {
        public enum TaskType
        {
            Reading,Crafting,Chemistry
        }
        public TaskType taskType;
        public Transform taskLocation;
        public string animationName;

    }
    public List<Task> tasks = new List<Task>();
    public List<Transform> potentialPlayerLocations = new List<Transform>();
    public Transform enemyEnterance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && other.GetComponent<EnemySense>() != null) 
        {
            other.GetComponent<EnemySense>().currentRoom = this;
            other.GetComponent<EnemySense>().inRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<EnemySense>().inRoom = false;
        other.GetComponent<EnemySense>().currentRoom = null;
    }
}
