using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    /*
    구현해야할 기능
    1. UI 제어 -> 끄고 키기
    2. 자신이 들어간 강의실이 구분 가능한 내용 추가해줘야함
    3. 방 안에 들어온 학생의 수를 보여준다.
    4. Exit 버튼 구현 -> 방을 나가는법을 구현해야한다.
    5. 다른 방에 들어가는 내용도 구현해야한다.
    */
    public GameObject uiSet;

    public Button exitButton;
    public TMP_Text roomName;
    public TMP_Text msgList;
    public TMP_Text connectInfo;

    public string[] roomList;

    void Start()
    {
    }

    private void SetRoomInfo()
    {
        Room room = PhotonNetwork.CurrentRoom;
        roomName.text = room.Name;
        connectInfo.text = $"({room.PlayerCount}/{room.MaxPlayers}";
    }

    private void UiController()
    {
        if (uiSet.gameObject == true)
            uiSet.gameObject.SetActive(false);
        else
            uiSet.gameObject.SetActive(true);
    }

    private void OnExitClick()
    {
        PhotonNetwork.LeaveRoom();
        //나갔을 때 필요한 이벤트 콜백은 NetworkManager에 나와있다.
        //여기서는 나가기만 해도 문제가 되지 않음
    }
}
