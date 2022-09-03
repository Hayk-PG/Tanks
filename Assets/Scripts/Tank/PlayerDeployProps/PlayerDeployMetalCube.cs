using UnityEngine;

public class PlayerDeployMetalCube : PlayerDeployProps
{
    protected override void Start()
    {
        InitializeRelatedPropsButton(Names.MetalCube);
    }

    protected override void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnInstantiateMetalCube -= OnInstantiate;
    }

    protected override void SubscribeToPropsEvent()
    {
        _propsTabCustomization.OnInstantiateMetalCube += OnInstantiate;
    }

    protected override void ActivateTileProps(TileProps tileProp, bool isPlayer1)
    {
        tileProp.ActiveProps(global::TileProps.PropsType.MetalCube,true, isPlayer1);
    }

    protected override void Result(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _iPlayerDeployProps?.ArmoredCubeTileProps(isPlayer1, transformPosition, tilePosition);
    }
}
