using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_SignUp : Tab_Base<MyPhotonCallbacks>
{
    [SerializeField] private InputField _inputFieldEmail;
    [SerializeField] protected InputField _inputFieldId;
    [SerializeField] protected InputField _inputFieldPassword;

    [SerializeField] private Button _confirmButton;

    private string Email
    {
        get => _inputFieldEmail.text;
    }
    protected string Id
    {
        get => _inputFieldId.text;
    }
    protected string Password
    {
        get => _inputFieldPassword.text;
    }

    public Action<string, string, string> OnSigned { get; set; }


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
        base.OpenTab();
    }

    protected virtual void ButtonSignUpInteractability()
    {
        _confirmButton.interactable = Id.Length > 6 && Password.Length > 4 ? true : false;
    }

    public virtual void OnClickConfirmButton()
    {
        MyPhoton.SetNickName(Id);
    }
}
