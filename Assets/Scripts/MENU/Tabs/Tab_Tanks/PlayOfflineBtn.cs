using UnityEngine;

[RequireComponent(typeof(Btn))]

public class PlayOfflineBtn : MonoBehaviour
{
    private Btn _btn;


    private void Awake()
    {
        _btn = Get<Btn>.From(gameObject);
    }

    private void OnEnable()
    {
        _btn.onSelect += Select;
    }

    private void OnDisable()
    {
        _btn.onSelect -= Select;
    }

    private void Select()
    {
        MyScene.Manager.LoadScene(MyScene.SceneName.Game);
    }
}
