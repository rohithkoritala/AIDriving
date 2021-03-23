using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MLInputShader : MLView
{
    public Camera AICamera;
    public Material MLInputShaderMaterial;
    public RenderTexture renderTexture;
    private RenderTextureDescriptor OutputDesc;

    private void Awake() {
        InitCamera();
        MLInputShaderMaterial = new Material(Shader.Find("Hidden/DepthSegmentationShader"));

        OutputDesc = new RenderTextureDescriptor(64, 64, RenderTextureFormat.Default, 16);
        renderTexture = new RenderTexture(OutputDesc);

        view.depthTextureMode = DepthTextureMode.DepthNormals;
        view.targetTexture = renderTexture;
    }

    private void OnEnable() {

    }
    
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, MLInputShaderMaterial);
    }
}