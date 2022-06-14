using UnityEngine;

public class TileProps : MonoBehaviour
{
    [SerializeField] private Sandbags _sandBags;
    [SerializeField] private bool _isSandBagsActive;

    public Sandbags Sandbags => _sandBags;


    private void Update()
    {
        if (_isSandBagsActive)
        {
            Sandbags.gameObject.SetActive(true);
            Sandbags.SandbagsDirection(true);
            _isSandBagsActive = false;
        }
    }
}
