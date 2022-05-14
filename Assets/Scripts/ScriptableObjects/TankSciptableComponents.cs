using UnityEngine;
using System;
using UnityEditor;
using UnityEditorInternal;

[CreateAssetMenu(fileName = "New components", menuName = "Tank components")]
public class TankSciptableComponents : ScriptableComponents
{
    private Rigidbody _rigidbody;
    private WheelColliderController _wheelColliderController;
    private WheelCollider _wheelCollider;


    public override void OnClickGetComponents()
    {
        _scripts = _componentsHolder.GetComponents<MonoBehaviour>();
        _rigidbody = Get<Rigidbody>.From(_componentsHolder);
        _wheelColliderController = Get<WheelColliderController>.FromChild(_componentsHolder);
        _wheelCollider = Get<WheelCollider>.FromChild(_wheelColliderController.gameObject);

        EditorUtility.SetDirty(this);
    }

    public void OnClickAddComponnets()
    {
        if (_target != null)
        {
            TagAndLayer();
            AddRigidbody();
            AddMeshCollider();
            AddRootScripts();
            AddWheelColliderController();
        }
    }

    private void TagAndLayer()
    {
        _target.tag = Tags.Player;
        _target.layer = LayerMask.NameToLayer(Layers.Vehicle);
    }

    private void AddRigidbody()
    {
        ComponentUtility.CopyComponent(_rigidbody);

        if (_target.GetComponent<Rigidbody>() == null)
            ComponentUtility.PasteComponentAsNew(_target.gameObject);
        else
            ComponentUtility.PasteComponentValues(_target.GetComponent(Get<object>.Type(_rigidbody)));
    }

    private void AddMeshCollider()
    {
        if (_target.transform.Find("Body") != null)
        {
            if (_target.transform.Find("Body").GetChild(0).gameObject.GetComponent<MeshCollider>() == null)
            {
                MeshCollider newMeshCollider = _target.transform.Find("Body").GetChild(0).gameObject.AddComponent<MeshCollider>();
                newMeshCollider.convex = true;
            }
        }
    }

    private void AddRootScripts()
    {
        GlobalFunctions.Loop<MonoBehaviour>.Foreach(_scripts, script =>
        {
            ComponentUtility.CopyComponent(script);
            Type scriptType = Get<object>.Type(script);

            if (_target.GetComponent(scriptType) == null) ComponentUtility.PasteComponentAsNew(_target);
            if (_target.GetComponent(scriptType) != null) ComponentUtility.PasteComponentValues(_target.GetComponent(scriptType));

            var newScript = _target.GetComponent(scriptType);

            if (newScript.GetType() == typeof(TankMovement))
            {
                TankMovement tankMovement = (TankMovement)newScript;
                tankMovement._wheelColliderController = Get<WheelColliderController>.FromChild(_target);
                tankMovement._rigidBody = Get<Rigidbody>.From(_target);
                tankMovement._rayCasts = Get<Raycasts>.FromChild(_target);
            }

            if (newScript.GetType() == typeof(ShootController))
            {
                ShootController shootController = (ShootController)newScript;
                shootController._canonPivotPoint = _target.transform.Find("CanonPivotPoint");
                shootController._shootPoint = shootController._canonPivotPoint.GetChild(0).transform.Find("ShootPoint");
                shootController._trajectory = Get<PlayerShootTrajectory>.From(shootController._shootPoint.gameObject);
            }

            if (newScript.GetType() == typeof(VehicleFall))
            {
                VehicleFall vehicleFall = (VehicleFall)newScript;

                GlobalFunctions.Loop<ParticleSystem>.Foreach(_target.GetComponentsInChildren<ParticleSystem>(true), particle => 
                {
                    if (particle.name == "SmokeCircleBright") vehicleFall._smoke = particle.gameObject;
                });
            }
        });
    }

    private void AddWheelColliderController()
    {
        ComponentUtility.CopyComponent(_wheelCollider);
        Type wheelColliderType = Get<object>.Type(_wheelCollider);
        WheelColliderController wheelColliderController = Get<WheelColliderController>.FromChild(_target);
        Transform[] wheels = wheelColliderController.gameObject.GetComponentsInChildren<Transform>();

        GlobalFunctions.Loop<Transform>.Foreach(wheels, wheel => 
        {
            if (wheel.GetComponent(wheelColliderType) == null) ComponentUtility.PasteComponentAsNew(wheel.gameObject);
            if (wheel.GetComponent(wheelColliderType) != null) ComponentUtility.PasteComponentValues(wheel.GetComponent(wheelColliderType));
        });
    }
}
