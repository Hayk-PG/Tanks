using System.Collections;
using UnityEngine;

public class GameplayAnnouncer : BaseAnnouncer
{
    protected override void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }
    protected override void OnDisable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnGameStarted()
    {
        StartCoroutine(GameStartAnnouncementCoroutine());
    }

    private IEnumerator GameStartAnnouncementCoroutine()
    {
        TextAnnouncement(0, "Ready?", true);
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(0, 0, out float clipLength);
        yield return new WaitForSeconds(clipLength);
        TextAnnouncement(0, "", false);
        TextAnnouncement(0, "Go!", true);
        SoundController.PlaySound(0, 1, out float nextClipLength);
        yield return new WaitForSeconds(nextClipLength);
        TextAnnouncement(0, "", false);
        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }
}
