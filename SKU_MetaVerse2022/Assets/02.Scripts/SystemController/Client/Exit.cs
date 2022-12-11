using Photon.Pun.Demo.Cockpit.Forms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void OnExitButtonClick()
    {
        Application.Quit();
        Debug.Log("Application Quit has worked");
    }
}