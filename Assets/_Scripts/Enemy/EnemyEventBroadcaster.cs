using System;
using System.Collections;
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
    private bool initialSetup = false;
    public float fallThreshold = -10f; // Adjust this threshold as needed
    public float rotationThreshold = 45f; // Adjust this threshold as needed

    private float initialYPosition;
    private Vector3 initialRotation;

    private void Awake()
    {
        sense = FindAnyObjectByType<EnemySense>();
        room = gameObject.transform.root.GetComponentInChildren<Room>();
        if (room != null)
        {
            Debug.Log("Room Successfully grabbed!");
            StartCoroutine(WaitForInitialDrop());
        }
        initialYPosition = transform.position.y;
        initialRotation = transform.eulerAngles;
        Debug.Log($"Initial Y Position set to: {initialYPosition}, Initial Rotation: {initialRotation}");
    }

    private void Update()
    {
        if (initialSetup && type == ObjectType.Physics)
        {
            CheckIfFallen();
        }
    }

    private IEnumerator WaitForInitialDrop()
    {
        initialSetup = false;
        yield return new WaitForSeconds(1f); // Wait for initial stabilization
        fallThreshold = initialYPosition + fallThreshold;
        initialSetup = true;
        Debug.Log($"Initial setup complete. Fall threshold set to: {fallThreshold}");
    }

    private void CheckIfFallen()
    {
        if (!hasFallen && initialSetup && IsFallen())
        {
            hasFallen = true;
            Debug.Log("Object has fallen. Broadcasting SusOccurance.");
            InvokeSusOccurrence();
        }
    }

    private bool IsFallen()
    {
        bool hasVerticalFall = transform.position.y < fallThreshold;
        bool hasRotationalFall = IsRotationallyFallen();

        if (hasVerticalFall)
        {
            Debug.Log("Vertical fall detected.");
        }

        if (hasRotationalFall)
        {
            Debug.Log("Rotational fall detected.");
        }

        return hasVerticalFall || hasRotationalFall;
    }

    private bool IsRotationallyFallen()
    {
        // Calculate the change in rotation from the initial setup
        float deltaXRotation = Mathf.DeltaAngle(initialRotation.x, transform.eulerAngles.x);
        float deltaZRotation = Mathf.DeltaAngle(initialRotation.z, transform.eulerAngles.z);

        bool xRotationExceeded = Mathf.Abs(deltaXRotation) > rotationThreshold;
        bool zRotationExceeded = Mathf.Abs(deltaZRotation) > rotationThreshold;

        if (xRotationExceeded)
        {
            Debug.Log($"X rotation exceeded: {deltaXRotation} > {rotationThreshold}");
        }

        if (zRotationExceeded)
        {
            Debug.Log($"Z rotation exceeded: {deltaZRotation} > {rotationThreshold}");
        }

        return xRotationExceeded || zRotationExceeded;
    }

    [ContextMenu("Invoke SusOccurrence")]
    public void InvokeSusOccurrence()
    {
        OnSusOccurrence();
    }

    protected virtual void OnSusOccurrence()
    {
        Debug.Log($"Sus occurrence broadcasted from {gameObject.name}");
        if (sense != null)
        {
            sense.InvokeSusOccurrence(transform, room);
        }
    }
}
