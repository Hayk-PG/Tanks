using UnityEngine;

//ADDRESSABLE
public class TabBaseBgFxImgLoader : TabBaseImgLoader<ITabBaseBgFxImgObserver>, ITabBaseBgFxImgObserver
{
    protected override void AssignObserversSprites(Sprite[] sprites) => GlobalFunctions.Loop<ITabBaseBgFxImgObserver>.Foreach(_observers, observer => observer.AssignSprites(_loadedSprites));
}
