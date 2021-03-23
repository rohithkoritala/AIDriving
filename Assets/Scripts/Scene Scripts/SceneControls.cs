using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControls : MonoBehaviour //this class allows scene to be reset and closed
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp("r")){ //reset current scene
            GlobalVariables.LapComplete = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetKeyUp("escape")){ //quit
            Application.Quit();
        }
    }
}
