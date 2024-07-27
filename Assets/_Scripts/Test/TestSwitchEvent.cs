using UnityEngine;

/// <summary>
/// TestSwitchEvent
/// </summary>
public class TestSwitchEvent : MonoBehaviour
{
    public float startHeight = -49.1f;
    public float endHeight = 0;
    public Transform lever;

    public void TestSwitchEventMethod(Component sender, object data)
    {
        bool value = (bool)data;

        LeanTween.rotateZ(lever.gameObject, -lever.localEulerAngles.z, 0.1f);

        if (value)
        {
            Debug.Log("Move Man Up");
            LeanTween.moveY(gameObject, endHeight, 5);
        }
        else
        {
            Debug.Log("Move Man Down");
            LeanTween.moveY(gameObject, startHeight, 5);
        }
    }
}

