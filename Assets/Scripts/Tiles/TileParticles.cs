using UnityEngine;

public class TileParticles : MonoBehaviour
{
    [SerializeField] private GameObject[] _tileParticles;



    public void TileParticlesActivity(int index, bool isActive)
    {
        if (_tileParticles != null && index < _tileParticles.Length)
        {
            _tileParticles[index].transform.SetParent(null);
            _tileParticles[index].SetActive(isActive);
        }           
    }
}
