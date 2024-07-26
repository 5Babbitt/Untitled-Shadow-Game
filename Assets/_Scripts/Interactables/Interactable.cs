using TMPro;
using UnityEngine;

/// <summary>
/// Interactable
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    public TMP_Text useText;

    public virtual void OnInteract(PlayerInteractor interactingPlayer)
    {
        Debug.Log(interactingPlayer + " interacted with" + name);
    }

    public abstract void OnFocus();
    public abstract void OnLoseFocus();

    public virtual void Start()
    {
        gameObject.layer = 11;

        // useText = UIManager.Instance.GetUseText();
    }
}
