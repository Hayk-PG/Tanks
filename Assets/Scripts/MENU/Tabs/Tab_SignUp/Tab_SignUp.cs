using UnityEngine;
using UnityEngine.UI;

public class Tab_SignUp : Tab_Base<Tab_StartGame>
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
        _object.onPlayOnline += Authoirize;     
    }

    protected virtual void OnDisable()
    {
        _object.onPlayOnline -= Authoirize;
    }

    protected virtual void Update()
    {
        ButtonSignUpInteractability();
    }

    public override void OpenTab()
    {
        MyPhoton.LeaveRoom();
        MyPhoton.LeaveLobby();
        base.OpenTab();
    }

    protected virtual void Authoirize()
    {
        if (!_data.IsAutoSignInChecked)
            OpenTab();
    }

    protected virtual void ButtonSignUpInteractability()
    {
        _confirmButton.interactable = Id.Length > 6 && Password.Length > 4 ? true : false;
    }

    public virtual void OnClickConfirmButton()
    {
        OnEnter();
    }

    protected virtual void OnEnter()
    {
        ExternalData.MyPlayfabRegistrationForm.Register(new MyPlayfabRegistrationValues { ID = Id, Password = Password, EMail = Email });
        SaveAccount();
    }

    protected virtual void SaveAccount()
    {
        _data.SetData(new Data.NewData { Id = Id, Password = Password });
    }
}
