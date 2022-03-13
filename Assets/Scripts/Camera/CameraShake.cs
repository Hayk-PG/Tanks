using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _anim;

    private const string _shake = "shake";
    private const string _bigShake = "bigShake";



    private void Awake()
    {
        _anim = GetComponentInParent<Animator>();
    }

    public void Shake()
    {
        _anim.SetTrigger(_shake);
        CameraBlur.CameraShakeBlur();
    }

    public void BigShake()
    {
        _anim.SetTrigger(_bigShake);
        CameraBlur.CameraShakeBlur();
    }
}
