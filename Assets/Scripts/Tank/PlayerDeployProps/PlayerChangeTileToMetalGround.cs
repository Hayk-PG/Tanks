using UnityEngine;

public class PlayerChangeTileToMetalGround : PlayerDeployProps
{
    protected override void Start()
    {
        InitializeRelatedPropsButton(Names.MetalGround);
    }

    protected override void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnChangeToMetalGround -= OnInstantiate;
    }

    protected override void SubscribeToPropsEvent()
    {
        _propsTabCustomization.OnChangeToMetalGround += OnInstantiate;
    }

    protected override bool IsTileFound(float tilePosX, bool isPlayer1, Vector3 transformPosition)
    {
        return isPlayer1 ? tilePosX >= transformPosition.x && tilePosX <= transformPosition.x + 0.5f :
                           tilePosX <= transformPosition.x  && tilePosX >= transformPosition.x - 0.5f;
    }

    protected override void ActivateTileProps(TileProps tileProp, bool isPlayer1)
    {
        tileProp.ActiveProps(global::TileProps.PropsType.MetalGround, true, isPlayer1);
    }

    protected override void Result(bool isPlayer1, Vector3 transformPosition, Vector3 tilePosition)
    {
        _iPlayerDeployProps.ChangeGroundToArmoredGround(isPlayer1, transformPosition, tilePosition);
    }
}
