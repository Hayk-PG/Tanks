#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable objects/Component/New tank's component")]
public class TankSciptableComponents : ScriptableComponents
{
    protected Rigidbody _rigidbody;
    protected WheelColliderController _wheelColliderController;
    protected WheelsColliderCenter _wheelColliderCenter;
    protected WheelCollider _wheelCollider;


    public override void OnClickGetComponents()
    {
        IsComponentsHolderIsAITank();

        _scripts = _componentsHolder.GetComponents<MonoBehaviour>();
        _rigidbody = Get<Rigidbody>.From(_componentsHolder);
        _wheelColliderController = Get<WheelColliderController>.FromChild(_componentsHolder);
        _wheelColliderCenter = Get<WheelsColliderCenter>.FromChild(_componentsHolder);
        _wheelCollider = Get<WheelCollider>.FromChild(_wheelColliderController.gameObject);

        EditorUtility.SetDirty(this);
    }

    public virtual void OnClickAddComponnets()
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

    public virtual void OnClickRemoveComponents()
    {
        if(_target != null)
        {
            GlobalFunctions.DebugLog(_target.GetComponents<Component>().Length);

            foreach (var component in _target.GetComponents<Component>())
            {
                if(component.GetType() != typeof(Transform))
                {
                    DestroyImmediate(component, true);
                }
            }
        }
    }

    protected virtual bool IsComponentsHolderIsAITank()
    {
        return _componentsHolder.GetComponents<MonoBehaviour>().ToList().Find(m => m.GetType() == typeof(AITankMovement));
    }

    protected virtual void TagAndLayer()
    {
        _target.tag = IsComponentsHolderIsAITank()? Tags.AI: Tags.Player;
        _target.layer = LayerMask.NameToLayer(Layers.Vehicle);
    }

    protected virtual void AddRigidbody()
    {
        ComponentUtility.CopyComponent(_rigidbody);

        if (_target.GetComponent<Rigidbody>() == null)
            ComponentUtility.PasteComponentAsNew(_target.gameObject);
        else
            ComponentUtility.PasteComponentValues(_target.GetComponent(Get<object>.Type(_rigidbody)));
    }

    protected virtual void AddMeshCollider()
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

    protected virtual void AddRootScripts()
    {
        GlobalFunctions.Loop<MonoBehaviour>.Foreach(_scripts, script =>
        {
            ComponentUtility.CopyComponent(script);
            Type scriptType = Get<object>.Type(script);

            if (_target.GetComponent(scriptType) == null) ComponentUtility.PasteComponentAsNew(_target);
            if (_target.GetComponent(scriptType) != null) ComponentUtility.PasteComponentValues(_target.GetComponent(scriptType));
        });
    }

    protected virtual void AddWheelColliderController()
    {
        Transform wheelCollidersTransform = _target.transform.Find("WheelColliders");
        Type wheelColliderType = Get<object>.Type(_wheelCollider);

        if (wheelCollidersTransform.GetComponent<WheelColliderController>() == null)
        {           
            ComponentUtility.CopyComponent(_wheelColliderController);
            ComponentUtility.PasteComponentAsNew(wheelCollidersTransform.gameObject);
        }
        else
        {
            ComponentUtility.PasteComponentValues(wheelCollidersTransform.GetComponent<WheelColliderController>());
        }

        if (wheelCollidersTransform.GetComponent<WheelsColliderCenter>() == null && _wheelColliderCenter != null && !IsComponentsHolderIsAITank())
        {
            ComponentUtility.CopyComponent(_wheelColliderCenter);
            ComponentUtility.PasteComponentAsNew(wheelCollidersTransform.gameObject);
        }
        else
        {
            ComponentUtility.PasteComponentValues(wheelCollidersTransform.GetComponent<WheelsColliderCenter>());
        }

        for (int i = 0; i < wheelCollidersTransform.childCount; i++)
        {
            ComponentUtility.CopyComponent(_wheelCollider);

            if (wheelCollidersTransform.GetChild(i).GetComponent(wheelColliderType) == null)
            {
                ComponentUtility.PasteComponentAsNew(wheelCollidersTransform.GetChild(i).gameObject);
            }
            if (wheelCollidersTransform.GetChild(i).GetComponent(wheelColliderType) != null)
            {
                ComponentUtility.PasteComponentValues(wheelCollidersTransform.GetChild(i).GetComponent(wheelColliderType));
            }
        }       
    }
}
#endif
