using System;


public struct Converter
{
    public static float AngleConverter(float axis)
    {
        axis = axis > 180 ? axis - 360 : axis;

        return axis;
    }

    public static string HhMMSS(int hours, int minutes, int seconds)
    {
        return hours.ToString("D2") + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    public static double Double(float value)
    {
        return Math.Round(value, 1);
    }
}
