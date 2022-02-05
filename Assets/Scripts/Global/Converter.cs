
public struct Converter
{
    public static float AngleConverter(float axis)
    {
        axis = axis > 180 ? axis - 360 : axis;
        return axis;
    }
}
