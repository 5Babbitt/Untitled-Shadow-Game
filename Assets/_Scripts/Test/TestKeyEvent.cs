using UnityEngine;

/// <summary>
/// TestKeyEvent
/// </summary>
public class TestKeyEvent : MonoBehaviour
{
    public GameObject objectToSpawm;

    private void Start()
    {
        objectToSpawm.SetActive(false);
    }

    public void TestKeyEventMethod()
    {
        objectToSpawm.SetActive(true);
    }
}

