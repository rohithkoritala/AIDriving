using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

        public void GotoSelectionScene()
        {
            SceneManager.LoadScene("selection");
        }

        public void GotoMenuScene()
        {
            SceneManager.LoadScene("main menu");
        }

    public void GotoControlScene()
        {
            SceneManager.LoadScene("controls");
        }
        public void GotoRaceScene()
        {
            SceneManager.LoadScene("CarTestScene");
        }
        public void GotoExitScene()
        {
            Application.Quit();
        }

}
