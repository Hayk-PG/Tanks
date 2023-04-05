using UnityEngine;

public class RipplePostProcessor : MonoBehaviour
{
    public Material RippleMaterial;
    public float MaxAmount = 50f;
 
    [Range(0,1)]
    public float Friction = .9f;
 
    private float Amount = 0f;
 
    private void Update()
    {
        this.RippleMaterial.SetFloat("_Amount", this.Amount);

        this.Amount *= this.Friction;

        print(this.Amount);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, this.RippleMaterial);
    }

    public void ApplyRippleFX(Vector3 pos)
    {
        Vector3 ScreenPoint = Camera.main.WorldToScreenPoint(pos);

        this.Amount = this.MaxAmount;
        this.RippleMaterial.SetFloat("_CenterX", ScreenPoint.x);
        this.RippleMaterial.SetFloat("_CenterY", ScreenPoint.y);

        this.RippleMaterial.SetFloat("_Amount", this.Amount);
        this.Amount *= this.Friction;
    }
}
