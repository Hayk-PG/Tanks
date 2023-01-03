using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class TilePropsContainer : MonoBehaviour
{
    [SerializeField] private TileProps _tileProps;

    public AAProjectileLauncher AAProjectileLauncher { get; private set; }



    private void OnEnable()
    {
        _tileProps.onAAProjectileLauncherActivity += OnAALauncherActivity;
    }

    private void OnDisable()
    {
        _tileProps.onAAProjectileLauncherActivity -= OnAALauncherActivity;
    }

    private void OnAALauncherActivity(bool isActive, bool? isFirstPlayer)
    {
        if (isActive)
        {
            if (AAProjectileLauncher != null)
            {
                InitAAProjectileLauncher(AAProjectileLauncher, isFirstPlayer);
            }
            else
            {
                LoadAddressable(AddressablesPath.LABEL_AAProjectileLauncher, result =>
                {
                    if (result != null)
                    {
                        GameObject gameObject = Instantiate(result, transform);
                        AAProjectileLauncher = Get<AAProjectileLauncher>.From(gameObject);
                        InitAAProjectileLauncher(AAProjectileLauncher, isFirstPlayer);
                    }
                });
            }
        }
        else
        {
            AAProjectileLauncher?.gameObject.SetActive(false);
            AAProjectileLauncher?.Init(null);
        }
    }

    private void LoadAddressable(string path, Action<GameObject> onResult)
    {
        Addressables.LoadAssetAsync<GameObject>(path).Completed += (asyncOperationHandle) =>
        {
            if (asyncOperationHandle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                onResult?.Invoke(asyncOperationHandle.Result);
            else
                onResult?.Invoke(null);
        };
    }

    private void InitAAProjectileLauncher(AAProjectileLauncher aAProjectileLauncher, bool? isFirstPlayer)
    {
        string tankName = isFirstPlayer.Value == true ? Names.Tank_FirstPlayer : Names.Tank_SecondPlayer;
        TankController ownerTankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.gameObject.name == tankName);
        aAProjectileLauncher.gameObject.SetActive(true);
        aAProjectileLauncher.Init(ownerTankController);
    }
}
