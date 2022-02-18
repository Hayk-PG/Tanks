using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Animator _anim;
    private Text _text;


    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _text = GetComponent<Text>();
    }

    public void UpdateScoreText(int score)
    {
        _text.text = score.ToString();
        _anim.SetTrigger("play");
    }
}
