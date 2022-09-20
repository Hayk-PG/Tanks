using System.Collections;
using UnityEngine;

public class TileParticles : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] _tileParticles;
    private GameObject _bottomTile;
    private TileParticles _bottomTileParticles;


    public void DirectActivationOfTileParticles(int index)
    {
        if (_tileParticles != null && index < _tileParticles.Length)
            _tileParticles[index].SetActive(true);
    }

    public void TileParticlesActivity(int index, TilesData tilesData, Vector3 bottomTile, bool isActive)
    {
        if (_tileParticles != null && index < _tileParticles.Length)
        {
            transform.parent = null;
            transform.position = bottomTile;
            _tileParticles[index].SetActive(isActive);
            StartCoroutine(SetParentCoroutine(index, tilesData, bottomTile));
        }           
    }

    private IEnumerator SetParentCoroutine(int particlesIndex, TilesData tilesData, Vector3 bottomTile)
    {
        yield return new WaitForSeconds(1);

        if (tilesData.TilesDict.ContainsKey(bottomTile))
        {
            _bottomTile = tilesData.TilesDict[bottomTile];
            _bottomTileParticles = Get<TileParticles>.FromChild(_bottomTile.gameObject);
            _bottomTileParticles?.DirectActivationOfTileParticles(particlesIndex);         
        }

        gameObject.SetActive(false);
    }
}
