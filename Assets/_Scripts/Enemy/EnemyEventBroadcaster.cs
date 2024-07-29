using System;
using UnityEngine;

public class EnemyEventBroadcaster : MonoBehaviour
{
    public Room room;
    public enum ObjectType
    {
        Physics, Puzzle, General
    }
    public ObjectType type;

    public EnemySense sense;

    private bool hasFallen = false;
    public float fallThreshold = -10f; // Adjust this threshold as needed
    public float rotationThreshold = 45f; // Adjust this threshold as needed

    private void Awake()
    {
        sense = FindAnyObjectByType<EnemySense>();
    }

    void Update()
    {
        if (type == ObjectType.Physics)
        {
            CheckIfFallen();
        }
    }

    private void CheckIfFallen()
    {
        if (!hasFallen && (transform.position.y < fallThreshold || IsRotationallyFallen()))
        {
            hasFallen = true;
            InvokeSusOccurrence();
        }
    }

    private bool IsRotationallyFallen()
    {
        return Mathf.Abs(transform.eulerAngles.x) > rotationThreshold || Mathf.Abs(transform.eulerAngles.z) > rotationThreshold;
    }

    [ContextMenu("Invoke SusOccurrence")]
    public void InvokeSusOccurrence()
    {
        OnSusOccurrence();
    }

    protected virtual void OnSusOccurrence()
    {
        Debug.Log("Sus occurance broadcasted");
        if (sense != null)
        {
            sense.InvokeSusOccurrence(transform, room);
        }
    }
}
