using UnityEngine;

public class AbilityDodge : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private PlayerTurn _playerTurn;

    [SerializeField] [Space]
    private bool _isActive;





    private void OnEnable()
    {
        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            if(Distance(GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController) < 5)
            {

            }
        }
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if(_playerTurn.MyTurn != turnState)
        {

        }
    }

    private float Distance(BaseBulletController bulletController)
    {
        return bulletController != null ? Vector3.Distance(_rigidbody.position, bulletController.RigidBody.position) : 100;
    }
}
