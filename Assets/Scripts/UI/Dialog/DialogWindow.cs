using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindow : MonoBehaviour
{
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TMP_Text dialogText;

    private System.Action onConfirmAction;
    private System.Action onCancelAction;

   
    public void SetMessage(string message)
    {
        dialogText.text = message;
    }

   
    public void SetOnConfirmAction(System.Action onConfirm)
    {
        onConfirmAction = onConfirm;
    }


    public void SetOnCancelAction(System.Action onCancel)
    {
        onCancelAction = onCancel;
    }

    void Start()
    {
        yesButton.onClick.AddListener(OnConfirmButtonClick);
        noButton.onClick.AddListener(OnCancelButtonClick);
    }

    void OnConfirmButtonClick()
    {
        if (onConfirmAction != null)
        {
            onConfirmAction.Invoke();
        }

        CloseDialog();
    }

    void OnCancelButtonClick()
    {
        if (onCancelAction != null)
        {
            onCancelAction.Invoke();
        }

        CloseDialog();
    }

    public void CloseDialog()
    {
        Destroy(gameObject);
    }
}
