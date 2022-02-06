using UnityEngine;

public class Trees : MonoBehaviour
{
    [SerializeField] GameObject[] _trees;

    int _randomTreeIndex;
    bool _randomActive;


    void Awake()
    {
        _randomTreeIndex = Random.Range(0, _trees.Length);
        _randomActive = Random.Range(0, 5) > 3 ? true : false;

        _trees[_randomTreeIndex].SetActive(_randomActive);
    }
}
