using UnityEngine;

public class Tab_GuidedMissileController : MonoBehaviour
{
    [SerializeField]
    private VariableJoystick _variableJoystick;

    public Vector3 Direction
    {
        get
        {
            return new Vector2(_variableJoystick.Horizontal, _variableJoystick.Vertical);
        }
    }
}
