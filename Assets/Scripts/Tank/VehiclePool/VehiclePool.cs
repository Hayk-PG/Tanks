using UnityEngine;

public class VehiclePool : MonoBehaviour
{
    [SerializeField] GameObject[] obj;

    internal class Other
    {
        internal Vector3 _position;

        internal Other(Vector3 position)
        {
            _position = position;
        }
    }


    /// <summary>
    /// 0: HitSound
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pos"></param>
    internal void Pool(int index, Other other)
    {
        if (obj[index].activeInHierarchy) return;

        else
        {
            if(other != null) obj[index].transform.position = other._position;
            obj[index].SetActive(true);
        }
    }
}
