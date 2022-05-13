using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankProperties))]
public class TankPropertiesCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TankProperties tankProperties = (TankProperties)target;

        if(GUILayout.Button("Get values from tank prefab", GUILayout.Height(40)))
        {
            tankProperties.GetValuesFromTankPrefab();
        }
    }
}

[CustomEditor(typeof(TankSciptableComponents))]
public class ScriptableComponentsCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TankSciptableComponents tankScriptableComponents = (TankSciptableComponents)target;

        if (GUILayout.Button("Get components from the components holder", GUILayout.Height(40)))
        {
            tankScriptableComponents.OnClickGetComponents();
        }

        if (GUILayout.Button("Add components to the target", GUILayout.Height(40)))
        {
            tankScriptableComponents.OnClickAddComponnets();
        }
    }
}
