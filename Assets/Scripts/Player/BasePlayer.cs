using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    protected virtual void AssignGameObjectName(string name)
    {
        this.name = name;
    }
}
