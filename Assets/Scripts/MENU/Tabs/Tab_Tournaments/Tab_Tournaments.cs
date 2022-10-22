public class Tab_Tournaments : Tab_Base<MyPlayfabTitleGroupsEntityKeys>
{
    private void OnEnable()
    {
        _object.onShareEntities += OnTitleGroupEntitiesShared;
    }

    private void OnDisable()
    {
        _object.onShareEntities -= OnTitleGroupEntitiesShared;
    }

    private void OnTitleGroupEntitiesShared(MyPlayfabTitleGroupsEntityKeys myPlayfabTitleGroupsEntityKeys)
    {
        base.OpenTab();
    }
}
