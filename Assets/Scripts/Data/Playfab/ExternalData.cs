using UnityEngine;

public class ExternalData : MonoBehaviour
{
    public static MyPlayfabEntity Entity { get; private set; }
    public static MyPlayfabTitleGroups TitleGroups { get; private set; }
    public static MyPlayfabEntityObjects EntityObjects { get; private set; }


    private void Awake()
    {
        Entity = new MyPlayfabEntity();
        TitleGroups = new MyPlayfabTitleGroups();
        EntityObjects = new MyPlayfabEntityObjects();
    }
}
