using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LastHealthFill : MonoBehaviour
{
    private Image _image;
    private Animator _animator;
    private RectTransform _rectTransform;

    private string _animTriggerName = "play";
    private float _anchorMax;


    private void Awake()
    {
        _image = GetComponent<Image>();
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnUpdate(float anchorMax)
    {
        _anchorMax = anchorMax;
        StartCoroutine(PlayAnimation());
    }

    public void OnAnimationEnd()
    {
        _rectTransform.anchorMax = new Vector2(_anchorMax, 1);
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
    }

    private IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(1);
        _animator.SetTrigger(_animTriggerName);
    }
}
