using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarUI : MonoBehaviour
{
    private RawImage DepthImage, SegmentImage;
    private MLInputShader DepthShader;
    private MonoBehaviour SegmentationShader;
    private Material material; 

    MLInputShader depthShader;

    // Start is called before the first frame update
    void Awake() {

        DepthImage = GameObject.Find("DepthImage").GetComponent<RawImage>();
        SegmentImage = GameObject.Find("SegmentImage").GetComponent<RawImage>();
    }

    private void Start() {
        DepthImage.texture = GameObject.Find("DepthCamera").GetComponent<Camera>().targetTexture;
        if(SegmentImage == null) Debug.Log("ERROR!");
        SegmentImage.texture = GameObject.Find("SegmentationCamera").GetComponent<Camera>().targetTexture;
    }

    /*private void OnApplicationQuit() {
        Debug.Log("Avg FPS: " + Time.frameCount / Time.time);
    }*/
}
