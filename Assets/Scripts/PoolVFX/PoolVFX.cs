using UnityEngine;

public class PoolVFX : MonoBehaviour
{
    /// <summary>
    /// 0: GroundSlamRed
    /// </summary>
    [SerializeField]
    private GameObject[] _vfx;

    private CameraShake _cameraShake;



    private void Awake()
    {
        _cameraShake = FindObjectOfType<CameraShake>();
    }

    public void Pool(int index, Vector3 position, bool shakeCamera)
    {
        if (_vfx[index].activeInHierarchy) return;

        else
        {
            _vfx[index].transform.position = position;
            _vfx[index].SetActive(true);
            if (shakeCamera) _cameraShake.Shake();
        }
    }
}
