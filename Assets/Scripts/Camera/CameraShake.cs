using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _anim;

    private const string _shake = "shake";



    private void Awake()
    {
        _anim = GetComponentInParent<Animator>();
    }

    public void Shake()
    {
        _anim.SetTrigger(_shake);
    }

}
