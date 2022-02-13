using UnityEngine;

public class WindUI : MonoBehaviour
{
    private WindSystemController _windSystemController;

    [SerializeField]
    private GameObject[] _icons;


    private void Awake()
    {
        _windSystemController = FindObjectOfType<WindSystemController>();
    }

    private void OnEnable()
    {
        _windSystemController.OnWindForce += OnWindForce;
    }

    private void OnDisable()
    {
        _windSystemController.OnWindForce -= OnWindForce;
    }

    private void OnWindForce(int windForce)
    {
        IconsActivity(_icons.Length, windForce, false);
        IconsActivity(Mathf.Abs(windForce), windForce, true);
    }

    private void IconsActivity(int count, int windForce, bool isActive)
    {
        for (int i = 0; i < count; i++)
        {
            _icons[i].SetActive(isActive);
            _icons[i].transform.rotation = Quaternion.Euler(0, windForce > 0 ? -180: 0, 0); 
        }
    }
}
