
public partial class Data
{
    public int Points { get; private set; }
    public int Level { get; private set; }
    public int[,] PointsSliderMinAndMaxValues { get; private set; } = new int[,]
    {
        { 0, 9900}, 
        {9900, 24560}
    };
}
