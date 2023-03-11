using UnityEngine;
using TMPro;

public class UserNameGUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _txtUserName;

    private ITabOperation _operationHandler;




    private void OnEnable() => TabsOperation.Handler.onOperationSubmitted += DisplayUserName;

    private void OnDisable() => TabsOperation.Handler.onOperationSubmitted -= DisplayUserName;

    private void DisplayUserName (ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        if(operation == TabsOperation.Operation.UserProfile)
        {
            _operationHandler = handler;

            if(Data.Manager.Id == null)
            {
                _operationHandler.OnOperationFailed();

                return;
            }

            _txtUserName.text = Data.Manager.Id;
        }
    }
}
