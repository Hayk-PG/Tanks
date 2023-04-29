using System;
using UnityEngine;


public class WoodBox : MonoBehaviour
{
    [SerializeField] [Space]
    private ParachuteWithWoodBoxCollision _parachuteWithWoodBoxCollision;

    public event Action onPickDropBox;




    private void OnEnable() => _parachuteWithWoodBoxCollision.onCollisionEnter += GetCollisions;

    private void OnDisable() => _parachuteWithWoodBoxCollision.onCollisionEnter -= GetCollisions;

    private void GetCollisions(ParachuteWithWoodBoxCollision.CollisionData collisionData)
    {
        if (collisionData._tankController == null)
            return;

        bool isLocalPlayerPickingDropBox = collisionData._tankController.BasePlayer != null;

        Conditions<bool>.Compare(isLocalPlayerPickingDropBox, HandleLocalPlayerPickDropBox, HandleEnemyPickDropBox);
    }

    private void HandleLocalPlayerPickDropBox()
    {
        RaiseOnPickDropBoxEvent();

        SecondarySoundController.PlaySound(9, 0);

        GameSceneObjectsReferences.Tab_DropBoxItemSelection.SetActivity(true);
    }

    private void HandleEnemyPickDropBox()
    {
        RaiseOnPickDropBoxEvent();

        SecondarySoundController.PlaySound(9, 0);
    }

    private void RaiseOnPickDropBoxEvent() => onPickDropBox?.Invoke();
}
