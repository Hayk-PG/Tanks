using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GlobalFunctions 
{
    public static void CanvasGroupActivity(CanvasGroup canvasGroup, bool isEnabled)
    {
        if (isEnabled)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public static void DebugLog(object message)
    {
        Debug.Log(message);
    }

    public static bool LocalPlayerChecker(bool isLocalPlayer)
    {
        return isLocalPlayer;
    }

    public class Loop<T>
    {
        public static void Foreach(T[] t, Action<T> OnLoop)
        {
            if (t != null)
            {
                foreach (var item in t)
                {
                    OnLoop?.Invoke(item);
                }
            }
        }

        public static void Foreach(List<T> t, Action<T> OnLoop)
        {
            if (t != null)
            {
                foreach (var item in t)
                {
                    OnLoop?.Invoke(item);
                }
            }
        }
    }

    public class ObjectsOfType<T> where T: UnityEngine.Object
    {
        public static T Find(Predicate<T> p)
        {
            return MonoBehaviour.FindObjectsOfType<T>().ToList().Find(p);
        }
    }   
}
