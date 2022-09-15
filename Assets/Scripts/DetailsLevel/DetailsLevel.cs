using UnityEngine;

[ExecuteInEditMode]
public class DetailsLevel : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private int _count;
    [SerializeField] private float _x, _y;

    public void Place()
    {
        float x = 0;
        float y = 0;
        Vector3 position = _meshFilter.gameObject.transform.position;

        for (int i = 0; i < _count; i++)
        {
            x += _x;
            y += _y;
            GameObject detail = Instantiate(_meshFilter.gameObject, position + new Vector3(x, y, 0), _meshFilter.gameObject.transform.rotation, transform);
        }
    }
}
 