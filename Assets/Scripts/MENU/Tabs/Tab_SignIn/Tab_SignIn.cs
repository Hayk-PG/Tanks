using System;
using UnityEngine;

public class Tab_SignIn : Tab_BaseSignUp
{
    [SerializeField] [Space]
    private Btn _btnSignUp;

    private CustomToggle _customToggleAutoSignIn;

    protected override CustomInputField CustomInputFieldID => _customInputFields[0];
    protected override CustomInputField CustomInputFieldPassword => _customInputFields[1];
    public bool IsAutoSignInChecked { get => _customToggleAutoSignIn.IsOn; set => _customToggleAutoSignIn.IsOn = value; }

    public event Action onOpenTabSignUp;



    protected override void Awake()
    {
        base.Awake();

        _customToggleAutoSignIn = Get<CustomToggle>.FromChild(gameObject);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        MenuTabs.Tab_SignUp.onOpenTabSignIn += OpenTab;

        _btnSignUp.onSelect += delegate { onOpenTabSignUp?.Invoke(); };
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        MenuTabs.Tab_SignUp.onOpenTabSignIn -= OpenTab;

        _btnSignUp.onSelect -= delegate { onOpenTabSignUp?.Invoke(); };
    }

    protected override void Authenticate()
    {
        if (Data.Manager.IsAutoSignInChecked)
        {
            CustomInputFieldID.Text = Data.Manager.Id;

            CustomInputFieldPassword.Text = Data.Manager.Password;

            IsAutoSignInChecked = true;

            Confirm();
        }
    }

    protected override void Confirm()
    {
        base.Confirm();

        User.Login(CustomInputFieldID.Text, CustomInputFieldPassword.Text, result =>
        {
            if (_elapsedTime > _waitTime)
            {
                ResetTab();

                return;
            }

            if (result != null)
            {
                DeleteOrEnableAutoSignInData();

                SaveUserCredentials(NewData(CustomInputFieldID.Text, CustomInputFieldPassword.Text));

                CacheUserIds(result.PlayFabId, result.EntityToken.Entity.Id, result.EntityToken.Entity.Type);

                CacheUserItemsData(result.PlayFabId);

                CacheUserStatisticsData(result.PlayFabId);

                SendUserCredentialsToTabHomeOnline(CustomInputFieldID.Text, result.PlayFabId);

                ResetPlayfabCallbackDelayCounter();
            }
            else
            {
                ResetTab();

                OnAuthenticationFailed();
            }
        });
    }

    private void DeleteOrEnableAutoSignInData()
    {
        if (IsAutoSignInChecked)
            Data.Manager.SetData(new Data.NewData { AutoSignIn = 1 });
        else
            Data.Manager.DeleteData(Keys.AutoSignIn);
    }

    protected override void SaveUserCredentials(Data.NewData newData)
    {
        if (IsAutoSignInChecked)
            base.SaveUserCredentials(newData);
    }

    protected override void CacheUserItemsData(string playfabId)
    {
        User.GetItems(playfabId, items => 
        {
            Data.Manager.Coins = items[0];
            Data.Manager.Masters = items[1];
            Data.Manager.Strengths = items[2];
        });
    }

    protected override void CacheUserStatisticsData(string playfabId) => User.GetStats(playfabId, result => { Data.Manager.Statistics = result; });

    public override void OnOperationFailed()
    {
        print("Failed to sign in! " + OperationHandler);

        if (OperationHandler == This)
        {
            IsAutoSignInChecked = false;

            base.OpenTab();
        }
        else
        {
            OperationHandler.OnOperationFailed();
        }
    }
}
