using System;

public static class GlobalFunctions 
{
    public static void OnClick(Action Func)
    {
        Func?.Invoke();
    }
}
