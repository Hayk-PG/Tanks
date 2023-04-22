using UnityEngine;

public class DashParticleSystem : MonoBehaviour
{
    [SerializeField]
    private Dash _dash;

    private void OnParticleSystemStopped() => _dash.ResetTransform(gameObject);
}
