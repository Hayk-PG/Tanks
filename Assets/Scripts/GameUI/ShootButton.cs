using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public delegate void OnButton(bool isTrue);

    public event OnButton OnPointer;
    public event OnButton OnClick;

    [SerializeField] bool p;


    public void OnButtonClicked()
    {
        OnClick?.Invoke(true);
    }

    public void OnPointerDown()
    {
        OnPointer?.Invoke(true);

        p = true;
    }

    public void OnPointerUp()
    {
        OnPointer?.Invoke(false);

        p = false;
    }
}
