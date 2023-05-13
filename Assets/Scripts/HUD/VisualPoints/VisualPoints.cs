using UnityEngine;
using TMPro;

public class VisualPoints : MonoBehaviour
{
    //-----UI-----

    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private TMP_Text _txt;

    //-----MOVEMENT-----

    private Vector3 _targetPosition;
     
    private float _distanceToTarget;

    private bool _isActive;





    private void Update() => MoveTowardsTarget();

    public void Initialize(string txt, Vector3 initialPozition, Vector3 targetPosition)
    {
        _txt.text = txt;

        transform.position = initialPozition;

        _targetPosition = targetPosition;

        _distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

        SetActive(true);

        SecondarySoundController.PlaySound(0, 3);
    }

    private void MoveTowardsTarget()
    {
        bool canMoveTowardsTarget = _isActive;

        if (!canMoveTowardsTarget)
            return;

        UpdateSelfPosition();

        UpdateAlpha();

        bool hasReachedDestination = _canvasGroup.alpha <= 0.1f;

        if (hasReachedDestination)
            SetActive(false);
    }

    private void UpdateSelfPosition() => transform.position = Vector3.Lerp(transform.position, _targetPosition, 5f * Time.deltaTime);

    private void UpdateAlpha()
    {
        _canvasGroup.alpha = Mathf.InverseLerp(0, _distanceToTarget, Vector3.Distance(transform.position, _targetPosition));
    }

    private void SetActive(bool isActive)
    {
        _isActive = isActive;

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, _isActive);
    }
}
