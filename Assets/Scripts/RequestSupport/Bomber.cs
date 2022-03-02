using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField]
    private Transform _propeller, _bombSpwnPoint;

    [SerializeField]
    private BombController _bombPrefab;


    private void FixedUpdate()
    {
        transform.Translate(-transform.forward * 2 * Time.fixedDeltaTime);
        _propeller.Rotate(Vector3.right, -1000 * Time.deltaTime);
    }

    public void DropBomb(IScore _iScore)
    {
        BombController _bomb = Instantiate(_bombPrefab, _bombSpwnPoint.position, Quaternion.identity);
        _bomb._iScore = _iScore;
    }
}
