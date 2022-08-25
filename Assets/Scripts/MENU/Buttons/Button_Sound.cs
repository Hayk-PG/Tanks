using UnityEngine;

public class Button_Sound : MonoBehaviour
{
    [SerializeField] private int _listIndex;
    [SerializeField] private int _clipIndex;


    public void OnButtonSound()
    {
        UISoundController.PlaySound(_listIndex, _clipIndex);
    }
}
