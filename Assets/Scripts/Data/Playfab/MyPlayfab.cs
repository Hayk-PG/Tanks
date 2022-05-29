using UnityEngine;


public partial class MyPlayfab : MonoBehaviour
{
    public static MyPlayfab Manager { get; private set; }


    private void Awake()
    {
        Manager = this;
    }
}
