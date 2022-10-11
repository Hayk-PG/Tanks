using UnityEngine;

public class PlatformSerializer : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbodyPlatformHorizontal, _rigidbodyPlatformVertical;

    public Rigidbody RigidbodyPlatformHor
    {
        get => _rigidbodyPlatformHorizontal;
        set => _rigidbodyPlatformHorizontal = value;
    } 
       
    public Rigidbody RigidbodyPlatformVert
    {
        get => _rigidbodyPlatformVertical;
        set => _rigidbodyPlatformVertical = value;
    }
}
