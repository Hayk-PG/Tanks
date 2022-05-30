using UnityEngine;

public class InitializeAITankButton : BaseInitializeTankButton
{
    public override void Properties(BaseSelectTankButton bstb)
    {
        bstb._textTankName.text = bstb._data.AvailableAITanks[bstb._index]._tankName;
        bstb._textPlayerLevel.text = "";
        bstb._stars.Display(bstb._data.AvailableAITanks[bstb._index]._starsCount);
    }

    public override void Icon(BaseSelectTankButton bstb)
    {
        bstb._iconTank.sprite = bstb._data.AvailableAITanks[bstb._index]._iconTank;
    }
}
