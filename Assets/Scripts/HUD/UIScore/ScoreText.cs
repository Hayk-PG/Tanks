using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Animator _anim;
    private Text _text;

    private int Score
    {
        get => int.Parse(_text.text);
        set => _text.text = value.ToString();
    }


    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _text = GetComponent<Text>();
    }

    public void UpdateScoreText(int score)
    {
        Score += score;
        if(_anim != null) _anim.SetTrigger("play");
    }
}
