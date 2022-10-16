using System.Collections;
using UnityEngine;

public class WoodBoxDrop : MonoBehaviour
{
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;
    private bool _isSoundPlayed;


    private void Awake()
    {
        _parachuteWithWoodBoxCollision = Get<ParachuteWithWoodBoxCollision>.From(gameObject);
    }

    private void OnEnable()
    {
        _parachuteWithWoodBoxCollision.OnCollision += OnCollision;
    }

    private void OnDisable()
    {
        _parachuteWithWoodBoxCollision.OnCollision -= OnCollision;
    }

    private void OnCollision(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        if (collisionData._tankController == null && collisionData._bulletController == null && !_isSoundPlayed)
        {
            StartCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        SecondarySoundController.PlaySound(2, 1);
        _isSoundPlayed = true;

        yield return new WaitForSeconds(1);
        _isSoundPlayed = false;
    }
}
