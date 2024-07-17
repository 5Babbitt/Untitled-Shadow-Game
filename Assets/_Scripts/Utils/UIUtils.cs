using UnityEngine;
using UnityEngine.UIElements;

public static class UIUtils
{
    public static void CopyToClipboard(this string text)
    {
        GUIUtility.systemCopyBuffer = text;
    }
    
    public static void MakeDisableInputOnHide(VisualElement ve)
    {
        ve.RegisterCallback<TransitionEndEvent>(OnTransitionEnd);

        void OnTransitionEnd(TransitionEndEvent evt)
        {
            var ve = (VisualElement)evt.target;
            var isVisible = ve.resolvedStyle.opacity > 0f;

            ve.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public static void RegisterChangePositioningOnTransitions(VisualElement ve)
    {
        ve.RegisterCallback<TransitionStartEvent>(OnTransitionStart);

        void OnTransitionStart(TransitionStartEvent evt)
        {
            var ve = (VisualElement)evt.target;
            var classes = ve.GetClasses();
            foreach (var className in classes)
            {
                if (className.Contains("hidden"))
                {
                    ve.pickingMode = PickingMode.Ignore;
                    return;
                }
            }

            ve.pickingMode = PickingMode.Position;
        }
    }

}
