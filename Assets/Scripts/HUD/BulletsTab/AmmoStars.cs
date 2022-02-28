using UnityEngine;
using UnityEngine.UI;


public enum AmmoTypeStars { OneStar, TwoStars, ThreeStars, FourStars, FiveStars }

public class AmmoStars : MonoBehaviour
{        
    public AmmoTypeStars _ammoTypeStars;

    [SerializeField]
    private Image[] _stars;

    [SerializeField]
    private Sprite[] _starsSprites;


    public void OnSetStars(AmmoTypeStars ammoTypeStars)
    {
        switch (ammoTypeStars)
        {
            case AmmoTypeStars.OneStar: SetStarsSprites(1); break;
            case AmmoTypeStars.TwoStars: SetStarsSprites(2); break;
            case AmmoTypeStars.ThreeStars: SetStarsSprites(3); break;
            case AmmoTypeStars.FourStars: SetStarsSprites(4); break;
            case AmmoTypeStars.FiveStars: SetStarsSprites(5); break;
        }
    }

    private void SetStarsSprites(int length)
    {
        foreach (var star in _stars)
        {
            star.sprite = _starsSprites[1];
        }

        for (int i = 0; i < length; i++)
        {
            _stars[i].sprite = _starsSprites[0];
        }
    }
}
