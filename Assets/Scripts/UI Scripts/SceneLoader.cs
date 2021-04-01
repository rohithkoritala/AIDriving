using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

        public void GotoSelectionScene()
        {
            GlobalVariables.LapComplete = true;
            SceneManager.LoadScene("selection");
        }

        public void GotoMenuScene()
        {
            GlobalVariables.LapComplete = true;
            SceneManager.LoadScene("main menu");
        }

    public void GotoControlScene()
        {
            SceneManager.LoadScene("controls");
        }
        public void GotoRaceScene()
        {
            GlobalVariables.LapComplete = true;
            SceneManager.LoadScene("CarTestScene");
        }
        public void GotoExitScene()
        {
            Application.Quit();
        }

}
