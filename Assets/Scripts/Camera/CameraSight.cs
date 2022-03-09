using UnityEngine;

public class CameraSight : MonoBehaviour
{
    private static Camera MainCamera { get; set; }



    private void Awake()
    {
        MainCamera = Camera.main;
    }

    public static Vector3 ScreenPoint(Vector3 position)
    {
        return MainCamera.WorldToScreenPoint(position);
    }

    public static Vector3 WorldPoint(Vector3 position)
    {
        return MainCamera.ScreenToWorldPoint(position);
    }

    public static Vector3 ViewportPoint(Vector3 position)
    {
        return MainCamera.WorldToViewportPoint(position);
    }

    public static bool IsInCameraSight(Vector3 position)
    {
        return ViewportPoint(position).x >= 0 && ViewportPoint(position).x <= 1 && ViewportPoint(position).y >= 0 &&
               ViewportPoint(position).y <= 1 ? true : false;
    }
}
