using System;
using UnityEngine;

public enum NewWeaponContent { TornadoShell }

public class WoodBox : MonoBehaviour
{
    private enum Content { AddScore, AddHealth, AddWeapon, GiveTurn, NewWeapon}
    
    [SerializeField] 
    private Content _content;
    [SerializeField]
    private NewWeaponContent _newWeaponContent;

    [SerializeField]
    private WeaponProperties[] _newWeapon;

    private Tab_WoodboxContent _tabWoodboxContent;
    private TurnController _turnController;
    private NewWeaponFromWoodBox _newWeaponFromWoodBox;

    private int _contentIndex;
    private int _newWeaponContentIndex;
    private bool _isTornadoShellTanken;

    public Action<WeaponProperties> OnNewWeaponTaken { get; set; }



    private void Awake()
    {
        _tabWoodboxContent = FindObjectOfType<Tab_WoodboxContent>();
        _turnController = FindObjectOfType<TurnController>();
        _newWeaponFromWoodBox = FindObjectOfType<NewWeaponFromWoodBox>();
    }

    public void OnContent(int contentIndex, int newWeaponContentIndex, TankController tankController)
    {
        _contentIndex = contentIndex < Enum.GetValues(typeof(Content)).Length ? contentIndex : Enum.GetValues(typeof(Content)).Length - 1;
        _content = (Content)_contentIndex;

        switch (_content)
        {
            case Content.AddScore: AddPlayerScore(tankController); break;
            case Content.AddHealth: AddPlayerHealth(tankController); break;
            case Content.AddWeapon: AddWeapon(tankController); break;
            case Content.GiveTurn: GiveTurn(tankController); break;
            case Content.NewWeapon: NewWeapon(newWeaponContentIndex, tankController); break;
        }
    }

    private void AddPlayerScore(TankController tankController)
    {
        ScoreController scoreController = Get<ScoreController>.From(tankController.gameObject);

        if(scoreController != null)
        {
            scoreController.GetScoreFromWoodBox(out bool isDone, out string text);

            if (isDone)
                _tabWoodboxContent.OnContent(Tab_WoodboxContent.Content.Score, text);
        }
    }

    private void AddPlayerHealth(TankController tankController)
    {
        HealthController healthController = Get<HealthController>.From(tankController.gameObject);

        if(healthController != null)
        {
            healthController.GetHealthFromWoodBox(out bool isDone, out string text);

            if (isDone)
                _tabWoodboxContent.OnContent(Tab_WoodboxContent.Content.Health, text);
        }
    }

    private void AddWeapon(TankController tankController)
    {
        PlayerAmmoType playerAmmoType = Get<PlayerAmmoType>.From(tankController.gameObject);

        if(playerAmmoType != null)
        {
            playerAmmoType.GetMoreBulletsFromWoodBox(out bool isDone, out string text);

            if (isDone)
                _tabWoodboxContent.OnContent(Tab_WoodboxContent.Content.Bullet, text);
        }
    }

    private void GiveTurn(TankController tankController)
    {
        PlayerTurn playerTurn = Get<PlayerTurn>.From(tankController.gameObject);

        if(playerTurn != null)
        {
            if (playerTurn.IsMyTurn)
            {
                _turnController.SetNextTurn(playerTurn.MyTurn == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
            }
        }
    }

    private void NewWeapon(int newWeaponContentIndex, TankController tankController)
    {
        _newWeaponContentIndex = _newWeaponContentIndex < Enum.GetValues(typeof(NewWeaponContent)).Length ? newWeaponContentIndex : Enum.GetValues(typeof(NewWeaponContent)).Length - 1;
        _newWeaponContent = (NewWeaponContent)_newWeaponContentIndex;

        if (tankController.BasePlayer != null)
        {
            _newWeaponFromWoodBox.SubscribeToWoodBoxEvent(this);

            switch (_newWeaponContent)
            {
                case NewWeaponContent.TornadoShell:
                    if (!_isTornadoShellTanken)
                    {
                        OnNewWeaponTaken?.Invoke(_newWeapon[_newWeaponContentIndex]);
                        _isTornadoShellTanken = true;
                    }
                    break;
            }
        }
    }
}
