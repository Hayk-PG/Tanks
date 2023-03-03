using System.Collections;
using UnityEngine;


public class DropBoxItemSelectionPanel : MonoBehaviour
{
    [SerializeField]
    private Tab_DropBoxItemSelection _dropBoxSelectionTab;

    [SerializeField] [Space]
    private BaseDropBoxSelectionPanelElement[] _elements;



    private void OnEnable() => _dropBoxSelectionTab.onDropBoxItemSelectionTabActivity += OnDropBoxItemSelectionTabActivity;

    private void OnDisable() => _dropBoxSelectionTab.onDropBoxItemSelectionTabActivity -= OnDropBoxItemSelectionTabActivity;

    private void OnDropBoxItemSelectionTabActivity(bool isActive)
    {
        if (isActive)
            Execute();
    }

    public void Execute() => StartCoroutine(LoopThroughElements());

    private IEnumerator LoopThroughElements()
    {
        yield return null;

        for (int i = 0; i < _elements.Length; i++)
        {
            if (i == 0)
                continue;

            if(Random.Range(0, 2) < 1 )
            {
                SetElementActivity(false, i);

                continue;
            }

            SetElementActivity(true, i);
        }
    }

    private void SetElementActivity(bool isActive, int elementIndex)
    {
        if (isActive)
        {
            _elements[elementIndex].gameObject.SetActive(true);
            _elements[elementIndex].Activate();
        }
        else
        {
            _elements[elementIndex].gameObject.SetActive(false);
        }
    }
}
