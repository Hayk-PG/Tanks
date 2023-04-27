using UnityEngine;

public class ParachuteIcon : BaseOutOfBoundsIndicator
{
    private void Update() => ControlMovement();

    public override void Init(Transform target) => Target = target;
}
