using UnityEngine;
using TMPro;


public class Tab_WoodboxContent : MonoBehaviour
{
    public enum Content { Health, Score, Bullet}

    [SerializeField] private GameObject[] _icons;
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _text;

    private bool IsHealth
    {
        get => _icons[0].activeInHierarchy;
        set => _icons[0].SetActive(value);
    }
    private bool IsScore
    {
        get => _icons[1].activeInHierarchy;
        set => _icons[1].SetActive(value);
    }
    private bool IsBullet
    {
        get => _icons[2].activeInHierarchy;
        set => _icons[2].SetActive(value);
    }
    private string Text
    {
        get => _text.text;
        set => _text.text = value;
    }

    public void OnContent(Content content, string text)
    {
        GlobalFunctions.Loop<GameObject>.Foreach(_icons, icon => { icon.SetActive(false); });
        _animator.SetTrigger("play");
        Text = text;
        SoundController.PlaySound(4, 0, out float clipLength);

        switch (content)
        {
            case Content.Health: IsHealth = true; break;
            case Content.Score: IsScore = true; break;
            case Content.Bullet: IsBullet = true; break;
        }
    }

    public void OnAnimationEnd()
    {
        IsHealth = false;
        IsScore = false;
        IsBullet = false;
    }
}
