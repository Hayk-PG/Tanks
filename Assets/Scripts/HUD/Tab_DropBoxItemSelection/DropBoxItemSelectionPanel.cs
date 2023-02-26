using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class DropBoxItemSelectionPanel : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;

    [SerializeField] [Space]
    private BaseDropBoxSelectionPanelElement[] _elements;




    private void Awake()
    {
        for (int i = 0; i < _elements.Length; i++)
        {
            if (Random.Range(0, 2) < 1)
            {
                SetElementActivity(false, i);

                continue;
            }

            SetElementActivity(true, i);
        }
    }

    public void Execute()
    {
        StartCoroutine(LoopThroughElements());
    }

    private IEnumerator LoopThroughElements()
    {
        yield return null;

        for (int i = 0; i < _elements.Length; i++)
        {
            if(Random.Range(0, 2) < 1)
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
