using UnityEngine;


public class WoodBox : MonoBehaviour
{
    [SerializeField] [Space]
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;



    private void OnEnable() => _parachuteWithWoodBoxCollision.onCollisionEnter += GetCollisions;

    private void OnDisable() => _parachuteWithWoodBoxCollision.onCollisionEnter -= GetCollisions;

    private void GetCollisions(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        if (collisionData._tankController?.BasePlayer == null)
            return;

        GameSceneObjectsReferences.Tab_DropBoxItemSelection.SetActivity(true);
    }
}
