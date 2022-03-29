using UnityEngine;

public class PlayerHUDTransform : MonoBehaviour
{
    public Transform Target { get; set; }

    [SerializeField]
    private float _xPositionModifier, _yPositionModifier; //-0.5f, 0.35f

    private Quaternion _noRotation = Quaternion.Euler(0, 0, 0);


    private void Awake()
    {
        Target = Get<PlayerTurn>.From(gameObject).transform;
    }

    private void Update()
    {
        if (Target != null)
        {
            transform.position = new Vector2(Target.position.x + _xPositionModifier, Target.position.y + _yPositionModifier);
            transform.rotation = _noRotation;
        }
    }
}
