using UnityEngine;
using System;


public class DropBoxSelectionPanelC4 : BaseDropBoxSelectionPanelElement
{
    private Func<TankController, Vector3?> minePosition;


    protected override void Use()
    {
        minePosition = MinePosition;

        _data[0] = minePosition;
        _data[1] = NegativePrice;
        _data[2] = _quantity;

        DropBoxSelectionHandler.RaiseEvent(DropBoxItemType.C4, _data);
    }

    private Vector3? MinePosition(TankController tankController)
    {
        Vector3? minePosition = null;

        float previousDistance = 0;
        float distance = previousDistance;

        foreach (var mine in FindObjectsOfType<Mine>())
        {
            previousDistance = distance;

            if (distance <= previousDistance)
            {
                distance = Vector3.Distance(mine.transform.position, tankController.transform.position);

                minePosition = mine.transform.position;
            }
        }

        return minePosition;
    }
}
