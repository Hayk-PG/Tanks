using UnityEngine;
using TMPro;

public class PropsGUI : MonoBehaviour
{
    [SerializeField]
    protected Canvas _canvas;
    
    [SerializeField] [Space]
    protected Animator _animator;

    [SerializeField] [Space]
    protected TMP_Text _text;

    protected Tile _tile;


    protected virtual void Awake()
    {
        _canvas.worldCamera = Camera.main;

        _tile = Get<Tile>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        _tile.OnTileHealth += OnTileHealth;
    }

    protected virtual void OnDisable()
    {
        _tile.OnTileHealth -= OnTileHealth;
    }

    protected virtual void OnTileHealth(float health)
    {
        _text.text = health.ToString();

        _animator.SetTrigger("play");
    }
}
