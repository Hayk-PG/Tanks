using System.Collections;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private ShootController _shootController;

    [SerializeField]
    private float _elapsedTime;
    private float _force = 1;



    private void Awake()
    {
        _shootController = Get<ShootController>.From(gameObject);

        _rigidbody = Get<Rigidbody>.From(_shootController.gameObject);
    }

    private void OnEnable() => _shootController.OnShoot += OnShoot;

    private void OnDisable() => _shootController.OnShoot -= OnShoot;

    private void OnShoot() => StartCoroutine(GenerateRecoil());

    private IEnumerator GenerateRecoil()
    {
        yield return StartCoroutine(PushBackwards(_elapsedTime, 0));
        yield return StartCoroutine(PushForward(_elapsedTime, 0));
    }

    private IEnumerator PushBackwards(float elapsedTime, float delay)
    {
        while (delay < elapsedTime)
        {
            delay += Time.deltaTime;

            _rigidbody.position += -transform.forward * (_force - delay) * Time.fixedDeltaTime;

            yield return null;
        }
    }

    private IEnumerator PushForward(float elapsedTime, float delay)
    {
        while (delay < elapsedTime)
        {
            delay += Time.deltaTime;

            _rigidbody.position += transform.forward * (_force - delay) / 2 * Time.fixedDeltaTime;

            yield return null;
        }
    }
}
