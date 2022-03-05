﻿using System;
using System.Collections.Generic;
using UnityEngine;

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
}
