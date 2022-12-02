using UnityEngine;

public class OptionsGear : MonoBehaviour
{
    private Btn _btn;
    private Options _options;


    private void Awake()
    {
        _btn = Get<Btn>.From(gameObject);
        _options = FindObjectOfType<Options>();
    }

    private void OnEnable() => _btn.onSelect += OpenOptions;

    private void OnDisable() => _btn.onSelect -= OpenOptions;

    private void OpenOptions()
    {
        _options.Activity(true);
    }
}
