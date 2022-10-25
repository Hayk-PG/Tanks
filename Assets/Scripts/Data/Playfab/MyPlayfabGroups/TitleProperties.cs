public class TitleProperties
{
    public string GroupID { get; private set; }
    public string GroupType { get; private set; }
    public string MemberID { get; private set; }
    public string MemberType { get; private set; }


    public TitleProperties(string groupId, string groupType, string memberId, string memberType)
    {
        GroupID = groupId;
        GroupType = groupType;
        MemberID = memberId;
        MemberType = memberType;
    }
}