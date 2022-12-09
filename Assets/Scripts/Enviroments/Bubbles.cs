using System.Collections;
using UnityEngine;

public class Bubbles : MonoBehaviour
{
    [SerializeField] private ParticleBubbles[] _customParticles;
    private GameManager _gameManager;



    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(SpawnParticles());
    }

    private IEnumerator SpawnParticles()
    {
        while (!_gameManager.IsGameEnded)
        {
            yield return new WaitForSeconds(Random.Range(0, 10));

            for (int i = 0; i < _customParticles.Length; i++)
            {
                int randomParticle = Random.Range(0, _customParticles.Length);
                int randomSeconds = Random.Range(0, 2);

                yield return new WaitForSeconds(randomSeconds);

                if (!_customParticles[randomParticle].gameObject.activeInHierarchy)
                {
                    _customParticles[randomParticle].gameObject.SetActive(true);
                    _customParticles[randomParticle].SetLifeTime(5);
                }
            }
        }
    }
}
