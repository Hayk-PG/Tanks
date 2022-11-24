using UnityEngine;
using UnityEngine.UI;

public class Tab_SignUp : Tab_Base<Tab_StartGame>
{
    [SerializeField] protected CustomInputField[] _customInputFields;
    [SerializeField] protected Btn _btn;
    protected Data _data;

    protected virtual CustomInputField CustomInputFieldEmail => _customInputFields[0];
    protected virtual CustomInputField CustomInputFieldID => _customInputFields[1];
    protected virtual CustomInputField CustomInputFieldPassword => _customInputFields[2];


    protected override void Awake()
    {
        base.Awake();
        _data = FindObjectOfType<Data>();
    }

    protected virtual void Update()
    {
        SetInteractability();
    }

    protected virtual void OnEnable()
    {
        _object.onPlayOnline += Authoirize;
        _btn.onSelect += Confirm;
    }

    protected virtual void OnDisable()
    {
        _object.onPlayOnline -= Authoirize;
        _btn.onSelect -= Confirm;
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

    public virtual void Confirm()
    {
        MyPlayfabRegistrationValues myPlayfabRegistrationValues = new MyPlayfabRegistrationValues
        {
            EMail = CustomInputFieldEmail.Text,
            ID = CustomInputFieldID.Text,
            Password = CustomInputFieldPassword.Text
        };

        ExternalData.MyPlayfabRegistrationForm.Register(myPlayfabRegistrationValues, result =>
        {
            SaveData(NewData(myPlayfabRegistrationValues));
        });
    }

    protected virtual Data.NewData NewData(MyPlayfabRegistrationValues myPlayfabRegistrationValues)
    {
        return new Data.NewData { Id = myPlayfabRegistrationValues.ID, Password = myPlayfabRegistrationValues.Password };
    }

    protected virtual void SaveData(Data.NewData newData)
    {
        _data.SetData(newData);
    }

    protected virtual void SetInteractability()
    {
        _btn.IsInteractable = CustomInputFieldID.Text.Length > 6 && CustomInputFieldPassword.Text.Length > 4? true : false;
    }
}
