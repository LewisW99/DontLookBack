using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/Retro Render Pipeline")]
public class RetroRenderPipelineAsset : RenderPipelineAsset
{
    public Material blitMaterial;
    public int lowResWidth = 320;
    public int lowResHeight = 180;

    protected override RenderPipeline CreatePipeline()
    {
        return new RetroRenderPipeline(blitMaterial, lowResWidth, lowResHeight);
    }
}
