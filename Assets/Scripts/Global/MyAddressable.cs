using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MyAddressable
{
    public static AsyncOperationHandle<UnityEngine.GameObject>[] _asyncOperationHandle = new AsyncOperationHandle<UnityEngine.GameObject>[100];

    public static void LoadAssetAsync(string path, int operationIndex, bool release, System.Action<UnityEngine.GameObject> onSuccess, System.Action onFailed)
    {
        //_asyncOperationHandle[operationIndex] = Addressables.LoadAssetAsync<UnityEngine.GameObject>(path);
        //_asyncOperationHandle[operationIndex].Completed += (result) => 
        //{ 
        //    if(result.Status == AsyncOperationStatus.Succeeded)
        //    {
        //        onSuccess?.Invoke(result.Result);

        //        if (release)
        //            Addressables.Release(_asyncOperationHandle[operationIndex]);
        //    }
        //    else
        //    {
        //        onFailed?.Invoke();
        //    }
        //};
    }

    public static void InstantiateAsync(string key, System.Action<UnityEngine.GameObject> onResult)
    {
        Addressables.InstantiateAsync(key).Completed += (addressable) =>
        {
            onResult?.Invoke(addressable.Result);
        };
    }

    public static void InstantiateAsync(string key, UnityEngine.Transform transform, System.Action<UnityEngine.GameObject> onResult)
    {
        Addressables.InstantiateAsync(key, transform).Completed += (addressable) =>
        {
            onResult?.Invoke(addressable.Result);
        };
    }

    public static void ReleaseInstance(UnityEngine.GameObject gameObject) => Addressables.ReleaseInstance(gameObject);
}
