using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Class representing an interactive button with animation.
/// </summary>
public class InteractiveButton : Button
{
    /// <summary>
    /// Event triggered when the interaction with the button is finished.
    /// </summary>
    [SerializeField]
    public UnityEvent onInteractionFinished;

    /// <summary>
    /// Overridden Start method from Unity's MonoBehaviour. 
    /// Adds the AnimateButtonPressing method to the button's onClick event.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        onClick.AddListener(AnimateButtonPressing);
    }

    /// <summary>
    /// Overridden OnDestroy method from Unity's MonoBehaviour. 
    /// Removes the AnimateButtonPressing method from the button's onClick event.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();

        onClick.RemoveListener(AnimateButtonPressing);
    }

    /// <summary>
    /// Animates the button pressing if the button is interactable.
    /// </summary>
    private void AnimateButtonPressing()
    {
        if (interactable)
        {
            InteractiveSystem.AnimateButtonPressing(this);
        }
    }
}