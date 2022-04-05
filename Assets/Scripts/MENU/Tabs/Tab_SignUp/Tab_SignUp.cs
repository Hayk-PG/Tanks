using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_SignUp : Tab_Base<MyPhotonCallbacks>
{
    [SerializeField] private InputField _inputFieldEmail;
    [SerializeField] private InputField _inputFieldId;
    [SerializeField] private InputField _inputFieldPassword;

    [SerializeField] private Button _buttonSignUp;

    private string Email
    {
        get => _inputFieldEmail.text;
    }
    private string Id
    {
        get => _inputFieldId.text;
    }
    private string Password
    {
        get => _inputFieldPassword.text;
    }

    public Action<string, string, string> OnSignedUp { get; set; }


    private void OnEnable()
    {
        _object.OnPhotonConnectedToMaster += OnPhotonConnectedToMaster;
    }

    private void OnDisable()
    {
        _object.OnPhotonConnectedToMaster += OnPhotonConnectedToMaster;
    }

    private void Update()
    {
        ButtonSignUpInteractability();
    }

    private void OnPhotonConnectedToMaster()
    {
        MenuTabs.Activity(MenuTabs.Tab_SignUp.CanvasGroup);
    }

    private void ButtonSignUpInteractability()
    {
        _buttonSignUp.interactable = Id.Length > 6 && Password.Length > 4 ? true : false;
    }

    public void OnClickSignUpButton()
    {
        MyPhoton.SetNickName(Id);
    }
}
