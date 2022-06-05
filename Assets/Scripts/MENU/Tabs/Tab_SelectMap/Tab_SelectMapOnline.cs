using UnityEngine;

public class Tab_SelectMapOnline : MonoBehaviour
{
    public struct RoomProperties
    {
        public string _roomName;
        public string _password;
        public bool _isPasswordSet;

        public RoomProperties(string roomName, string password, bool isPasswordSet)
        {
            _roomName = roomName;
            _password = password;
            _isPasswordSet = isPasswordSet;
        }
    }
    public RoomProperties _roomProperties;    
}
