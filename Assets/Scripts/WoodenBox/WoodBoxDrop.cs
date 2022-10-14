using System.Collections;
using UnityEngine;

public class WoodBoxDrop : MonoBehaviour
{
    private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;
    private bool _isSoundPlayed;


    private void Awake()
    {
        _parachuteWithWoodBoxController = Get<ParachuteWithWoodBoxController>.From(gameObject);
    }

    private void OnEnable()
    {
        _parachuteWithWoodBoxController.OnCollision += OnCollision;
    }

    private void OnDisable()
    {
        _parachuteWithWoodBoxController.OnCollision -= OnCollision;
    }

    private void OnCollision(WoodBoxCollisionData woodBoxCollisionData)
    {
        if (!woodBoxCollisionData._isCollidedWithTank && !woodBoxCollisionData._isCollidedWithBullet && !_isSoundPlayed)
            StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        SecondarySoundController.PlaySound(2, 1);
        _isSoundPlayed = true;

        yield return new WaitForSeconds(1);
        _isSoundPlayed = false;
    }
}
