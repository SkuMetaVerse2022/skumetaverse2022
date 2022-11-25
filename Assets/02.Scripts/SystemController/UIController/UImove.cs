using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UImove : MonoBehaviour
{
    //help는 [0], mission은 [1]
    public GameObject[] Main_ = new GameObject[2]; //부모 창 오브젝트
    public float posx = 940;
    public float r_posx = 1100;
    public GameObject[] openButton = new GameObject[2];
    public GameObject[] closeButton = new GameObject[2];

    public void NavOpen()
    {
        Main_[0].transform.DOMoveX(posx, 0.7f);

        openButton[0].SetActive(false);
        closeButton[0].SetActive(true);

    }

    public void ChatOpen()
    {
        Main_[1].transform.DOMoveX(posx, 0.7f);

        openButton[1].SetActive(false);
        closeButton[1].SetActive(true);

    }

    public void Close()
    {
        for (int i = 0; i < 2; i++)
        {
            Main_[i].transform.DOMoveX(r_posx, 0.7f);
            openButton[i].SetActive(true);
            closeButton[i].SetActive(false);
        }
    }
}
