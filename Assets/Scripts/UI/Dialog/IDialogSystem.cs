using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogSystem 
{
    void ShowConfirmationDialog( string message, Action onConfirm, Action onCancel);
}
