using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLViewControllerTemp : MonoBehaviour
{
    private MLInput[] MLViews;

    // Start is called before the first frame update
    private void Awake() {
        MLViews = GetComponentsInChildren<MLInput>();
    }
/*
    private void Start() {
        foreach(MLInput input in GetMLViews()) {
            input.GetMLInput();
        }
    }*/
float timer = 1f;

    private void Update() { 
        timer += Time.deltaTime;
        if(timer >= 0.5f){
            foreach(MLInput input in GetMLViews()) {
                input.GetMLInput();
                timer = 0f;
            }
        }
    }

    public MLInput[] GetMLViews() {
        return MLViews;
    }
}
