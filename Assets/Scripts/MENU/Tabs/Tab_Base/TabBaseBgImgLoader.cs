using UnityEngine;


//ADDRESSABLE
public class TabBaseBgImgLoader : TabBaseImgLoader<ITabBaseBgImgObserver>, ITabBaseBgImgObserver
{
    protected override void AssignObserversSprites(Sprite[] sprites) => GlobalFunctions.Loop<ITabBaseBgImgObserver>.Foreach(_observers, observer => observer.AssignSprites(_loadedSprites));

    public override void AssignSprites(Sprite[] sprites)
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].sprite = sprites[i];
            _images[i].color = Color.white;
        }
    }
}
