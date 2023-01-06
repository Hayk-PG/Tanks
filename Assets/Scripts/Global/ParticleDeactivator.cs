using UnityEngine;
using UnityEngine.AddressableAssets;

//ADDRESSABLE
public class ParticleDeactivator : MonoBehaviour
{
    private enum StopBehaviour { None, Disable, Destroy}
    [SerializeField] StopBehaviour _stopBehaviour;
    [SerializeField] private bool _releaseInstance;


    private void OnParticleSystemStopped()
    {
        if (_releaseInstance)
            Addressables.ReleaseInstance(gameObject);

        OnStopBehaviour(_stopBehaviour);
    }

    private void OnStopBehaviour(StopBehaviour stopBehaviour)
    {
        switch (stopBehaviour)
        {
            case StopBehaviour.None: return;
            case StopBehaviour.Disable: gameObject.SetActive(false);break;
            case StopBehaviour.Destroy: Destroy(gameObject);break;
        }
    }
}
