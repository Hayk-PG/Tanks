using System;
using UnityEngine;

public abstract class Tab_BaseSignUp : Tab_Base
{
    [SerializeField] protected CustomInputField[] _customInputFields;
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

    protected override void OnEnable()
    {
        base.OnEnable();

        MenuTabs.Tab_Initialize.onJumpTabSignUp += Authoirize;
        MenuTabs.Tab_StartGame.onPlayOnline += Authoirize;
        _optionsGameMode.onJumpTabSignUp += Authoirize;
        _optionsLogOut.onJumpTabSignUp += Authoirize;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        MenuTabs.Tab_Initialize.onJumpTabSignUp -= Authoirize;
        MenuTabs.Tab_StartGame.onPlayOnline -= Authoirize;
        _optionsGameMode.onJumpTabSignUp -= Authoirize;
        _optionsLogOut.onJumpTabSignUp -= Authoirize;
    }

    protected virtual void Update() => SetInteractability();

    public override void OpenTab()
    {
        MyPhoton.LeaveRoom();
        MyPhoton.LeaveLobby();
        base.OpenTab();
    }

    protected override void GoForward()
    {
        base.GoForward();
        Confirm();
    }

    protected abstract void Authoirize();

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

    protected virtual void CreateUserItemsData(string playfabId) => User.UpdateItems(playfabId, 0, 0, 0);

    protected virtual void ConnectToPhoton(string photonNetworkNickname, string photonNetworkUserId)
    {
        MyPhoton.Connect(photonNetworkNickname, photonNetworkUserId);
    }

    protected virtual void SetInteractability() => _btnForward.IsInteractable = CustomInputFieldID.Text.Length > 6 && CustomInputFieldPassword.Text.Length > 4 ? true : false;
}
