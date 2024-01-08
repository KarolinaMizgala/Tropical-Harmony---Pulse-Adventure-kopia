using System;

/// <summary>
/// Interface for a dialog system.
/// </summary>
public interface IDialogSystem
{
    /// <summary>
    /// Shows a confirmation dialog with a specified message and actions for confirm and cancel.
    /// </summary>
    /// <param name="message">The message to display in the dialog.</param>
    /// <param name="onConfirm">The action to perform when the dialog is confirmed.</param>
    /// <param name="onCancel">The action to perform when the dialog is cancelled.</param>
    void ShowConfirmationDialog(string message, Action onConfirm, Action onCancel);
}