using UnityEngine;

public class Tile : MonoBehaviour, IDamage
{
    Collider _collider;
    ChangeTiles _changeTiles;

    [SerializeField] GameObject _explosion;


    void Awake()
    {
        _collider = GetComponent<Collider>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
    }

    public void Damage(float damage)
    {
        _collider.isTrigger = true;
        _explosion.SetActive(true);
        _explosion.transform.parent = null;
        Destroy(gameObject);
        UpdateTiles();
    }

    void UpdateTiles()
    {
        _changeTiles.UpdateTiles();
    }
}
