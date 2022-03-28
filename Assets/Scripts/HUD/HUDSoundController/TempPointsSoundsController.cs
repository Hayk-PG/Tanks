
public class TempPointsSoundsController : UIBaseSoundController
{
    private TempPoints _tempPoints;


    private void Awake()
    {
        _tempPoints = FindObjectOfType<TempPoints>();
    }

    private void OnEnable()
    {
        if (_tempPoints != null) _tempPoints.OnTempPointsMotionSoundFX += OnTempPointsMotionSoundFX;
        if (_tempPoints != null) _tempPoints.OnScoreTextUpdated += OnTempPointsReachedSoundFX;
    }

    private void OnDisable()
    {
        if (_tempPoints != null) _tempPoints.OnTempPointsMotionSoundFX -= OnTempPointsMotionSoundFX;
        if (_tempPoints != null) _tempPoints.OnScoreTextUpdated += OnTempPointsReachedSoundFX;
    }

    private void OnTempPointsMotionSoundFX()
    {
        PlaySoundFX(0);
    }

    private void OnTempPointsReachedSoundFX(int score)
    {
        PlaySoundFX(1);
    }
}
