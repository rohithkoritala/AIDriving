using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLViewController : MonoBehaviour
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

    public MLInput[] GetMLViews() {
        return MLViews;
    }
}
