
public static class EventInfo
{
    public static byte Code_InstantiatePlayers { get; private set; } = 0;
    public static object Content_InstantiatePlayers { get; private set; } = "InstantiatePlayers";

    public static byte Code_InstantiateWoodenBox { get; private set; } = 1;
    public static object[] Content_InstantiateWoodenBox { get; set; }

    public static byte Code_TornadoDamage { get; private set; } = 2;
    public static object[] Content_TornadoDamage { get; set; }
}
