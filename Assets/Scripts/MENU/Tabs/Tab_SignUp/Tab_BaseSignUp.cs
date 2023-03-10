using UnityEngine;

public abstract class Tab_BaseSignUp : Tab_Base, ITabOperation
{
    [SerializeField] 
    protected CustomInputField[] _customInputFields;

    [SerializeField] [Space]
    protected Tab_Options _tabOptions;

    [SerializeField] [Space]
    protected TabLoading _tabOptionsLoading;

    protected OptionsGameMode _optionsGameMode;

    protected OptionsLogOut _optionsLogOut;

    protected MyPhotonCallbacks _myPhotonCallbacks;

    protected virtual CustomInputField CustomInputFieldEmail { get; }
    protected virtual CustomInputField CustomInputFieldID { get; }
    protected virtual CustomInputField CustomInputFieldPassword { get; }




    protected override void Awake()
    {
        base.Awake();

        _optionsGameMode = FindObjectOfType<OptionsGameMode>();

        _optionsLogOut = FindObjectOfType<OptionsLogOut>();

        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    protected virtual void Update() => SetInteractability();

    public override void OpenTab()
    {
        MyPhoton.LeaveRoom();
        MyPhoton.LeaveLobby();

        CloseOptionsTabs();

        base.OpenTab();
    }

    protected virtual void CloseOptionsTabs()
    {
        _tabOptions.GetOptionsActivityHolder(false);

        _tabOptionsLoading.Close();
    }

    protected override void GoForward()
    {
        OperationHandler = This;

        base.GoForward();

        Confirm();
    }

    protected override void OnOperationSubmitted(ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {
        if (operation == TabsOperation.Operation.Authenticate)
        {
            OperationHandler = handler;

            Authenticate();
        }
    }

    protected abstract void Authenticate();

    protected virtual void OnAuthenticationFailed() => OperationHandler?.OnOperationFailed();

    protected virtual void Confirm() => OpenLoadingTab();

    protected virtual Data.NewData NewData(string userId, string userPassword)
    {
        return new Data.NewData { Id = userId, Password = userPassword };
    }

    protected virtual void SaveUserCredentials(Data.NewData newData) => Data.Manager.SetData(newData);

    protected virtual void CacheUserIds(string playfabId, string entityId, string entityType)
    {
        Data.Manager.PlayfabId = playfabId;
        Data.Manager.EntityID = entityId;
        Data.Manager.EntityType = entityType;
    }

    protected virtual void CacheUserItemsData(string playfabId) => User.UpdateItems(playfabId, 0, 0, 0);

    protected virtual void CacheUserStatisticsData(string playfabId) => User.UpdateStats(playfabId, null, createdStatisticsOutput => { Data.Manager.Statistics = createdStatisticsOutput; });

    protected virtual void SendUserCredentialsToTabHomeOnline(string photonNetworkNickname, string photonNetworkUserId)
    {
        TabsOperation.Handler.SubmitOperation(OperationHandler, TabsOperation.Operation.PlayOnline, new object[] { photonNetworkNickname, photonNetworkUserId });
    }

    protected virtual void SetInteractability() => _btnForward.IsInteractable = CustomInputFieldID.Text.Length > 6 && CustomInputFieldPassword.Text.Length > 4 ? true : false;
}
