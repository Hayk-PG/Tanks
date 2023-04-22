using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] 
    private ParticleSystem _dashParticle;

    private BaseShootController _baseShootController;
    private AbilityDodge _abilityDodge;




    private void Awake()
    {
        _baseShootController = Get<BaseShootController>.From(transform.parent.gameObject);

        _abilityDodge = Get<AbilityDodge>.From(transform.parent.gameObject);
    }

    private void OnEnable()
    {
        _baseShootController.OnDash += OnDash;

        if (_abilityDodge != null)
            _abilityDodge.onDodge += PlayParticles;
    }

    private void OnDash(float direction)
    {
        if (direction != 0)
            PlayParticles();
    }

    public void PlayParticles()
    {
        _dashParticle.transform.SetParent(null);
        _dashParticle.Play();
    }

    public void ResetTransform(GameObject gameObject)
    {
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localEulerAngles = new Vector3(0, -180, 0);
    }
}
