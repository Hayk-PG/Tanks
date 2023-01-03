using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MyAddressable
{
    public static AsyncOperationHandle<UnityEngine.GameObject> _asyncOperationHandle;

    public static void LoadAssetAsync(string path, bool release, System.Action<UnityEngine.GameObject> onSuccess, System.Action onFailed)
    {
        _asyncOperationHandle = Addressables.LoadAssetAsync<UnityEngine.GameObject>(path);
        _asyncOperationHandle.Completed += (result) => 
        { 
            if(result.Status == AsyncOperationStatus.Succeeded)
            {
                onSuccess?.Invoke(result.Result);

                if (release)
                    Addressables.Release(_asyncOperationHandle);
            }
            else
            {
                onFailed?.Invoke();
            }
        };
    }
}
