using UnityEngine;

public class RemoteControlAimRay : MonoBehaviour
{
    [SerializeField] private RectTransform _canvasRectransform;
    private RectTransform _rectTransform;
    private Animator _animator;
    private Ray _ray;
    private Camera _mainCamera;
    private bool _isPlayingAnimation;
    
    


    private void Awake()
    {
        _rectTransform = Get<RectTransform>.From(gameObject);
        _animator = Get<Animator>.From(gameObject);
        _mainCamera = Camera.main;      
    }

    private void Update()
    {
        //_ray = CameraPoint.ScreenPointToRay(_mainCamera, Input.mousePosition);

        //if (Physics.Raycast(_ray, out RaycastHit hit) && Input.GetMouseButton(0) && Input.GetAxis("Mouse X") < 0.1f && Input.GetAxis("Mouse X") > -0.1f && Input.GetAxis("Mouse Y") < 0.1f && Input.GetAxis("Mouse Y") > -0.1f)
        //{
        //    _rectTransform.anchoredPosition = Input.mousePosition / _canvasRectransform.localScale.x;
        //    _isPlayingAnimation = true;
        //}
        //else if (!Input.GetMouseButton(0) || Input.GetAxis("Mouse X") > 0.1f || Input.GetAxis("Mouse X") < -0.1f || Input.GetAxis("Mouse Y") > 0.1f || Input.GetAxis("Mouse Y") < -0.1f)
        //{
        //    _isPlayingAnimation = false;
        //}

        //_animator.SetBool("isPlaying", _isPlayingAnimation);
    }
}
