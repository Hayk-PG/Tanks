using UnityEngine;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "New components", menuName = "Tank components")]
public class TankSciptableComponents : ScriptableComponents
{
    [Header("Layer mask")]
    [SerializeField] private LayerMask _tankLayerMask;

    [Header("Child scripts")]
    [SerializeField] private WheelColliderController _wheelColliderController;



    public override void OnClickGetComponents()
    {
        _scripts = _componentsHolder.GetComponents<MonoBehaviour>();
        _wheelColliderController = Get<WheelColliderController>.FromChild(_componentsHolder);
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
        _target.layer = _tankLayerMask;
    }

    private void AddRigidbody()
    {
        if (_target.GetComponent<Rigidbody>() == null) _target.AddComponent<Rigidbody>();
    }

    private void AddMeshCollider()
    {
        if (_target.transform.Find("Body") != null)
        {
            if (_target.transform.Find("Body").GetChild(0).gameObject.GetComponent<MeshCollider>() == null)
            {
                MeshCollider meshCollider = _target.transform.Find("Body").GetChild(0).gameObject.AddComponent<MeshCollider>();
                meshCollider.convex = true;
            }
        }
    }

    private void AddRootScripts()
    {
        GlobalFunctions.Loop<MonoBehaviour>.Foreach(_scripts, script =>
        {
            Type s = script.GetType();
            if (_target.GetComponent(s) == null) _target.AddComponent(s);
        });
    }

    private void AddWheelColliderController()
    {
        if (_target.transform?.Find("WheelColliders").GetComponent<WheelColliderController>() != null)
        {
            _target.transform.Find("WheelColliders").gameObject.AddComponent<WheelColliderController>();
        }
    }
}
