using System.Collections;
using UnityEngine;

public class Shields : MonoBehaviour
{
    private GameObject[] _shields = new GameObject[2];
    private IEnumerator _coroutine;
    private int _s;


    private void Awake()
    {
        _shields[0] = transform.GetChild(0).gameObject;
        _shields[1] = transform.GetChild(1).gameObject;
    }

    public void Activity(int index, bool isActive)
    {
        _shields[index].SetActive(isActive);
    }

    public void RunTimerCoroutine()
    {
        Conditions<bool>.Compare(_coroutine != null, delegate { StopCor(); StartCor(); }, StartCor);
    }

    private void StopCor()
    {
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private void StartCor()
    {
        _coroutine = TimerCoroutine(true);
        StartCoroutine(_coroutine);
    }

    private IEnumerator TimerCoroutine(bool isActive)
    {
        bool _isActive = isActive;
        _s = 10;

        while (_isActive)
        {
            if (_s > 0) _s--;
            else
            {
                _s = 0;
                _isActive = false;
                GlobalFunctions.Loop<GameObject>.Foreach(_shields, shield => { shield.SetActive(_isActive); });  
            }

            yield return new WaitForSeconds(1);
        }
    }
}
