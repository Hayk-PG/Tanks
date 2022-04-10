using UnityEngine;

public partial class MyPlugins : MonoBehaviour
{
    public static MyPlugins _instance;

    


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
