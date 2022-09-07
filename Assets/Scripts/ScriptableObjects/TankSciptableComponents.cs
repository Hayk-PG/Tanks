#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable objects/Component/New tank's component")]
public class TankSciptableComponents : ScriptableComponents
{
    private Rigidbody _rigidbody;
    private WheelColliderController _wheelColliderController;
    private WheelCollider _wheelCollider;


    public override void OnClickGetComponents()
    {
        IsComponentsHolderIsAITank();

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

    private bool IsComponentsHolderIsAITank()
    {
        return _componentsHolder.GetComponents<MonoBehaviour>().ToList().Find(m => m.GetType() == typeof(AITankMovement));
    }

    private void TagAndLayer()
    {
        _target.tag = IsComponentsHolderIsAITank()? Tags.AI: Tags.Player;
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
        });
    }

    private void AddWheelColliderController()
    {
        ComponentUtility.CopyComponent(_wheelColliderController);
        ComponentUtility.CopyComponent(_wheelCollider);

        Transform wheelCollidersTransform = _target.transform.Find("WheelColliders");
        Transform[] wheels = wheelCollidersTransform.gameObject.GetComponentsInChildren<Transform>();
        Type wheelColliderType = Get<object>.Type(_wheelCollider);

        if (wheelCollidersTransform.GetComponent<WheelColliderController>() == null)
            ComponentUtility.PasteComponentAsNew(wheelCollidersTransform.gameObject);
        else
            ComponentUtility.PasteComponentValues(wheelCollidersTransform.GetComponent<WheelColliderController>());         

        GlobalFunctions.Loop<Transform>.Foreach(wheels, wheel =>
        {
            if (wheel.GetComponent(wheelColliderType) == null) ComponentUtility.PasteComponentAsNew(wheel.gameObject);
            if (wheel.GetComponent(wheelColliderType) != null) ComponentUtility.PasteComponentValues(wheel.GetComponent(wheelColliderType));
        });
    }
}
#endif
