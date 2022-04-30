using UnityEngine;

public partial class Data : MonoBehaviour
{
    public static Data Manager { get; private set; }

    private void Awake()
    {
        Instance();
    }

    private void Instance()
    {
        if (Manager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
