using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractiveButton : Button
{
    [SerializeField]
    public UnityEvent onInteractionFinished;

    protected override void Start()
    {
        base.Start();

        onClick.AddListener(AnimateButtonPressing);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        onClick.RemoveListener(AnimateButtonPressing);
    }

    private void AnimateButtonPressing()
    {
        if (interactable)
        {
            InteractiveSystem.AnimateButtonPressing(this);
        }
    }
}
