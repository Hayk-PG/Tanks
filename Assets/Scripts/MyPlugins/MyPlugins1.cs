using System;
using UnityEngine;

public partial class MyPlugins : MonoBehaviour
{
    private AndroidJavaClass _unityClass;
    private AndroidJavaObject _unityActivity;
    private AndroidJavaObject _testString;
    private static AndroidJavaObject _pluginInstance;

    public Action OnPluginService { get; set; }


    private void Start()
    {
        InitializePlugin("com.defaultcompany.unityplugin.ServiceIntent");
    }

    private void OnDestroy()
    {
        StopService();
    }

    private void InitializePlugin(string pluginName)
    {
        _unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _unityActivity = _unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        _pluginInstance = new AndroidJavaObject(pluginName);

        _pluginInstance.CallStatic("GetUnityActivity", _unityActivity);

        StartService();
    }

    public void StartService()
    {
        _pluginInstance.CallStatic("StartService");
    }

    public void StopService()
    {
        _pluginInstance.CallStatic("StopService");
    }

    public void Message()
    {
        OnPluginService?.Invoke();
    }
}
