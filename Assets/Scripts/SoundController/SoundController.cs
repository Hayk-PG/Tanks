using UnityEngine;

public class SoundController : MonoBehaviour
{
    protected static SoundController _inst;
    protected AudioSource _musicSRC;


    protected virtual void Awake()
    {
        Instance();
        _musicSRC = Get<AudioSource>.From(transform.Find("MusicSRC").gameObject);
    }

    protected void Instance()
    {
        if (_inst != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _inst = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void MusicSRCCondition(bool isMuted)
    {
        _inst._musicSRC.mute = !isMuted;
    }
}
