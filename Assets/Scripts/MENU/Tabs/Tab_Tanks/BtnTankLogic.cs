using UnityEngine;

public class BtnTankLogic : MonoBehaviour
{
    public void Select(Btn_Tank btnTank)
    {
        if (Data.Manager.SelectedTankIndex == btnTank.RelaedTankIndex)
            print(Data.Manager.SelectedTankIndex);
    }
}
