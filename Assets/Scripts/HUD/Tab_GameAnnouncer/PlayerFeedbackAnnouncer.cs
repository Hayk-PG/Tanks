using UnityEngine;

public class PlayerFeedbackAnnouncer : MonoBehaviour
{
    public void AnnounceFeedback(int soundListIndex, int clipIndex)
    {
        GameSceneObjectsReferences.GameplayAnnouncer.AnnouncePlayerFeedback(soundListIndex, clipIndex);
    }
}
