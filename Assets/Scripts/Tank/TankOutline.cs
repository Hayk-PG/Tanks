using UnityEngine;


public class TankOutline : MonoBehaviour
{
    private Outline _outline;


    private void Awake()
    {
        GlobalFunctions.Loop<MeshRenderer>.Foreach(transform.parent.GetComponentsInChildren<MeshRenderer>(), meshRenderer => 
        {
            _outline = meshRenderer.gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineWidth = 1;
            _outline.OutlineColor = Color.white;
        });
    }
}
