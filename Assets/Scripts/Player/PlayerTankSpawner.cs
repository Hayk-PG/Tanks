using Photon.Pun;
using UnityEngine;

public class PlayerTankSpawner : MonoBehaviourPun
{
    [SerializeField] protected TankController[] _tanks;

    private Transform _spawnPointForPlayer1;
    private Transform _spawnPointForPlayer2;

    private BasePlayerTankController<TankController> _playerTankController;


    private void Awake()
    {
        _spawnPointForPlayer1 = GameObject.Find("1PlayerStartPoint").transform;
        _spawnPointForPlayer2 = GameObject.Find("2PlayerStartPoint").transform;

        _playerTankController = Get<BasePlayerTankController<TankController>>.From(gameObject);
    }

    public void SpawnTanks(int tankIndex, int spawnPointIndex)
    {
        Vector3 position = spawnPointIndex == 0 ? _spawnPointForPlayer1.position : _spawnPointForPlayer2.position;
        Quaternion rotation = spawnPointIndex == 0 ? _spawnPointForPlayer1.rotation : _spawnPointForPlayer2.rotation;
        TankController tank = Instantiate(_tanks[tankIndex], position, rotation);
        tank.name = spawnPointIndex == 0 ? Names.Tank_FirstPlayer : Names.Tank_SecondPlayer;

        _playerTankController.GetControl(tank);
    }
}
