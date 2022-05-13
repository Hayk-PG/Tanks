using UnityEngine;

[CreateAssetMenu(fileName = "New components", menuName = "Components")]
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
