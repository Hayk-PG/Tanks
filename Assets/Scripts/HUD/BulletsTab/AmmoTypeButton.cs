using System;
using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeButton : MonoBehaviour
{
    public AmmoStars _ammoStars;

    [Serializable]
    public struct Properties
    {
        internal Button _button;
        public Image _buttonImage, _iconImage;
        public Text _titleText;
        public Text _pointsToUnlockText;

        public Sprite ButtonSprite
        {
            get => _buttonImage.sprite;
            set => _buttonImage.sprite = value;
        }
        public Sprite IconSprite
        {
            get => _iconImage.sprite;
            set => _iconImage.sprite = value;
        }
        public string Title
        {
            get => _titleText.text;
            set => _titleText.text = value;
        }
        public string PointsToUnlock
        {
            get => _pointsToUnlockText.text;
            set => _pointsToUnlockText.text = value;
        }

        public int Index { get; set; }
        public int UnlockPoints { get; set; }
        public int PlayerPoints { get; set; }

        public bool IsClicked { get; set; }
        public bool ButtonInteractability
        {
            get
            {
                if (_button != null) return _button.interactable;
                else return false;
            }
            set
            {
                if (_button != null) _button.interactable = value;
            }
        }
        public bool CanUseIt { get; set; }
    }
    public Properties _properties;

    public event Action<AmmoTypeButton> OnClickAmmoTypeButton;



    private void Awake()
    {
        _properties._button = GetComponent<Button>();
        _properties.CanUseIt = true;
    }

    public virtual void OnClickButton()
    {
        OnClickAmmoTypeButton?.Invoke(this);
    }   

    public virtual void DisplayPointsToUnlock(int playerPoints)
    {
        _properties.PlayerPoints = playerPoints;

        Conditions<int>.Compare(_properties.PlayerPoints, _properties.UnlockPoints, OnPointsEquals, OnPlayerPointsGreater, OnPlayerPointsLesser);
    }

    protected virtual void OnPointsEquals()
    {
        Interactability();
        _properties.PointsToUnlock = "<color=#FFFFFF>" + _properties.PlayerPoints + "</color>" + "<color=#FFFFFF>" + "/" + _properties.UnlockPoints + "</color>";
    }

    protected virtual void OnPlayerPointsGreater()
    {
        Interactability();
        _properties.PointsToUnlock = "<color=#FFFFFF>" + _properties.PlayerPoints + "</color>" + "<color=#FFFFFF>" + "/" + _properties.UnlockPoints + "</color>";
    }

    protected virtual void OnPlayerPointsLesser()
    {
        _properties.ButtonInteractability = false;
        _properties.PointsToUnlock = "<color=#EB3817>" + _properties.PlayerPoints + "</color>" + "<color=#FFFFFF>" + "/" + _properties.UnlockPoints + "</color>";
    }

    protected virtual void Interactability()
    {
        if(_properties.CanUseIt) _properties.ButtonInteractability = true;
    }
}
