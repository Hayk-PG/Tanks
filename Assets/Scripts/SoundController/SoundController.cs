using UnityEngine;

public class SoundController : MonoBehaviour
{
    private static SoundController _inst;
    private AudioSource _musicSRC;


    private void Awake()
    {
        Instance();
        _musicSRC = Get<AudioSource>.From(transform.Find("MusicSRC").gameObject);
    }

    private void Instance()
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
