using System.Collections;
using UnityEngine;

public enum BomberType { Light, Medium, Heavy, Nuke }


public class Bomber : MonoBehaviour
{
    [SerializeField] [Space]
    private Transform _bombSpwnPoint;

    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private BombController _bombPrefab;

    [SerializeField] [Space]
    private BomberAddressable _bomberAddressable;

    [SerializeField] [Space]
    private Camera _cameraAirBomber;

    [SerializeField] [Space]
    private AirBomberCameraAnimationController _airBombCameraAnimationController;

    [SerializeField] [Space]
    private ExternalSoundSource _externalSoundSource;

    private bool _isCameraBlurred;
    private bool _isExternalSoundSourceFadeOut;


    public IScore OwnerScore { get; set; }

    public PlayerTurn OwnerTurn { get; set; }

    public Vector3 DropPoint { get; set; }

    public float MinX { get; set; }
    public float MaxX { get; set; }

    private bool IsOutOfBoundaries
    {
        get => _rigidbody.position.x < MinX || _rigidbody.position.x > MaxX;
    }
    private bool IsBombDropped { get; set; }





    private void OnEnable()
    {
        _bomberAddressable.LoadMeshes();

        ResetCameraAirBomberSettings();

        ResetExternalSoundSourceFadeOut();

        StartCoroutine(FadeInExternalSoundSource());

        PlayerAirBomberCameraAnimation("Transition");   
    }

    private void FixedUpdate()
    {
        DropBomb();

        Conditions<bool>.Compare(IsOutOfBoundaries, Deactivate, null);
    }

    private void ResetCameraAirBomberSettings()
    {
        _cameraAirBomber.orthographicSize = 1;

        _isCameraBlurred = false;
    }

    private void ResetExternalSoundSourceFadeOut() => _isExternalSoundSourceFadeOut = false;

    private IEnumerator FadeInExternalSoundSource()
    {
        float volume = _externalSoundSource.Volume = 0;

        while (volume < 1)
        {
            volume += 2 * Time.deltaTime;

            _externalSoundSource.Volume = volume;

            yield return null;
        }

        _externalSoundSource.Volume = 1;
    }

    private void FadeOutExternalSoundSource()
    {
        if (!_isExternalSoundSourceFadeOut)
        {
            StartCoroutine(FadeOutExternalSoundSourceCoroutine());

            _isExternalSoundSourceFadeOut = true;
        }
    }

    private IEnumerator FadeOutExternalSoundSourceCoroutine()
    {
        float volume = _externalSoundSource.Volume;

        while(volume > 0)
        {
            volume -= 2 * Time.deltaTime;

            _externalSoundSource.Volume = volume;

            yield return null;
        }

        _externalSoundSource.Volume = 0;
    }

    public void DropBomb()
    {
        if (!IsBombDropped)
        {
            bool isPreparingToDrop = _rigidbody.position.x >= DropPoint.x - 1f && _rigidbody.position.x <= DropPoint.x + 1f;
            bool IsWithinXRangeOfDropPoint = _rigidbody.position.x >= DropPoint.x - 0.1f && _rigidbody.position.x <= DropPoint.x + 0.1f;

            if (isPreparingToDrop)
            {
                ZoomInCameraSmoothly();

                BlurCamera();
            }

            if (IsWithinXRangeOfDropPoint)
            {
                SpawnBomb();

                FadeOutExternalSoundSource();
            }
        }
    }

    private void ZoomInCameraSmoothly() => _cameraAirBomber.orthographicSize = Mathf.Lerp(_cameraAirBomber.orthographicSize, 0.5f, 3 * Time.deltaTime);

    private void SpawnBomb()
    {
        BaseBulletController bomb = Instantiate(_bombPrefab, _bombSpwnPoint.position, Quaternion.identity);

        GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController = bomb;

        bomb.OwnerScore = OwnerScore;

        IsBombDropped = true;
    }

    private void BlurCamera()
    {
        if (!_isCameraBlurred)
        {
            PlayerAirBomberCameraAnimation("Blur", 1);

            _isCameraBlurred = true;
        }
    }

    private void PlayerAirBomberCameraAnimation(string stateName = "", int layer = 0)
    {
        if (!_airBombCameraAnimationController.gameObject.activeInHierarchy)
            _airBombCameraAnimationController.gameObject.SetActive(true);

        _airBombCameraAnimationController.PlayAnimation(stateName, layer);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

        IsBombDropped = false;
    }
}
