using System.Collections.Generic;
using UnityEngine;

public class MyCameras : MonoBehaviour
{
    public static MyCameras Instance;

    [SerializeField]
    private List<Camera> _cameras;

    public static List<Camera> Cameras => Instance._cameras;




    private void Awake()
    {
        Instance = this;
    }
}
