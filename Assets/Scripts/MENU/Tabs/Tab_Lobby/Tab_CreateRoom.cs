using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_CreateRoom : MonoBehaviour
{
    public CanvasGroup CanvasGroup { get; private set; }

    [SerializeField] private InputField _inputFieldName;
    [SerializeField] private InputField _inputFieldPassword;
    [SerializeField] private Toggle _toggleSetPassword;
    [SerializeField] private Button _createButton;

    [SerializeField] private GameObject _tab_Password;

    private string Name
    {
        get => _inputFieldName.text;
        set => _inputFieldName.text = value;
    }
    private string Password
    {
        get => _inputFieldPassword.text;
        set => _inputFieldPassword.text = value;
    }
    private bool IsPasswordSet
    {
        get => _toggleSetPassword.isOn;
        set => _toggleSetPassword.isOn = value;
    }
    public Action<string, string, bool> OnOpenTab_SelectMap { get; set; }


    private void Awake()
    {
        CanvasGroup = Get<CanvasGroup>.From(gameObject);
    }

    private void Update()
    {
        CreateButtonInteractability();
    }

    private void CreateButtonInteractability()
    {
        _createButton.interactable = Name.Length > 0 && IsPasswordSet && Password.Length > 0 ? true :
                                     Name.Length > 0 && !IsPasswordSet ? true : false;
    }

    public void OnClickCreate()
    {      
        OnOpenTab_SelectMap?.Invoke(Name, Password, IsPasswordSet);
    }

    public void OnToggleValueChanged()
    {
        if (IsPasswordSet)
        {
            _tab_Password.SetActive(true);
        }
        else
        {
            _tab_Password.SetActive(false);
        }

        Password = "";
    }
}
