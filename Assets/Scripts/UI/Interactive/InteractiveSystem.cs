using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// Class representing an interactive system for animating button presses.
/// </summary>
public class InteractiveSystem : MonoBehaviour
{
    /// <summary>
    /// Event triggered when the interaction with the button is finished.
    /// </summary>
    public static UnityEvent onInteractionFinished;

    /// <summary>
    /// Animates the pressing of an InteractiveButton.
    /// </summary>
    /// <param name="button">The InteractiveButton to animate.</param>
    public static void AnimateButtonPressing(InteractiveButton button)
    {
        if (Time.timeScale == 0)
        {
            button.transform.DOScale(0.7f, 0.1f).SetUpdate(true).OnComplete(() =>
            {
                button.transform.DOScale(1f, 0.1f).SetUpdate(true).OnComplete(() =>
                {
                    button.onInteractionFinished.Invoke();
                });
            });

            return;
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Append(button.transform.DOScale(0.7f, 0.1f));
        sequence.Append(button.transform.DOScale(1f, 0.1f));
        sequence.AppendCallback(() => button.onInteractionFinished.Invoke());
    }
}