using UnityEngine;

public class Trees : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _trees;
    private GameObject _activeTree;
    private Material _activeTreeMaterial;

    private int _randomTreeIndex;
    private bool _randomActive;

    private float _amplitude, _desiredAmplitude;

    private WindSystemController _windSystemController;


    private void Awake()
    {
        _randomTreeIndex = Random.Range(0, _trees.Length);
        _randomActive = Random.Range(0, 5) > 3 ? true : false;

        _trees[_randomTreeIndex].SetActive(_randomActive);
        _activeTree = _trees[_randomTreeIndex].activeInHierarchy ? _trees[_randomTreeIndex] : null;

        _windSystemController = FindObjectOfType<WindSystemController>();
    }

    private void Start()
    {
        _activeTreeMaterial = _activeTree?.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if(_activeTreeMaterial != null)
        {
            _amplitude = _activeTreeMaterial.GetFloat("_MBAmplitude");
            _desiredAmplitude = Mathf.Abs(_windSystemController.WindForce) * 5;

            _activeTreeMaterial.SetFloat("_MBAmplitude", Mathf.Lerp(_amplitude, _desiredAmplitude, 2.5f * Time.deltaTime));
        }
    }
}
