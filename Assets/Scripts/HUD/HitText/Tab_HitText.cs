using UnityEngine;

public class Tab_HitText : MonoBehaviour
{
    [SerializeField]
    private HitTextManager _hitTextManagerForPlayer1, _hitTextManagerForPlayer2;

    public HitTextManager HitTexManagerForPlayer1 => _hitTextManagerForPlayer1;
    public HitTextManager HitTextManagerForPlayer2 => _hitTextManagerForPlayer2;
}