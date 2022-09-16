#if UNITY_EDITOR
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

        if (GUILayout.Button("Apply values to the same type AI Tank", GUILayout.Height(40)))
        {
            tankProperties.ApplyValuesToSameTypeAITank();
        }
    }
}

[CustomEditor(typeof(AITankProperties))]
public class AITankPropertiesCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AITankProperties aiTankProperties = (AITankProperties)target;

        if (GUILayout.Button("Get values from ai tank prefab", GUILayout.Height(40)))
        {
            aiTankProperties.GetValuesFromTankPrefab();
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

[CustomEditor(typeof(WeaponProperties))]
public class WeaponPropertiesCustomEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponProperties weaponProperties = (WeaponProperties)target;

        if (GUILayout.Button("Randomize", GUILayout.Height(40)))
        {
            weaponProperties.Randomize();
        }

        if (GUILayout.Button("Set properties", GUILayout.Height(40)))
        {
            weaponProperties.OnClickSetWeaponProperties();
        }
    }
}

[CustomEditor(typeof(DetailsLevel))]
public class DetailsLevelCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DetailsLevel detailsLevel = (DetailsLevel)target;

        if (GUILayout.Button("Place", GUILayout.Height(40)))
        {
            detailsLevel.Place();
        }
    }
}

#endif
