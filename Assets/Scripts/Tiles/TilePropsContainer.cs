using UnityEngine;
using UnityEngine.AddressableAssets;


public class TilePropsContainer : MonoBehaviour
{
    [SerializeField] private TileProps _tileProps;

    public AALauncher AALauncher { get; private set; }



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
            if (AALauncher != null)
            {
                InitAALauncher(AALauncher, isFirstPlayer);
            }
            else
            {
                MyAddressable.InstantiateAsync((string)AddressablesPath.AALauncher[0, 0], transform, gameobject => 
                {
                    AALauncher = Get<AALauncher>.From(gameobject);
                    InitAALauncher(AALauncher, isFirstPlayer);
                });
            }
        }
        else
        {
            AALauncher?.gameObject.SetActive(false);
            AALauncher?.Init(null);
        }
    }

    private void InitAALauncher(AALauncher aALauncher, bool? isFirstPlayer)
    {
        string tankName = isFirstPlayer.Value == true ? Names.Tank_FirstPlayer : Names.Tank_SecondPlayer;
        TankController ownerTankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.gameObject.name == tankName);
        aALauncher.gameObject.SetActive(true);
        aALauncher.Init(ownerTankController);
    }
}
