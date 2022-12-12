using UnityEngine;
using UnityEngine.UI;

public class WindUI : MonoBehaviour
{
    [SerializeField] private Image[] _arrows;
    [SerializeField] private Color[] _clrs;
    private WindSystemController _windSystemController;


    private void Awake() => _windSystemController = FindObjectOfType<WindSystemController>();

    private void OnEnable() => _windSystemController.onWindForce += OnWindForce;

    private void OnDisable() => _windSystemController.onWindForce -= OnWindForce;

    private void OnWindForce(int windForce)
    {
        transform.rotation = Quaternion.Euler(0, windForce > 0 ? -180 : 0, 0);
        IconsActivity(_arrows.Length, _clrs[1]);
        IconsActivity(Mathf.Abs(windForce), _clrs[0]);
    }

    private void IconsActivity(int count, Color color)
    {
        int c = count > _arrows.Length ? _arrows.Length : count;

        for (int i = 0; i < c; i++)
        {
            _arrows[i].color = color;
        }
    }
}
