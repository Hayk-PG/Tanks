using System.Collections;
using UnityEngine;

public class WoodBoxDrop : MonoBehaviour
{
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;
    [SerializeField] private GameObject _particles;

    private bool _isSoundPlayed;
    private bool _isParticlesActivated;


    private void Awake()
    {
        _parachuteWithWoodBoxCollision = Get<ParachuteWithWoodBoxCollision>.From(gameObject);
    }

    private void OnEnable()
    {
        _parachuteWithWoodBoxCollision.onCollisionEnter += OnCollision;
    }

    private void OnDisable()
    {
        _parachuteWithWoodBoxCollision.onCollisionEnter -= OnCollision;
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
        ActivateParticles();
        _isSoundPlayed = true;

        yield return new WaitForSeconds(1);
        _isSoundPlayed = false;
    }

    private void ActivateParticles()
    {
        if (!_isParticlesActivated && _particles != null)
        {
            _particles.gameObject.SetActive(true);
            _particles.transform.SetParent(null);
        }
    }
}
