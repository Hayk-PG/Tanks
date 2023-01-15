using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private GameObject _dashParticle;
    private BaseShootController _baseShootController;


    private void Awake() => _baseShootController = Get<BaseShootController>.From(transform.parent.gameObject);

    private void OnEnable() => _baseShootController.OnDash += OnDash;

    private void OnDisable() => _baseShootController.OnDash -= OnDash;

    private void DashParticlesActitivity(bool isActive)
    {
        if (isActive)
        {
            _dashParticle.transform.SetParent(null);
            _dashParticle.SetActive(true);
        }
        else
        {
            _dashParticle.transform.SetParent(transform);
            _dashParticle.transform.localPosition = Vector3.zero;
            _dashParticle.SetActive(false);
        }
    }

    private void OnDash(float direction)
    {
        DashParticlesActitivity(false);

        if (direction != 0)
            DashParticlesActitivity(true);
    }
}
