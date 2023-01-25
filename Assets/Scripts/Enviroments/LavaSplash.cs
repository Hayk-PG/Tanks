using UnityEngine;

public class LavaSplash : MonoBehaviour
{
    [SerializeField] private ParticleBubbles[] _lavaSplashes;


    public void ActivateSmallSplash(Vector3 position)
    {
        if (!IsActive(_lavaSplashes[0].gameObject))
        {
            Activate(position, _lavaSplashes[0]);
            SecondarySoundController.PlaySound(5, 0);
        }
    }

    public void ActivateLargeSplash(Vector3 position)
    {
        if (!IsActive(_lavaSplashes[1].gameObject))
        {
            Activate(position, _lavaSplashes[1]);
            SecondarySoundController.PlaySound(5, 1);
        }
    }

    private bool IsActive(GameObject gameObject)
    {
        return gameObject.activeInHierarchy;
    }

    private void Activate(Vector3 position, ParticleBubbles particleBubbles)
    {
        particleBubbles.transform.position = position;
        particleBubbles.gameObject.SetActive(true);
        particleBubbles.SetLifeTime(2);
    }
}
