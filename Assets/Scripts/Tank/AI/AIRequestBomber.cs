using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRequestBomber : AIActionPlanner
{
    private List<AmmoTypeButton> _ammoTypeButtons = new List<AmmoTypeButton>();
    private AmmoTypeButton _airSupportCorespondentButton;
    private AirSupport _airSupport;
    private IScore _iScore;
    private Transform _player;
    private Bomber _bomber;
    private bool _isBomberCalled;
    private bool _hasBombDropped;

    private bool _canUseAirSupport = true;
    private bool CanDropBomb
    {
        get
        {
            return _isBomberCalled && _bomber != null && !_hasBombDropped;
        }
    }
    private bool IsTimeToDropBomb
    {
        get
        {
            return _bomber.transform.position.x <= _player.position.x + 0.1f && _bomber.transform.position.x >= _player.position.x - 0.1f;
        }
    }


    protected override void Awake()
    {
        base.Awake();
        _airSupport = FindObjectOfType<AirSupport>();
        _iScore = Get<IScore>.From(gameObject);
        _player = GlobalFunctions.ObjectsOfType<TankController>.Find(player => player.BasePlayer != null).transform;
    }

    protected override void OnEnable()
    {

    }

    protected override void OnDisable()
    {

    }

    private void Start()
    {
        CacheAmmoTypeButtons();
    }

    private void Update()
    {
        if (CanDropBomb && IsTimeToDropBomb)
        {
            _bomber.DropBomb(_iScore);
            _hasBombDropped = true;
        }
    }

    private void CacheAmmoTypeButtons()
    {
        GlobalFunctions.Loop<AmmoTypeButton>.Foreach(FindObjectsOfType<AmmoTypeButton>(), ammoTypeButton =>
        {
            if (ammoTypeButton._properties._buttonType == ButtonType.Props || ammoTypeButton._properties._buttonType == ButtonType.Support)
            {
                _ammoTypeButtons.Add(ammoTypeButton);

                if (ammoTypeButton._properties.SupportOrPropsType == Names.AirSupport)
                    _airSupportCorespondentButton = ammoTypeButton;
            }
        });
    }

    private IEnumerator RunAmmoTypeButtonTimer(AmmoTypeButton ammoTypeButton)
    {
        _canUseAirSupport = false;
        float unlockTime = (ammoTypeButton._properties.Minutes * 60) + ammoTypeButton._properties.Seconds;

        while (unlockTime > 0)
        {
            unlockTime--;

            if (unlockTime <= 0)
                _canUseAirSupport = true;

            yield return new WaitForSeconds(1);
        }
    }

    public void CallBomber(out bool canCallBomber)
    {
        canCallBomber = _airSupportCorespondentButton != null && _iScore.Score >= _airSupportCorespondentButton._properties.RequiredScoreAmmount && _canUseAirSupport;

        if (canCallBomber)
        {
            _airSupport.Call(out Bomber bomber, _playerTurn);
            _bomber = bomber;
            _isBomberCalled = true;
            StartCoroutine(RunAmmoTypeButtonTimer(_airSupportCorespondentButton));
        }
    }
}
