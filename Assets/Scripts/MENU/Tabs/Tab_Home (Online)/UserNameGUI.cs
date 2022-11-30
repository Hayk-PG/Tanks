using UnityEngine;
using TMPro;

public class UserNameGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtUserName;

    private MyPhotonCallbacks _myPhotonCallbacks;


    private void Awake()
    {
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
    }

    private void OnEnable()
    {
        _myPhotonCallbacks._OnConnectedToMaster += delegate { _txtUserName.text = Data.Manager.Id; };
    }

    private void OnDisable()
    {
        _myPhotonCallbacks._OnConnectedToMaster -= delegate { _txtUserName.text = Data.Manager.Id; };
    }
}
