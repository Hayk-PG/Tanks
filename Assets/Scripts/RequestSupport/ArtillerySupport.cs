using System.Collections;
using UnityEngine;

public class ArtillerySupport : MonoBehaviour
{
    [SerializeField] private ArtilleryBulletController _artillerPrefab;
    [SerializeField] private float _deviation;
    private MainCameraController _mainCameraController;



    private void Awake()
    {
        _mainCameraController = FindObjectOfType<MainCameraController>();
    }

    public void Call(Vector3 coordinate, IScore ownerScore, PlayerTurn ownerTurn)
    {
        StartCoroutine(InstantiateArtilleryShells(coordinate, 5, ownerScore, ownerTurn));
    }

    private IEnumerator InstantiateArtilleryShells(Vector3 coordinate, int shellsCount, IScore ownerScore, PlayerTurn ownerTurn)
    {        
        yield return new WaitForSeconds(1);

        for (int i = 0; i < shellsCount; i++)
        {
            float randomDeviation = Random.Range(-_deviation, _deviation);
            ArtilleryBulletController artillery = Instantiate(_artillerPrefab, new Vector3(coordinate.x + randomDeviation, coordinate.y + 10, coordinate.z), Quaternion.identity);
            artillery.OwnerScore = ownerScore;
            artillery.IsLastShellOfBarrage = i < shellsCount - 1 ? false : true;
            _mainCameraController.CameraOffset(ownerTurn, artillery.transform, null, null);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
