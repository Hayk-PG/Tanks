using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameplayAnnouncer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _txt;

    public event Action onGameStartAnnouncement;



    private void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnGameStarted() => StartCoroutine(AnnounceGameStart());

    private IEnumerator AnnounceGameStart()
    {
        TextAnnouncement(GlobalFunctions.TextWithColorCode("#F6B30A", "Ready?"), true);

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(0, Indexes.Combat_Announcer_Male_Effect_Ready, out float clipLength);

        yield return new WaitForSeconds(clipLength);

        TextAnnouncement("", false);
        TextAnnouncement(GlobalFunctions.TextWithColorCode("#6EF60C", "Go"), true);

        onGameStartAnnouncement?.Invoke();

        SoundController.PlaySound(0, Indexes.Combat_Announcer_Male_Effect_Go, out float nextClipLength);

        yield return new WaitForSeconds(nextClipLength);

        TextAnnouncement("", false);

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }

    public void AnnounceGameResult(bool isWin)
    {
        StartCoroutine(AnnounceGameResultCoroutine(isWin));
    }

    private IEnumerator AnnounceGameResultCoroutine(bool isWin)
    {
        yield return new WaitForSeconds(1);

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);

        int clipIndex = isWin ? 2 : 3;

        SoundController.PlaySound(0, clipIndex, out float clipLength);

        TextAnnouncement(GlobalFunctions.TextWithColorCode(isWin ? "#17EAE0": "#EB4F7C", isWin ? "You win!" : "You Lose!"), true);

        yield return new WaitForSeconds(clipLength);

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }

    private void TextAnnouncement(string text, bool isActive)
    {
        _txt.text = text;
        _txt.gameObject.SetActive(isActive);
    }
}
