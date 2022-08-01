using System.Collections;
using UnityEngine;

public class ArtillerySupport : MonoBehaviour
{
    [SerializeField] private ArtilleryBulletController _artillerPrefab;
    [SerializeField] private float _deviation;


    public void Call(Vector3 coordinate, IScore iScore)
    {
        StartCoroutine(InstantiateArtilleryShells(coordinate, 5, iScore));
    }

    private IEnumerator InstantiateArtilleryShells(Vector3 coordinate, int shellsCount, IScore iScore)
    {        
        yield return new WaitForSeconds(1);

        for (int i = 0; i < shellsCount; i++)
        {
            float randomDeviation = Random.Range(-_deviation, _deviation);
            ArtilleryBulletController artillery = Instantiate(_artillerPrefab, new Vector3(coordinate.x + randomDeviation, coordinate.y + 10, coordinate.z), Quaternion.identity);
            artillery.OwnerScore = iScore;
            artillery.IsLastShellOfBarrage = i < shellsCount - 1 ? false : true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
