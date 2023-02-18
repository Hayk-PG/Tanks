﻿using System;
using UnityEngine;


public class TileProps : MonoBehaviour
{
    public enum PropsType { None, MetalCube, MetalGround, ExplosiveBarrels, AALauncher, Mine, All}
    public PropsType _propsType;

    private Tile _tile;

    [SerializeField] [Space]
    private MetalCube _metalCube;

    [SerializeField] [Space]
    private MetalTile _metalGround;

    [SerializeField] [Space]
    private ExplosiveBarrels _explosiveBarrels;

    [SerializeField] [Space]
    private Mine _mine;
   
    public MetalCube MetalCube => _metalCube;
    public MetalTile MetalGround => _metalGround;
    public ExplosiveBarrels ExplosiveBarrels => _explosiveBarrels;
    public Mine Mine => _mine;

    public bool ShouldAvoid { get; private set; }

    public event Action<bool, bool?> onAAProjectileLauncherActivity;



    private void Awake() => _tile = Get<Tile>.From(gameObject);

    private void OnTileProtection(bool isProtected)
    {
        if(_tile != null)
            _tile.IsProtected = isProtected;
    }

    private void SetArmoredCubeActivity(bool isActive) => MetalCube.gameObject.SetActive(isActive);

    private void SetArmoredTileActivity(bool isActive) => MetalGround.gameObject.SetActive(isActive);

    private void SetExplosiveBarrelsActivity(bool isActive) => ExplosiveBarrels.gameObject.SetActive(isActive);

    private void SetAAProjectileLaucnherActivity(bool isActive, bool? isPlayer1)
    {
        onAAProjectileLauncherActivity?.Invoke(isActive, isPlayer1);
    }

    private void SetMineActivity(bool isActive)
    {
        Mine.gameObject.SetActive(isActive);
    }

    public void ActiveProps(PropsType propsType, bool isActive, bool? isPlayer1)
    {
        switch (propsType)
        {
            case PropsType.MetalCube:
                SetArmoredCubeActivity(isActive);

                ShouldAvoid = true;
                break;

            case PropsType.MetalGround:
                SetArmoredTileActivity(isActive);

                ShouldAvoid = false;
                break;

            case PropsType.ExplosiveBarrels:
                SetExplosiveBarrelsActivity(isActive);
                SetArmoredTileActivity(isActive);

                ShouldAvoid = true;
                break;

            case PropsType.AALauncher:
                SetAAProjectileLaucnherActivity(isActive, isPlayer1);
                SetArmoredTileActivity(isActive);

                ShouldAvoid = false;
                break;

            case PropsType.Mine:
                SetMineActivity(isActive);

                ShouldAvoid = true;
                break;

            case PropsType.All:
                SetExplosiveBarrelsActivity(isActive);
                SetAAProjectileLaucnherActivity(isActive, isPlayer1);
                SetArmoredCubeActivity(isActive);
                SetArmoredTileActivity(isActive);
                SetMineActivity(isActive);

                ShouldAvoid = false;
                break;
        }

        _propsType = propsType;

        OnTileProtection(isActive);
    }
}


