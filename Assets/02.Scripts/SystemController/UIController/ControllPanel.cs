using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControllPanel : MonoBehaviourPun
{
    [Header("- 제어판 패널")]
    [SerializeField]
    private GameObject controllPanel;

    [Header("- agora 영상 패널")]
    [SerializeField]
    private GameObject agoraPanel;

    [Header("- agora 버튼 패널")]
    [SerializeField]
    private GameObject agoraButtonPanel;

    [Header("- Chatting UI")]
    [SerializeField]
    private GameObject chatPanel;

    [Header("- MiniMap 패널")]
    [SerializeField]
    private GameObject minimapPanel;

    [Header("- Help 패널")]
    [SerializeField]
    private GameObject helpPanel;

    [Header("- 기타 각종 패널")]
    public GameObject adminBtn;

    [Header("- 아고라 마이크 제어")]
    [SerializeField]
    private GameObject agoraMicOn;
    [SerializeField]
    private GameObject agoraMicOff;

    private PhotonView pv;

    private void Start()
    {   
        //UI 주인을 찾기 위한 컴포넌트 저장
        pv = GetComponent<PhotonView>();
        helpPanel = GameObject.Find("HelpPanel");
    }

    private void Update()
    {
        CtrlPanel();
        AgoraPanel();
        ChatPanel();
        MiniMapPanel();
        HelpPanel();
    }

    //UI노출 여부를 제어하는 코드
    private void CtrlPanel()
    {
        //내꺼만 제어한다.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (controllPanel.gameObject.activeSelf == false)
            {
                controllPanel.gameObject.SetActive(true);
                //adminBtn.gameObject.SetActive(true);    
            }
            else
            {
                //adminBtn.gameObject.SetActive(false);
                controllPanel.gameObject.SetActive(false);
            }
        }

    }

    private void ChatPanel()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (chatPanel.gameObject.activeSelf == true)
                chatPanel.gameObject.SetActive(false);
            else
                chatPanel.gameObject.SetActive(true);
        }

    }


    private void AgoraPanel()
    {
        //내꺼가 아니면 보여주지 않는다.

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (agoraPanel.gameObject.activeSelf == false)
            {
                agoraPanel.gameObject.SetActive(true);
                agoraButtonPanel.gameObject.SetActive(true);
            }
            else
            {
                agoraPanel.gameObject.SetActive(false);
                agoraButtonPanel.gameObject.SetActive(false);
            }
        }

    }

    private void MiniMapPanel()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (minimapPanel.gameObject.activeSelf == true)
                minimapPanel.gameObject.SetActive(false);
            else
                minimapPanel.gameObject.SetActive(true);
        }
    }

    private void HelpPanel()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (helpPanel.gameObject.activeSelf == false)
            {
                helpPanel.gameObject.SetActive(true);
            }

            else
            {
                helpPanel.gameObject.SetActive(false);
            }
        }
    }
}
