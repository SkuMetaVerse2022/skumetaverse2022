using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

public class ChatManager : MonoBehaviourPunCallbacks
{
    /*
    public List<string> chatList = new List<string>();
    public Button sendButton;
    public TMP_Text[] chatLog;
    public TMP_Text chattingList;
    public TMP_InputField input;
    public ScrollRect scroll_rect;
    private string chatters;

    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
        }
        else
        {
            Debug.Log(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        ChatterUpdate();
        if (Input.GetKeyDown(KeyCode.Return) && !input.isFocused)
            SendButtonOnClicked();

        
    }

    public void SendButtonOnClicked()
    {
        if (input.text.Equals(""))
        {
            Debug.Log("Empty");
            return;
        }
        string msg = $"[{PhotonNetwork.LocalPlayer.NickName}] {input.text}";
        //Debug.Log($"msg is : {msg}");
        //Debug.Log($"{PhotonNetwork.LocalPlayer.NickName}");
        //Debug.Log($"msg input is : {input.text}");
        pv.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        ReceiveMsg(msg);
        input.ActivateInputField();
        input.text = "";
    }

    private void ChatterUpdate()
    {
        chatters = "PlayerList\n";
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            chatters += p.NickName + "\n";
        }
        chattingList.text = chatters;
    }
    /*
    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        chatLog.text += '\n' + msg;
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
    */
    public GameObject ChatPanel;
    public TMP_Text[] ChatText;
    public TMP_InputField ChatInput;

    public GameObject mainInputField;
    private string chatters;
    public TMP_Text chattingList;

    private PhotonView pv;

    private float time;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void ChatOpen()
    {
        ChatPanel.SetActive(true);
    }

    public void ChatClose()
    {
        ChatPanel.SetActive(false);
    }

    void Update()
    {
        ChatterUpdate();
        if (mainInputField.GetComponent<TMP_InputField>().isFocused == true)
        {
            mainInputField.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
            ChatInput.placeholder.color = new Color(50, 50, 50, 0.5f);
        }

        else
        {
            mainInputField.GetComponent<Image>().color = new Color(255, 255, 255, 0.0f);
            ChatInput.placeholder.color = new Color(50, 50, 50, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Send();
            ChatInput.ActivateInputField();
            ChatInput.Select();
        }

        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text != "")
            {
                time += Time.deltaTime;

                if (time > 8f)
                {
                    for (int j = 1; j < ChatText.Length; j++)
                    {
                        ChatText[j - 1].text = ChatText[j].text;
                        time = 0f;
                        ChatText[j].text = "";
                    }
                }
            }
    }

    #region 채팅
    public void Send()
    {
        if (ChatInput.text != "")
        {
            pv.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
            ChatInput.text = "";
        }
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion

    private void ChatterUpdate()
    {
        chatters = "PlayerList\n";
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            chatters += p.NickName + "\n";
        }
        chattingList.text = chatters;
    }
}
