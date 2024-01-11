using UnityEngine;
using Zenject;

/// <summary>
/// Class representing a dialog system.
/// </summary>
public class DialogSystem : MonoBehaviour
{
    /// <summary>
    /// Prefab for the dialog window.
    /// </summary>
    [Inject]
    private DialogWindow dialogWindowPrefab;

    /// <summary>
    /// Shows a confirmation dialog with a specified message and actions for confirm and cancel.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="onConfirm">The action to perform when the dialog is confirmed.</param>
    /// <param name="onCancel">The action to perform when the dialog is cancelled.</param>
    public void ShowConfirmationDialog(string message, System.Action onConfirm, System.Action onCancel)
    {
        var canvas = GameObject.Find("Canvas");
        DialogWindow dialogInstance = Instantiate(dialogWindowPrefab, canvas.transform);

        dialogInstance.SetMessage(message);
        dialogInstance.SetOnConfirmAction(onConfirm);
        dialogInstance.SetOnCancelAction(onCancel);
    }
    public void OnCancel()
      {
        DialogWindow dialogWindow = FindObjectOfType<DialogWindow>();
        if (dialogWindow != null)
        {
            dialogWindow.CloseDialog();
        }

        }
}