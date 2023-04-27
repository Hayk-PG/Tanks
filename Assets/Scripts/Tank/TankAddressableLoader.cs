using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;


//ADDRESSABLE
public class TankAddressableLoader : MonoBehaviour
{
    [Serializable] private class UniqueAssets
    {
        public AssetReference assetReference;
        public Transform parent;
        public Vector3 localPosition;
        public Vector3 localEulerAngles;
        public Vector3 localScale;

        public bool adjustRotation;
        public bool adjustScale;
    }


    [Header("MESHES")]
    [SerializeField] [Space]
    private AssetReference[] _assetReferencesBody;

    [SerializeField] [Space]
    private AssetReference[] _assetReferenceTurret;

    [SerializeField] [Space]
    private AssetReference[] _assetReferenceCanon;

    [SerializeField] [Space]
    private AssetReference[] _assetReferenceWheels;

    [Header("PERIPHERALS")] 
    [SerializeField] [Space]
    private AssetReference[] assetReferencePeripherals;

    [Header("UNIQUE ASSETS")]
    [SerializeField] [Space]
    private UniqueAssets[] _uniqueAssets;

    [Header("PARENTS")]
    [SerializeField] [Space]
    private Transform _transformBody;

    [SerializeField] [Space]
    private Transform _transformTurret;

    [SerializeField] [Space]
    private Transform _transformCanon;

    [SerializeField] [Space]
    private Transform[] _transformWheels;



    private void Start() => StartCoroutine(RunInstantiateAsyncCoroutines());

    private IEnumerator RunInstantiateAsyncCoroutines()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(InstantiateAsync(_assetReferencesBody, _transformBody));
        yield return StartCoroutine(InstantiateAsync(_assetReferenceTurret, _transformTurret));
        yield return StartCoroutine(InstantiateAsync(_assetReferenceCanon, _transformCanon));
        yield return StartCoroutine(InstantiateAsync(_assetReferenceWheels, _transformWheels));
        yield return StartCoroutine(InstantiateAsync(assetReferencePeripherals, transform.parent));
        yield return StartCoroutine(InstantiateUniqueAssets());
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

    private IEnumerator InstantiateUniqueAssets()
    {
        foreach (var uniqueAsset in _uniqueAssets)
        {
            uniqueAsset.assetReference.InstantiateAsync(uniqueAsset.parent).Completed += asset => 
            {
                asset.Result.transform.localPosition = uniqueAsset.localPosition;

                if (uniqueAsset.adjustRotation)
                    asset.Result.transform.localEulerAngles = uniqueAsset.localEulerAngles;

                if(uniqueAsset.adjustScale)
                    asset.Result.transform.localScale = uniqueAsset.localScale;
            };
        }

        yield return null;
    }
}
