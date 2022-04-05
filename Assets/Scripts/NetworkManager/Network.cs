using UnityEngine;

public class Network : MonoBehaviour
{
    public static Network Manager { get; set; }


    private void Awake()
    {
        if(Manager != null)
        {
            Destroy(Manager);
        }
        else
        {
            Manager = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
