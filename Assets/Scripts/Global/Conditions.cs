﻿using System;

public class Conditions<T>
{
    public delegate void F();


    public static void Compare<T1>(T1 a, T1 b, Action OnEquals, Action OnGreater, Action OnLesser) where T1 : IComparable
    {
        if (a.Equals(b))
            OnEquals?.Invoke();

        if (a.CompareTo(b) > 0)
            OnGreater?.Invoke();

        if (a.CompareTo(b) < 0)
            OnLesser?.Invoke();
    }

    public static void Compare(bool A, bool B, F OnATrue, F OnBTrue, F OnAFalse, F OnBFalse)
    {
        if (A == true)
            OnATrue?.Invoke();
        if (B == true)
            OnBTrue?.Invoke();
        if (A == false)
            OnAFalse?.Invoke();
        if (B == false)
            OnBFalse?.Invoke();
    }

    public static void CheckNull<T3>(T3 a, T3 b, Action OnNull, Action OnExists)
    {
        if (a == null)
            OnNull?.Invoke();
        else
            OnExists.Invoke();
    }
}
