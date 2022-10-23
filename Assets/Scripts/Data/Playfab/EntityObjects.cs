public class EntityObjects 
{
    public string ObjectName { get; private set; }
    public string Key { get; private set; }
    public object Value { get; private set; }


    public EntityObjects(string objectsName, string key, object value)
    {
        ObjectName = objectsName;
        Key = key;
        Value = value;
    }
}
