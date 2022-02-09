using UnityEngine;

public class Get<T> : MonoBehaviour 
{
    public static T From(GameObject obj) 
    {
        return obj.GetComponent<T>() != null ? obj.GetComponent<T>() :
               obj.GetComponentInParent<T>() != null ? obj.GetComponentInParent<T>() : default;
    }
}
