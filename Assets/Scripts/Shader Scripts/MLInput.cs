using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public interface MLInput
{
    IEnumerable<float> GetMLInput();
}

public class MLView : MonoBehaviour, MLInput {
    protected Camera view = null;
    public Texture2D viewTexture;
    private bool instantiated = false;
    
    protected void InitCamera() {
        view = this.GetComponent<Camera>();   
    }

    public virtual void Start() {
        viewTexture = new Texture2D(view.targetTexture.height, view.targetTexture.width, TextureFormat.RGBA32, false);
        StartCoroutine("ReadPixelsRoutine");
    } 
    float time = 1f;
    public virtual void Update() {
        if(time > 0.2f) {
            view.Render();
            time = 0f;
        }

        time += Time.deltaTime;
    }

    public virtual IEnumerable<float> GetMLInput() {RenderTexture.active = view.targetTexture;
        //Graphics.CopyTexture(view.targetTexture, viewTexture);
        //viewTexture.ReadPixels(new Rect(0, 0, view.targetTexture.width, view.targetTexture.height), 0, 0);

        /*IEnumerable<float> floatArray = viewTexture.GetPixels().Select(x => new float[] { x.r, x.g, x.b }).Flatten();

        return floatArray;*/
        float[] colorFloats = new float[64 * 64];

        if(instantiated) {
            Color[] pixels = viewTexture.GetPixels();

            for(int i = 0; i < pixels.Length; i++) {
                colorFloats[i] = pixels[i].r;
            }
        }

        return colorFloats;
    }

    IEnumerator ReadPixelsRoutine() {
        AsyncGPUReadback.Request(view.targetTexture, default, ReadPixelsCallback);
        
        float time = 0f;

        while(true) {
            while(time < 0.2f) {
                time += Time.deltaTime;
                yield return null;
            }
            
            AsyncGPUReadback.Request(view.targetTexture, default, ReadPixelsCallback);
            time = 0;
        }
    }

    void ReadPixelsCallback(AsyncGPUReadbackRequest requestObject) {
        if(viewTexture != null) {
            viewTexture.LoadRawTextureData(requestObject.GetData<uint>());
            viewTexture.Apply();
        }
        
        instantiated = true;
    }
}

public static class ColorExtension
{
    public static IEnumerable<float> Flatten(this IEnumerable<float[]> floatArray2D)
    {
        foreach (var i in floatArray2D)
        {
            foreach (var j in i)
            {
                yield return j;
            }
        }
    }

    public static IEnumerable<float> Flatten(this IEnumerable<IEnumerable<float>> floatArray2D)
    {
        foreach (var i in floatArray2D)
        {
            foreach (var j in i)
            {
                yield return j;
            }
        }
    }
}

public abstract class MLCarState : MonoBehaviour, MLInput {
    protected Rigidbody state;

    protected void InitState() {
        state = this.GetComponent<Rigidbody>();
    }

    public IEnumerable<float> GetMLInput() {
        List<float> stateData = new List<float>();
        IEnumerable<float> stateDataEnum;

        Vector3 localVelocityDirection = state.transform.InverseTransformDirection(state.velocity);

        stateData.Add(state.mass);
        stateData.Add(localVelocityDirection.x);
        stateData.Add(localVelocityDirection.y);
        stateData.Add(localVelocityDirection.z);

        stateDataEnum = stateData;

        return stateDataEnum;
    }
}