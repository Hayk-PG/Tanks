using System;
using System.Collections;
using UnityEngine;

public class GameplayAnnouncer : BaseAnnouncer
{
    private int coroutineCallCounts;



    protected override void OnEnable() => _gameManager.OnGameStarted += OnGameStarted;

    protected override void OnDisable() => _gameManager.OnGameStarted += OnGameStarted;

    private void OnGameStarted() => StartCoroutine(GameStartAnnouncementCoroutine());

    private IEnumerator GameStartAnnouncementCoroutine()
    {
        coroutineCallCounts++;

        if (coroutineCallCounts <= 1)
        {
            TextAnnouncement(0, GlobalFunctions.TextWithColorCode("#FF7700", "Ready?"), true);

            SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
            SoundController.PlaySound(0, Indexes.Combat_Announcer_Male_Effect_Ready, out float clipLength);

            yield return new WaitForSeconds(clipLength);

            TextAnnouncement(0, "", false);
            TextAnnouncement(0, GlobalFunctions.TextWithColorCode("#9DD70D", "Go"), true);

            OnGameStartAnnouncement?.Invoke();

            SoundController.PlaySound(0, Indexes.Combat_Announcer_Male_Effect_Go, out float nextClipLength);

            yield return new WaitForSeconds(nextClipLength);

            TextAnnouncement(0, "", false);

            SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
        }
    }
}
