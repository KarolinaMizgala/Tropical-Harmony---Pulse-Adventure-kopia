using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class InteractiveSystem : MonoBehaviour
{
    public static UnityEvent onInteractionFinished;

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
