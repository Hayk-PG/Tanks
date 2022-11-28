using UnityEngine;

public class ExternalData : MonoBehaviour
{
    public static MyPlayfabRegistrationForm MyPlayfabRegistrationForm { get; private set; }
    public static MyPlayfabEntity Entity { get; private set; }
    public static MyPlayfabProfile Profile { get; private set; }
    public static MyPlayfabTitleGroups TitleGroups { get; private set; }
    public static MyPlayfabEntityObjects EntityObjects { get; private set; }
    public static MyPlayfabUserData MyPlayfabUserData { get; private set; }


    private void Awake()
    {
        MyPlayfabRegistrationForm = new MyPlayfabRegistrationForm();
        Entity = new MyPlayfabEntity();
        Profile = new MyPlayfabProfile();
        TitleGroups = new MyPlayfabTitleGroups();
        EntityObjects = new MyPlayfabEntityObjects();
        MyPlayfabUserData = new MyPlayfabUserData();
    }
}
