using UnityEngine;

public class ParticleBubbles : MonoBehaviour
{
    public void SetLifeTime(float lifeTime)
    {
        Invoke("Disable", lifeTime);
    }

    private void Disable() => gameObject.SetActive(false);
}
