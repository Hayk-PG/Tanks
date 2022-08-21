using System.Collections;
using UnityEngine;

public class CanonPushBack : MonoBehaviour
{
    private BaseShootController _baseShootController;


    private void Awake()
    {
        _baseShootController = Get<BaseShootController>.From(gameObject);
    }

    private void OnEnable()
    {
        _baseShootController.OnShoot += OnShoot;
    }

    private void OnDisable()
    {
        _baseShootController.OnShoot -= OnShoot;
    }

    private void OnShoot()
    {
        StartCoroutine(CanonPushBackCoroutine());
    }

    private IEnumerator CanonPushBackCoroutine()
    {       
        float fixedZ = -0.87f;
        
        while (transform.localPosition.z > fixedZ)
        {
            float updatedZ = transform.localPosition.z;
            transform.localPosition = new Vector3(0, 0, updatedZ -= 10 * Time.deltaTime);

            if (transform.localPosition.z <= fixedZ)
                transform.localPosition = new Vector3(0, 0, fixedZ);

            yield return null;
        }

        yield return null;

        while (transform.localPosition.z < 0)
        {
            float updatedZ = transform.localPosition.z;
            transform.localPosition = new Vector3(0, 0, updatedZ += 4 * Time.deltaTime);

            if (transform.localPosition.z >= 0)
                transform.localPosition = Vector3.zero;

            yield return null;
        }
    }
}
