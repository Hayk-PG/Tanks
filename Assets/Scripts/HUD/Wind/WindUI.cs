using UnityEngine;

public class WindUI : MonoBehaviour
{
    private WindSystemController _windSystemController;

    [SerializeField]
    private GameObject[] _arrows;

    [SerializeField]
    private GameObject _windIcon;


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
        IconsActivity(_arrows.Length, windForce, false);
        IconsActivity(Mathf.Abs(windForce), windForce, true);
    }

    private void IconsActivity(int count, int windForce, bool isActive)
    {
        for (int i = 0; i < count; i++)
        {
            _arrows[i].SetActive(isActive);
            _arrows[i].transform.rotation = Quaternion.Euler(0, windForce > 0 ? -180: 0, 0); 
        }

        _windIcon.SetActive(isActive);
    }
}
