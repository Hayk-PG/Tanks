using UnityEngine;

public class PlayerFeedbackAnnouncer : MonoBehaviour
{
    public void AnnounceFeedback(int soundListIndex, int clipIndex, bool isPositiveFeedback)
    {
        GameSceneObjectsReferences.GameplayAnnouncer.AnnouncePlayerFeedback(soundListIndex, clipIndex, isPositiveFeedback);
    }
}
