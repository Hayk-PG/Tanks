using UnityEngine;
using UnityEngine.Events;

public class TopBarBackButton : MonoBehaviour
{
    [SerializeField] private UnityEvent OnBackButton;


    public void OnClickButton()
    {
        OnBackButton?.Invoke();
    }
}
