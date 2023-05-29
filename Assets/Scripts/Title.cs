using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    void Update()
    {

    }
    public void PlayPressed()
    {
        SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
