using UnityEngine;

public class MatchmakeRoomInfo : MonoBehaviour, IMatchmakeTextResult
{
    protected CustomInputField _customInputField;


    protected virtual void Awake()
    {
        _customInputField = Get<CustomInputField>.From(gameObject);
    }

    public virtual string TextResultOffline()
    {
        return null;
    }

    public virtual string TextResultOnline()
    {
        return null;
    }
}
