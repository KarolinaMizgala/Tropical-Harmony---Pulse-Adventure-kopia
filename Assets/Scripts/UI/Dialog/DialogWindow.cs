using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class representing a dialog window.
/// </summary>
public class DialogWindow : MonoBehaviour
{
    /// <summary>
    /// Button for confirming the dialog.
    /// </summary>
    [SerializeField] private Button yesButton;

    /// <summary>
    /// Button for cancelling the dialog.
    /// </summary>
    [SerializeField] private Button noButton;

    /// <summary>
    /// Text component for displaying the dialog message.
    /// </summary>
    [SerializeField] private TMP_Text dialogText;

    /// <summary>
    /// Action to perform when the dialog is confirmed.
    /// </summary>
    private System.Action onConfirmAction;

    /// <summary>
    /// Action to perform when the dialog is cancelled.
    /// </summary>
    private System.Action onCancelAction;

    /// <summary>
    /// Sets the message to be displayed in the dialog window.
    /// </summary>
    /// <param name="message">The message to display.</param>
    public void SetMessage(string message)
    {
        dialogText.text = message;
    }

    /// <summary>
    /// Sets the action to be performed when the confirm button is clicked.
    /// </summary>
    /// <param name="onConfirm">The action to perform on confirm.</param>
    public void SetOnConfirmAction(System.Action onConfirm)
    {
        onConfirmAction = onConfirm;
    }

    /// <summary>
    /// Sets the action to be performed when the cancel button is clicked.
    /// </summary>
    /// <param name="onCancel">The action to perform on cancel.</param>
    public void SetOnCancelAction(System.Action onCancel)
    {
        onCancelAction = onCancel;
    }

    /// <summary>
    /// Adds listeners to the confirm and cancel buttons.
    /// </summary>
    void Start()
    {
        yesButton.onClick.AddListener(OnConfirmButtonClick);
        noButton.onClick.AddListener(OnCancelButtonClick);
    }

    /// <summary>
    /// Invokes the confirm action and closes the dialog.
    /// </summary>
    void OnConfirmButtonClick()
    {
        if (onConfirmAction != null)
        {
            onConfirmAction.Invoke();
        }

        CloseDialog();
    }

    /// <summary>
    /// Invokes the cancel action and closes the dialog.
    /// </summary>
    void OnCancelButtonClick()
    {
        if (onCancelAction != null)
        {
            onCancelAction.Invoke();
        }

        CloseDialog();
    }

    /// <summary>
    /// Closes the dialog window.
    /// </summary>
    public void CloseDialog()
    {
        Destroy(gameObject);
    }
}