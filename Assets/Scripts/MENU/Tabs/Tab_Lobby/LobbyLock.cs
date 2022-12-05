using UnityEngine;

public class LobbyLock : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private CanvasGroup _btnCanvasGroup;


    private void Awake() => _canvasGroup = Get<CanvasGroup>.From(gameObject);

    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
        GlobalFunctions.CanvasGroupActivity(_btnCanvasGroup, !isActive);
    }
}
