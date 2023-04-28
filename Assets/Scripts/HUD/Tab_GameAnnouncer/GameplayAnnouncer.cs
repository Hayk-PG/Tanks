using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameplayAnnouncer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _txt;

    [SerializeField] [Space]
    private TMP_ColorGradient __yellow, _green, _blue, _red;

    private string[] _gameResultTexts = new string[2];

    public event Action onGameStartAnnouncement;




    private void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnGameStarted() => StartCoroutine(AnnounceGameStart());

    private IEnumerator AnnounceGameStart()
    {
        yield return new WaitForSeconds(1);

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Down);
        SoundController.PlaySound(0, Indexes.Combat_Announcer_Male_Effect_Ready, out float clipLength);

        yield return null;

        TextAnnouncement("Ready?", true);

        SetColorGradient(__yellow);

        yield return new WaitForSeconds(clipLength);

        SoundController.PlaySound(0, Indexes.Combat_Announcer_Male_Effect_Go, out float nextClipLength);

        yield return null;

        TextAnnouncement("", false);
        TextAnnouncement("Go!", true);

        SetColorGradient(_green);

        onGameStartAnnouncement?.Invoke();

        GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(null, HudTabsHandler.HudTab.GameplayAnnouncer, false);

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

        _gameResultTexts = isWin ? new string[] { "You", " Win!" } : new string[] { "You", " Lose!"};

        SetColorGradient(isWin ? _blue : _red);

        yield return null;

        TextAnnouncement(_gameResultTexts[0], true);

        yield return new WaitForSeconds(clipLength / 5);

        TextAnnouncement("", false);
        TextAnnouncement(_gameResultTexts[1], true);

        yield return null;

        SoundController.MusicSRCVolume(SoundController.MusicVolume.Up);
    }

    private void TextAnnouncement(string text, bool isActive)
    {
        _txt.text = text;
        _txt.gameObject.SetActive(isActive);

        PlayWhooshSoundFx(!String.IsNullOrWhiteSpace(text));
    }

    private void PlayWhooshSoundFx(bool canPlay)
    {
        if (!canPlay)
            return;

        UISoundController.PlaySound(5, UnityEngine.Random.Range(0, 2));
    }

    private void SetColorGradient(TMP_ColorGradient tMP_ColorGradient)
    {
        _txt.colorGradientPreset = tMP_ColorGradient;
    }
}
