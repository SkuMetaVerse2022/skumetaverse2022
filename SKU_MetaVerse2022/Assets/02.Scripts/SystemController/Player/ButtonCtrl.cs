using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    //버튼
    public Button HiBtn;
    public Button ClapBtn;

    //애니메이션 변수
    public Animator anim;

    //버튼 On/off변수
    public GameObject uiobj;

    
    public GameObject pdfCtrlBtn;
    
    void Start()
    {
       
    }

    void Update()
    {
        
    }

    public void HiOnclickBtn()
    {
        anim.SetTrigger("doHello");
    }

    public void ClapOnclickBtn()
    {
        anim.SetTrigger("doClap");
    }

    public void BtnOnOff()
    {
        if(uiobj.activeSelf == false)
        {
            uiobj.SetActive(true);
        }
        else
        {
            uiobj.SetActive(false);
        }
        
        
    }

    public void PdfBtnOnOff()
    {
        pdfCtrlBtn.SetActive(!pdfCtrlBtn.active);
    }
}
