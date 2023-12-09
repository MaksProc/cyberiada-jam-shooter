using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class RenderWithoutCamera : MonoBehaviour
{

    public float val = 0;

    RenderTexture rt;
    public GameObject go;
    public GameObject part;
    Camera camera;
    CommandBuffer cb;

    Mesh bakedMesh;

    void OnEnable()
    {
        RenderTextureFormat format = RenderTextureFormat.Default;


        rt = new RenderTexture((int)300, (int)300, 0, format);
        rt.name = "TVE Colors Tex";

        //camera.enabled = false;
        cb = new CommandBuffer();
        cb.name = "The Vegetatioon Engine Motion";
        cb.ClearRenderTarget(true, true, Color.blue);
        cb.DrawRenderer(go.GetComponent<Renderer>(), go.GetComponent<Renderer>().sharedMaterial, 0, 0);
        //cb.DrawRenderer(part.GetComponent<ParticleSystemRenderer>(), part.GetComponent<ParticleSystemRenderer>().sharedMaterial);

        bakedMesh = new Mesh();
        cb.DrawMesh(bakedMesh, Matrix4x4.identity, part.GetComponent<Renderer>().sharedMaterial);

        Shader.SetGlobalTexture("_GlowMap", rt);
    }

    void LateUpdate()
    {       
        part.GetComponent<ParticleSystemRenderer>().BakeMesh(bakedMesh, true);

        Graphics.SetRenderTarget(rt);

        //Matrix4x4 p = camera.projectionMatrix;
        Matrix4x4 p = Matrix4x4.Ortho(-10, 10, -10, 10, -10000, 10000);

        GL.LoadProjectionMatrix(GL.GetGPUProjectionMatrix(p, true));
        GL.modelview = new Matrix4x4
        (
            new Vector4(1f, 0f, 0f, 0f),
            new Vector4(0f, 0f, -1f, 0f),
            new Vector4(0f, -1f, 0f, 0f),
            new Vector4(0f, 0f, 0f, 1f)
        );
        Graphics.ExecuteCommandBuffer(cb);

    }
}
