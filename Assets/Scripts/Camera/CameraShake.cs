using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Animator _anim;
    private CameraBlur _cameraBlur;

    private const string _shake = "shake";
    private const string _bigShake = "bigShake";



    private void Awake()
    {
        _anim = GetComponentInParent<Animator>();
        _cameraBlur = FindObjectOfType<CameraBlur>();
    }

    public void Shake()
    {
        _anim.SetTrigger(_shake);
        _cameraBlur.CameraShakeBlur();
    }

    public void BigShake()
    {
        _anim.SetTrigger(_bigShake);
        _cameraBlur.CameraShakeBlur();
    }
}
