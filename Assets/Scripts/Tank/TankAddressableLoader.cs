using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TankAddressableLoader : MonoBehaviour
{
    [SerializeField] private AssetReference[] _assetReferencesBody;
    [SerializeField] private AssetReference[] _assetReferenceTurret;
    [SerializeField] private AssetReference[] _assetReferenceCanon;
    [SerializeField] private AssetReference[] _assetReferenceWheels;

    [SerializeField] private Transform _transformBody;
    [SerializeField] private Transform _transformTurret;
    [SerializeField] private Transform _transformCanon;
    [SerializeField] private Transform[] _transformWheels;



    private void Start() => StartCoroutine(RunInstantiateAsyncCoroutines());

    private IEnumerator RunInstantiateAsyncCoroutines()
    {
        yield return StartCoroutine(InstantiateAsync(_assetReferencesBody, _transformBody));
        yield return StartCoroutine(InstantiateAsync(_assetReferenceTurret, _transformTurret));
        yield return StartCoroutine(InstantiateAsync(_assetReferenceCanon, _transformCanon));
        yield return StartCoroutine(InstantiateAsync(_assetReferenceWheels, _transformWheels));
    }

    private IEnumerator InstantiateAsync(AssetReference[] assetReferences, Transform transform)
    {
        int index = 0;
        bool isInstantiated = false;

        while (index < assetReferences.Length)
        {
            foreach (var assetReference in assetReferences)
            {
                isInstantiated = false;
                assetReference.InstantiateAsync(transform).Completed += delegate (AsyncOperationHandle<GameObject> asyncOperationHandle)
                {
                    isInstantiated = true;
                    index++;
                };
                yield return new WaitUntil(() => isInstantiated == true);
            }
        }
        yield return null;
    }

    private IEnumerator InstantiateAsync(AssetReference[] assetReferences, Transform[] transform)
    {
        int index = 0;
        bool isInstantiated = false;

        while (index < assetReferences.Length)
        {
            foreach (var assetReference in assetReferences)
            {
                isInstantiated = false;
                assetReference.InstantiateAsync(transform[index]).Completed += delegate (AsyncOperationHandle<GameObject> asyncOperationHandle)
                {
                    isInstantiated = true;
                    index++;
                };
                yield return new WaitUntil(() => isInstantiated == true);
            }
        }
        yield return null;
    }
}
