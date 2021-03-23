using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showcase : MonoBehaviour
{
    public GameObject car;
    public int x;
    public int y;
    public int z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        car.transform.Rotate(x, y, z);
    }
}
