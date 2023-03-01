using UnityEngine;

public class DropBoxSelectionPanelScores : BaseDropBoxSelectionPanelElement
{
    public event System.Action<int> onScores;


    protected override void Use()
    {
        int scores = Random.Range(1, 51);

        onScores?.Invoke(scores * 100);
    }
}
