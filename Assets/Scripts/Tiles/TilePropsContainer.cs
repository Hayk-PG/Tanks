using UnityEngine;


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
                MyAddressable.LoadAssetAsync((string)AddressablesPath.AAProjectileLauncher[0, 0], (int)AddressablesPath.AAProjectileLauncher[0, 1], true, delegate (GameObject gameObject)
                 {
                     AAProjectileLauncher = Get<AAProjectileLauncher>.From(Instantiate(gameObject, transform));
                     InitAAProjectileLauncher(AAProjectileLauncher, isFirstPlayer);
                 },
                null);
            }
        }
        else
        {
            AAProjectileLauncher?.gameObject.SetActive(false);
            AAProjectileLauncher?.Init(null);
        }
    }

    private void InitAAProjectileLauncher(AAProjectileLauncher aAProjectileLauncher, bool? isFirstPlayer)
    {
        string tankName = isFirstPlayer.Value == true ? Names.Tank_FirstPlayer : Names.Tank_SecondPlayer;
        TankController ownerTankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.gameObject.name == tankName);
        aAProjectileLauncher.gameObject.SetActive(true);
        aAProjectileLauncher.Init(ownerTankController);
    }
}
