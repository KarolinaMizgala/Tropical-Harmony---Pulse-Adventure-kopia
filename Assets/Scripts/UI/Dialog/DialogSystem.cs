using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DialogSystem : MonoBehaviour
{

    [Inject]
    private DialogWindow dialogWindowPrefab;


    public void ShowConfirmationDialog(string message, System.Action onConfirm, System.Action onCancel)
    {
       var canvas = GameObject.Find("Canvas");
        DialogWindow dialogInstance = Instantiate(dialogWindowPrefab, canvas.transform);

        dialogInstance.SetMessage(message);
        dialogInstance.SetOnConfirmAction(onConfirm);
        dialogInstance.SetOnCancelAction(onCancel);
    }


}
