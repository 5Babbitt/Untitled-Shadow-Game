using UnityEngine;

/// <summary>
/// EscapeTrigger
/// </summary>
[RequireComponent (typeof(BoxCollider), typeof(Rigidbody))]
public class EscapeTrigger : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.Escaped();
        }
    }
}
