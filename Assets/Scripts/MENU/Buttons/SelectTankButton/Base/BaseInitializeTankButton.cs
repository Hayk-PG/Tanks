using System;
using UnityEngine;

public class BaseInitializeTankButton : MonoBehaviour
{
    public virtual void Properties(BaseSelectTankButton bstb)
    {
        bstb._textTankName.text = bstb._data.AvailableTanks[bstb._index]._tankName;
        bstb._textPlayerLevel.text = "Lvl " + bstb._data.AvailableTanks[bstb._index]._availableInLevel;
        bstb._stars.Display(bstb._data.AvailableTanks[bstb._index]._starsCount);
    }

    public virtual void Icon(BaseSelectTankButton bstb)
    {
        bstb._iconTank.sprite = bstb._data.AvailableTanks[bstb._index]._iconTank;
    }
}
