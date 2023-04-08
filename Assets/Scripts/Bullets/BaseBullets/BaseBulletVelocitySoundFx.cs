using UnityEngine;

public class BaseBulletVelocitySoundFx : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private int _clipIndex;

    private bool _isPlayed;



    private void FixedUpdate()
    {
        if(!_isPlayed && !SoundController.IsSoundMuted && _rigidbody.velocity.y <= 0)
        {
            SecondarySoundController.PlaySound(2, _clipIndex);

            _isPlayed = true;
        }
    }
}
