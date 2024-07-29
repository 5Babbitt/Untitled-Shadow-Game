using UnityEngine;

/// <summary>
/// Snippet1
/// </summary>
public class Snippet : Interactable
{
    [SerializeField] private string loreSnippet;

    private bool isViewing;
    public bool IsViewing => isViewing;

    public override void OnInteract(PlayerInteractor interactingPlayer)
    {
        base.OnInteract(interactingPlayer);

        if (!isViewing)
        {
            if (HUDController.Instance != null)
                HUDController.Instance.ViewSnippet(loreSnippet);
            isViewing = true;
        }
        else
        {
            if (HUDController.Instance != null)
                HUDController.Instance.CloseSnippet();
            isViewing = false;
        }
    }

    public override void OnFocus()
    {
        useText = "Read";

        base.OnFocus();
    }

    public override void OnLoseFocus()
    {
        base.OnLoseFocus();
    }

    protected override void UpdateInteractText()
    {


        base.UpdateInteractText();
    }
}
