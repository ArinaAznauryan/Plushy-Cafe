using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering;

namespace LiquidVolumeFX {

    public class LiquidVolumeDepthPrePassRenderFeature : ScriptableRendererFeature {

        static class ShaderParams {
            public static int RTBackBuffer = Shader.PropertyToID("_VLBackBufferTexture");
            public static int RTFrontBuffer = Shader.PropertyToID("_VLFrontBufferTexture");
            public static int FlaskThickness = Shader.PropertyToID("_FlaskThickness");
            public static int ForcedInvisible = Shader.PropertyToID("_LVForcedInvisible");
            public const string SKW_FP_RENDER_TEXTURE = "LIQUID_VOLUME_FP_RENDER_TEXTURES";
        }

        enum Pass {
            BackBuffer = 0,
            FrontBuffer = 1
        }

        public readonly static List<LiquidVolume> lvBackRenderers = new List<LiquidVolume>();
        public readonly static List<LiquidVolume> lvFrontRenderers = new List<LiquidVolume>();

        public static void AddLiquidToBackRenderers(LiquidVolume lv) {
            if (lv == null || lv.topology != TOPOLOGY.Irregular || lvBackRenderers.Contains(lv)) return;
            lvBackRenderers.Add(lv);
        }

        public static void RemoveLiquidFromBackRenderers(LiquidVolume lv) {
            if (lv == null || !lvBackRenderers.Contains(lv)) return;
            lvBackRenderers.Remove(lv);
        }

        public static void AddLiquidToFrontRenderers(LiquidVolume lv) {
            if (lv == null || lv.topology != TOPOLOGY.Irregular || lvFrontRenderers.Contains(lv)) return;
            lvFrontRenderers.Add(lv);
        }

        public static void RemoveLiquidFromFrontRenderers(LiquidVolume lv) {
            if (lv == null || !lvFrontRenderers.Contains(lv)) return;
            lvFrontRenderers.Remove(lv);
        }

        class DepthPass : ScriptableRenderPass
        {

            const string profilerTag = "LiquidVolumeDepthPrePass";

            Material mat;
            RTHandle targetHandle; // Updated to RTHandle
            int passId;
            List<LiquidVolume> lvRenderers;
            public ScriptableRenderer renderer;
            public bool interleavedRendering;

            public DepthPass(Material mat, Pass pass, RenderPassEvent renderPassEvent)
            {
                this.renderPassEvent = renderPassEvent;
                this.mat = mat;

                switch (pass)
                {
                    case Pass.BackBuffer:
                        targetHandle = RTHandles.Alloc(
                        width: 1, // Specify dimensions; can be updated dynamically
                        height: 1,
                        depthBufferBits: DepthBits.Depth16, // Match your requirements
                        colorFormat: GraphicsFormat.R8G8B8A8_UNorm, // Example format
                        dimension: TextureDimension.Tex2D,
                        name: "_VLBackBufferRT"
                        );

                        passId = (int)Pass.BackBuffer;
                        lvRenderers = lvBackRenderers;

                        break;

                    case Pass.FrontBuffer:
                        targetHandle = RTHandles.Alloc(
                            width: 1, // Specify dimensions; can be updated dynamically
                            height: 1,
                            depthBufferBits: DepthBits.Depth16, // Match your requirements
                            colorFormat: GraphicsFormat.R8G8B8A8_UNorm, // Example format
                            dimension: TextureDimension.Tex2D,
                            name: "_VLFrontBufferRT"
                        );

                        passId = (int)Pass.FrontBuffer;
                        lvRenderers = lvFrontRenderers;

                        break;
            }
            }

            public void Setup(LiquidVolumeDepthPrePassRenderFeature feature, ScriptableRenderer renderer)
            {
                this.renderer = renderer;
                this.interleavedRendering = feature.interleavedRendering;
            }

//            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
//            {
//                cameraTextureDescriptor.colorFormat = LiquidVolume.useFPRenderTextures ? RenderTextureFormat.RHalf : RenderTextureFormat.ARGB32;
//                cameraTextureDescriptor.sRGB = false;
//                cameraTextureDescriptor.depthBufferBits = 16;

//                //targetHandle = RTHandles.Alloc(cameraTextureDescriptor, name: "TargetRTHandle"); // Allocate handle with descriptor
//                targetHandle = RTHandles.Alloc(
//    cameraTextureDescriptor.width,
//    cameraTextureDescriptor.height,
//    colorFormat: cameraTextureDescriptor.colorFormat,
//    depthBufferBits: cameraTextureDescriptor.depthBufferBits,
//    name: "TargetRTHandle"
//);
//                if (!interleavedRendering)
//                {
//                    ConfigureTarget(targetHandle); // Updated to RTHandle
//                }
//            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (lvRenderers == null || lvRenderers.Count == 0) return;

                CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
                try
                {
                    cmd.Clear();
                    cmd.SetGlobalFloat(ShaderParams.ForcedInvisible, 0);

                    // Perform rendering operations here (e.g., DrawRenderer calls)
                    foreach (var lv in lvRenderers)
                    {
                        if (lv != null && lv.isActiveAndEnabled)
                        {
                            cmd.SetGlobalFloat(ShaderParams.FlaskThickness, 1.0f - lv.flaskThickness);
                            cmd.DrawRenderer(lv.mr, mat, lv.subMeshIndex >= 0 ? lv.subMeshIndex : 0, passId);
                        }
                    }

                    context.ExecuteCommandBuffer(cmd); // Execute the commands
                }
                finally
                {
                    CommandBufferPool.Release(cmd); // Always release the CommandBuffer
                }
            }


            public override void FrameCleanup(CommandBuffer cmd)
            {
                if (targetHandle != null)
                {
                    RTHandles.Release(targetHandle); // Release RTHandle properly
                    targetHandle = null;
                }
            }
        }

        [SerializeField, HideInInspector]
        Shader shader;

        public static bool installed;
        Material mat;
        DepthPass backPass, frontPass;

        [Tooltip("Renders each irregular liquid volume completely before rendering the next one.")]
        public bool interleavedRendering;

        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;

        private void OnDestroy() { 
            Shader.SetGlobalFloat(ShaderParams.ForcedInvisible, 0);
            CoreUtils.Destroy(mat);
        }

        public override void Create() {
            name = "Liquid Volume Depth PrePass";
            shader = Shader.Find("LiquidVolume/DepthPrePass");
            if (shader == null) {
                return;
            }
            mat = CoreUtils.CreateEngineMaterial(shader);
            backPass = new DepthPass(mat, Pass.BackBuffer, renderPassEvent);
            frontPass = new DepthPass(mat, Pass.FrontBuffer, renderPassEvent);
        }

        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            installed = true;
            if (backPass != null && lvBackRenderers.Count > 0)
            {
                backPass.Setup(this, renderer);
                renderer.EnqueuePass(backPass);
            }
            if (frontPass != null && lvFrontRenderers.Count > 0)
            {
                frontPass.Setup(this, renderer);
                frontPass.renderer = renderer;
                renderer.EnqueuePass(frontPass);
            }
        }

    }
}
