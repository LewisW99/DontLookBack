using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;

public class RetroRenderPipeline : RenderPipeline
{
    private Material blitMaterial;
    private int width;
    private int height;

    public RetroRenderPipeline(Material blitMat, int lowResWidth, int lowResHeight)
    {
        blitMaterial = blitMat;
        width = lowResWidth;
        height = lowResHeight;
    }

    [Obsolete]
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach (var camera in cameras)
        {
            if (camera == null)
                continue;

            var cmd = CommandBufferPool.Get("Retro Render Pass");

            context.SetupCameraProperties(camera);

            // Allocate low-res RT
            RenderTextureDescriptor lowResDesc = new RenderTextureDescriptor(width, height, RenderTextureFormat.Default);
            lowResDesc.msaaSamples = 1;
            lowResDesc.depthBufferBits = 16;
            lowResDesc.sRGB = true;

            cmd.GetTemporaryRT(Shader.PropertyToID("_LowResTex"), lowResDesc, FilterMode.Point);
            var lowResID = new RenderTargetIdentifier("_LowResTex");

            // Downsample render to low-res texture
            cmd.SetRenderTarget(lowResID);
            cmd.ClearRenderTarget(true, true, Color.black);


   
            // Create RendererList
            var cullingParams = new ScriptableCullingParameters();


            if (!camera.TryGetCullingParameters(out cullingParams))
                continue;

            var cullResults = context.Cull(ref cullingParams);

            ShaderTagId pipelineTag = new ShaderTagId("SRPDefaultUnlit");

            var drawSettings = new DrawingSettings(pipelineTag, new SortingSettings(camera));
            var filterSettings = new FilteringSettings(RenderQueueRange.all);

            context.DrawRenderers(cullResults, ref drawSettings, ref filterSettings);
            var cullingResults = context.Cull(ref cullingParams);
            var rendererListDesc = new RendererListDesc(new ShaderTagId("SRPDefaultUnlit"), cullingResults, camera)
            {
                sortingCriteria = SortingCriteria.CommonOpaque,
                renderQueueRange = RenderQueueRange.all
            };

            var rendererList = context.CreateRendererList(rendererListDesc);

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            cmd.DrawRendererList(rendererList);

            // Blit low-res to screen with point filtering
            cmd.Blit(lowResID, BuiltinRenderTextureType.CameraTarget, blitMaterial);

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            cmd.ReleaseTemporaryRT(Shader.PropertyToID("_LowResTex"));
            context.ExecuteCommandBuffer(cmd);
            context.Submit();

            CommandBufferPool.Release(cmd);
        }
    }
}
