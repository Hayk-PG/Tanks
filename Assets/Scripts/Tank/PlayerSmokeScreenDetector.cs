using UnityEngine;

public class PlayerSmokeScreenDetector : MonoBehaviour
{
    public void DetectPlayerSmokeScreenImpact(bool isImpacted)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => SetCamerasEnable(!isImpacted), () => CallPhotonNetworkSmokeScreenDetector(isImpacted));
    }

    private void CallPhotonNetworkSmokeScreenDetector(bool isImpacted)
    {
        GameSceneObjectsReferences.PhotonNetworkSmokeScreenDetector.DetectPlayerSmokeScreenImpact(gameObject.name, isImpacted);
    }

    public void SetCamerasEnable(bool isEnable)
    {
        MyCameras.Cameras[1].enabled = isEnable;
        MyCameras.Cameras[2].enabled = isEnable;
    }
}
