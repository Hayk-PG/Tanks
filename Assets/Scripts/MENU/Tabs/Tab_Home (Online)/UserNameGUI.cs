using UnityEngine;
using TMPro;

public class UserNameGUI : MonoBehaviour, ITabOperation
{
    [SerializeField]
    private TMP_Text _txtUserName;

    public ITabOperation This { get; set; }
    public ITabOperation OperationHandler { get; set; }




    private void Awake() => This = this;

    private void OnEnable() => TabsOperation.Handler.onOperationSubmitted += DisplayUserName;

    private void OnDisable() => TabsOperation.Handler.onOperationSubmitted -= DisplayUserName;

    private void DisplayUserName (ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        if(operation == TabsOperation.Operation.UserProfile)
        {
            OperationHandler = handler;

            if(Data.Manager.Id == null)
            {
                OperationHandler.OnOperationFailed();

                return;
            }

            OperationHandler.OnOperationSucceded();

            _txtUserName.text = Data.Manager.Id;
        }
    }

    public void OnOperationSucceded()
    {
        
    }

    public void OnOperationFailed()
    {
        
    }
}
