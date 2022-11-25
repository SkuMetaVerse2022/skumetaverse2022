using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneUiCtrl : MonoBehaviour
{
    [Header("- 도움말 패널")]
    [SerializeField]
    private GameObject helpPanel;

    [Header("- ESC 패널")]
    [SerializeField]
    private GameObject ESCPanel;

    [Header("- 제어판")]
    [SerializeField]
    private GameObject controllPanel;

    void Start()
    {
        Debug.Log(helpPanel.gameObject.name);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(helpPanel.gameObject.activeSelf == false)
            {
                helpPanel.gameObject.SetActive(true);
            }

            else
            {
                helpPanel.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ESCPanel.gameObject.activeSelf == false)
            {
                ESCPanel.gameObject.SetActive(true);
            }

            else
            {
                ESCPanel.gameObject.SetActive(false);
            }
        }

    }
}
