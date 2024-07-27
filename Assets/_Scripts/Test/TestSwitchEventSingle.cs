using UnityEngine;

public class TestSwitchEventSingle : MonoBehaviour
{
    public Transform lever;

    public void TestSwitchEventMethod(Component sender, object data)
    {

        Debug.Log("Move Man Single");
        LeanTween.rotateZ(lever.gameObject, -lever.localEulerAngles.z, 0.1f);
        LeanTween.moveY(gameObject, 0, 5);
    }
}

