using UnityEngine;

public class LobbyLock : MonoBehaviour
{
    private CanvasGroup _canvasGroup;


    private void Awake() => _canvasGroup = Get<CanvasGroup>.From(gameObject);

    public void SetActivity(bool isActive) => GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
}
