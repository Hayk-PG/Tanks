using System;
using System.Collections;
using UnityEngine;

public class WoodBoxDrop : MonoBehaviour
{
    [SerializeField]
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;

    private bool _isSoundPlayed;

    public event Action onCollidedWithGround;




    private void OnEnable() => _parachuteWithWoodBoxCollision.onCollisionEnter += OnCollision;

    private void OnDisable() => _parachuteWithWoodBoxCollision.onCollisionEnter -= OnCollision;

    private void OnCollision(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        bool isCollidedWithGround = collisionData._tankController == null && collisionData._bulletController == null;

        if (isCollidedWithGround)
            OnColliedWithGround();
    }

    private void OnColliedWithGround()
    {
        onCollidedWithGround?.Invoke();

        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        if (!_isSoundPlayed)
        {
            _isSoundPlayed = true;

            SecondarySoundController.PlaySound(2, 1);

            yield return new WaitForSeconds(1);

            _isSoundPlayed = false;
        }
    }
}
