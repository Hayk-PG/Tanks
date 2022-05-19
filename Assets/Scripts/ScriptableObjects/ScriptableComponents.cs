using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Component/New component")]
public abstract class ScriptableComponents : ScriptableObject
{
    [Header("Original game object")]
    public GameObject _componentsHolder;

    [Header("Target game object")]
    public GameObject _target;

    [Header("Script components")]
    public MonoBehaviour[] _scripts;

    
    public abstract void OnClickGetComponents();
}
