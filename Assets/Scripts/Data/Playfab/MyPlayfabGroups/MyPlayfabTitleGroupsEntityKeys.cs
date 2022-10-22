using System;
using UnityEngine;

public class MyPlayfabTitleGroupsEntityKeys : MonoBehaviour
{
    [SerializeField] private bool _areEntitiesShared;
 
    public string[] GroupsID { get; private set; }
    public string GroupsType { get; private set; }

    public event Action<MyPlayfabTitleGroupsEntityKeys> onShareEntities;


    private void Awake()
    {
        GetGroupsIDFromPlayfab();
        GetGroupsTypeFromPlayfab();      
    }

    private void Update()
    {
        if (_areEntitiesShared)
        {
            onShareEntities?.Invoke(this);
            _areEntitiesShared = false;
        }
    }

    private void GetGroupsIDFromPlayfab() => GroupsID = new string[] { "CBE02DC5CDA093FF" };
    private void GetGroupsTypeFromPlayfab() => GroupsType = "title_player_account";
}
