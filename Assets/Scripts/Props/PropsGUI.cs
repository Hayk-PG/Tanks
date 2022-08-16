using UnityEngine;
using TMPro;

public class PropsGUI : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;   
    [SerializeField] private Animator _animator;
    [SerializeField ]private TMP_Text _text;
    private Tile _tile;


    private void Awake()
    {
        _canvas.worldCamera = Camera.main;
        _tile = Get<Tile>.From(gameObject);
    }

    private void OnEnable()
    {
        _tile.OnTileHealth += OnTileHealth;
    }

    private void OnDisable()
    {
        _tile.OnTileHealth -= OnTileHealth;
    }

    private void OnTileHealth(float health)
    {
        _text.text = health.ToString();
        _animator.SetTrigger("play");
    }
}
