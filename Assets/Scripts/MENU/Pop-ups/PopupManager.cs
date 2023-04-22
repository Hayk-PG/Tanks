using UnityEngine;


public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    private OfflineGameResultPopup _offlineGameResultPopup;




    private void Awake()
    {
        SetInstance();

        _offlineGameResultPopup = new OfflineGameResultPopup();
    }

    private void SetInstance()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PrepareGameResultPopup()
    {
        _offlineGameResultPopup.PrepareGameResultPopup();
    }
}
