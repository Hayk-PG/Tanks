using System.Collections;
using UnityEngine;

public class GameSceneObjectsReferences : MonoBehaviour
{
    public static GameSceneObjectsReferences Instance { get; private set; }

    [Header("GameManager")]
    [SerializeField] private TurnController _turnController;
    [SerializeField] private WindSystemController _windSystemController;
    [SerializeField] private GameManagerBulletSerializer _gameManagerBulletSerializer;
    [SerializeField] private PhotonNetworkAALauncher _photonNetworkAALauncher;

    //GameManager
    public static TurnController TurnController => Instance._turnController;
    public static WindSystemController WindSystemController => Instance._windSystemController;
    public static GameManagerBulletSerializer GameManagerBulletSerializer => Instance._gameManagerBulletSerializer;
    public static PhotonNetworkAALauncher PhotonNetworkAALauncher => Instance._photonNetworkAALauncher;

    //LavaSplash
    public static LavaSplash LavaSplash { get; private set; }




    private void Awake()
    {
        Instance = this;

        LavaSplash = FindObjectOfType<LavaSplash>();
    }

    private void Start()
    {
        StartCoroutine(FindLavaSplash());
    }

    private IEnumerator FindLavaSplash()
    {
        yield return new WaitUntil(() => FindObjectOfType<LavaSplash>() != null);
        LavaSplash = FindObjectOfType<LavaSplash>();
    }

}

