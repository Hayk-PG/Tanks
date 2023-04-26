using UnityEngine;

public class EnemyPlayerIcon : BaseOutOfBoundsIndicator
{
    private void FixedUpdate() => ControlMovement();

    public override void Init(Transform target)
    {
        if (target == null)
            return;

        FaceDirection = target.name == Names.Tank_FirstPlayer ? new Vector2(-1, 1) : new Vector2(1, 1);

        Target = target;
    }
}
