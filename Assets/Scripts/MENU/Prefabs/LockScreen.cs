using UnityEngine;

public class LockScreen : MonoBehaviour,IReset
{
    [SerializeField] private GameObject _screenObj;


    public void Lock()
    {
        _screenObj.SetActive(true);
    }

    public void SetDefault()
    {
        _screenObj.SetActive(false);
    }
}
