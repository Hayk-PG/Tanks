using UnityEngine;

public abstract class Tab_BaseSignUp<T> : Tab_Base<T> where T: MonoBehaviour
{
    [SerializeField] protected CustomInputField[] _customInputFields;
    protected Data _data;
    protected Tab_SignUp _tabSignUp;
    protected Tab_SignIn _tabSignIn;

    protected virtual CustomInputField CustomInputFieldEmail { get; }
    protected virtual CustomInputField CustomInputFieldID { get; }
    protected virtual CustomInputField CustomInputFieldPassword { get; }


    protected override void Awake()
    {
        base.Awake();
        _data = FindObjectOfType<Data>();
        _tabSignUp = FindObjectOfType<Tab_SignUp>();
        _tabSignIn = FindObjectOfType<Tab_SignIn>();
    }

    protected virtual void Update()
    {
        SetInteractability();
    }

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

    protected abstract void Confirm();

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
        _btnForward.IsInteractable = CustomInputFieldID.Text.Length > 6 && CustomInputFieldPassword.Text.Length > 4 ? true : false;
    }
}
