using System.Collections;
using UnityEngine;

public abstract class AISupportAndProps : MonoBehaviour
{
    protected AmmoTypeButton _relatedTypeButton;
    protected IScore _iScore;
    protected PlayerTurn _playerTurn;
    protected Transform _player;
    protected bool _canUseAgain = true;
    protected virtual bool IsAllowedToUse
    {
        get
        {
            return _relatedTypeButton != null && _iScore.Score >= _relatedTypeButton._properties.RequiredScoreAmmount && _canUseAgain;
        }
    }


    protected virtual void Awake()
    {
        _iScore = Get<IScore>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _player = GlobalFunctions.ObjectsOfType<TankController>.Find(player => player.BasePlayer != null).transform;
    }

    protected virtual void CacheRelatedTypeButton(string typeName)
    {
        GlobalFunctions.Loop<AmmoTypeButton>.Foreach(FindObjectsOfType<AmmoTypeButton>(), ammoTypeButton =>
        {
            if (ammoTypeButton._properties.SupportOrPropsType == typeName)
                _relatedTypeButton = ammoTypeButton;
        });
    }

    protected virtual IEnumerator RunAmmoTypeButtonTimer(AmmoTypeButton ammoTypeButton)
    {
        _canUseAgain = false;

        float unlockTime = (ammoTypeButton._properties.Minutes * 60) + ammoTypeButton._properties.Seconds;

        while (unlockTime >= 0)
        {
            unlockTime--;

            if (unlockTime == 0)
                _canUseAgain = true;

            yield return new WaitForSeconds(1);
        }
    }

    protected abstract void OnUse();

    public virtual void Use(out bool isAllowedToUse)
    {
        isAllowedToUse = IsAllowedToUse;

        if (isAllowedToUse)
        {
            OnUse();
            StartCoroutine(RunAmmoTypeButtonTimer(_relatedTypeButton));
        }
    }    
}
