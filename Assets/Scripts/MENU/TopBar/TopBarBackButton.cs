using UnityEngine;
using UnityEngine.Events;

public class TopBarBackButton : MonoBehaviour
{
    [SerializeField] protected UnityEvent OnButtonEvent;


    public void OnClickButton()
    {
        OnButtonEvent?.Invoke();
    }
}
