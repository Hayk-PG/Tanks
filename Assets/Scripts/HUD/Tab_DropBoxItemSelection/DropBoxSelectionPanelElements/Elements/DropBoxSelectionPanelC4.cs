using UnityEngine;
using System;


public class DropBoxSelectionPanelC4 : BaseDropBoxSelectionPanelElement
{
    public event Action<Func<TankController, Vector3>, int> onC4;


    protected override void Use()
    {
        onC4?.Invoke(MinePosition, NegativePrice);

        CanUse = false;
    }

    private Vector3 MinePosition(TankController tankController)
    {
        Vector3 minePosition = Vector3.zero;

        float previousDistance = 0;
        float distance = 0;

        foreach (var mine in FindObjectsOfType<Mine>())
        {
            previousDistance = distance;

            if (distance < previousDistance)
            {
                distance = Vector3.Distance(mine.transform.position, tankController.transform.position);

                minePosition = mine.transform.position;
            }
        }

        return minePosition;
    }
}
