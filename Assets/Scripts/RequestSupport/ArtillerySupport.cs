using System.Collections;
using UnityEngine;

public class ArtillerySupport : MonoBehaviour
{
    [SerializeField] 
    private BaseBulletController _shellPrefab;
 


    public void Call(object[] data)
    {
        StartCoroutine(InstantiateShells((PlayerTurn)data[0], (IScore)data[1], (float[])data[2], (Vector3)data[3]));
    }

    private IEnumerator InstantiateShells(PlayerTurn ownerTurn, IScore ownerScore, float[] shellSpreadValues, Vector3 target)
    {
        yield return null;

        for (int i = 0; i < shellSpreadValues.Length; i++)
        {
            BaseBulletController shell = Instantiate(_shellPrefab, new Vector3(target.x + shellSpreadValues[i], target.y + 10, 0), Quaternion.identity);

            shell.OwnerScore = ownerScore;
            shell.IsLastShellOfBarrage = i < shellSpreadValues.Length - 1 ? false : true;

            GameSceneObjectsReferences.MainCameraController.CameraOffset(null, shell.RigidBody, null, null);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
