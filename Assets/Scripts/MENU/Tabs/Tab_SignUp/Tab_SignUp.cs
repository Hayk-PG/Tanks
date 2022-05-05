using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_SignUp : Tab_Base<MyPhotonCallbacks>
{
    protected Data _data;

    [SerializeField] private InputField _inputFieldEmail;
    [SerializeField] protected InputField _inputFieldId;
    [SerializeField] protected InputField _inputFieldPassword;

    [SerializeField] private Button _confirmButton;

    private string Email
    {
        get => _inputFieldEmail.text;
        set => _inputFieldEmail.text = value;
    }
    protected string Id
    {
        get => _inputFieldId.text;
        set => _inputFieldId.text = value;
    }
    protected string Password
    {
        get => _inputFieldPassword.text;
        set => _inputFieldPassword.text = value;
    }


    protected override void Awake()
    {
        base.Awake();
        _data = FindObjectOfType<Data>();
    }

    protected virtual void OnEnable()
    {
        _object._OnConnectedToMaster += OnPhotonConnectedToMaster;
    }

    protected virtual void OnDisable()
    {
        _object._OnConnectedToMaster += OnPhotonConnectedToMaster;
    }

    protected virtual void Update()
    {
        ButtonSignUpInteractability();
    }

    protected virtual void OnPhotonConnectedToMaster()
    {
        if (!_data.IsAutoSignInChecked)
            base.OpenTab();
    }

    protected virtual void ButtonSignUpInteractability()
    {
        _confirmButton.interactable = Id.Length > 6 && Password.Length > 4 ? true : false;
    }

    public virtual void OnClickConfirmButton()
    {
        MyPhoton.SetNickName(Id);
        SaveAccount();
    }

    protected virtual void SaveAccount()
    {
        _data.SetData(new Data.NewData { Id = Id, Password = Password });
    }
}
