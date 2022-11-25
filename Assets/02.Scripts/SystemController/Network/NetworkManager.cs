using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static string Name;
    public static string chooseCharName;      // 커스터마이징 씬에서 선택된 캐릭터 정보를 저장

    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        //Name = LoginInfo.userId;
        DontDestroyOnLoad(gameObject);
        ConnectToServer();
    }

    public void ConnectToServer()
    {

        Debug.Log("서버에 연결을 시도합니다.");
        PhotonNetwork.ConnectUsingSettings(); // 서버연결

        if (PlayerPrefs.HasKey("Name"))
            PhotonNetwork.NickName = PlayerPrefs.GetString("Name");
        //나중에 이 부분을 로그인 아이디에서 받아오도록 해야함

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터서버와 연결되었습니다");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    //여기서 여러 방으로 들어가는 내용을 분기해줘야함
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby 함수 작동");
        base.OnJoinedLobby();
        Debug.Log("대기실에 입장하였습니다.");

        //로그인 버튼이 눌러지면 아래의 InitializeRoom 함수를 작동
        //InitiliazeRoom(0);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방이 만들어졌습니다");
    }
    
    public static void InitiliazeRoom(int defaultRoomIndex)
    {
        //DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        RoomOptions roomOptions = new RoomOptions();
        //roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.MaxPlayers = 100;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("testRoom", roomOptions, TypedLobby.Default);
        SceneManager.LoadScene("SKU_MVLobby");
        Debug.Log(Name + "룸으로 이동합니다.");
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방 입장에 실패하였습니다.");
        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 입장하였습니다.");
        //base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnJoinedRoom()
    {
        //Vector3 loadPos = loadZone.transform.position;

        //강의실에 입장시 아래의 인스턴스 코드가 작동하지 않음, 이게 되야 아고라가 활성화 될 것으로 추정
        //spawnedPlayerPrefab = PhotonNetwork.Instantiate("Miso_character_anim", loadPos, Quaternion.identity);
        //Debug.Log("네트워크 인스턴스 게임 오브젝트 명 " + spawnedPlayerPrefab.name);
        //PhotonNetwork.Instantiate("Miso_character_anim", pos, Quaternion.identity);
        Debug.Log("룸에 접속 완료하였습니다");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //PhotonNetwork.Destroy(spawnedPlayerPrefab);
        //Failed to 'network-remove' GameObject because it's null.
        //나가기 이벤트가 기록된 스크립트에서 실행하니 문제없이 진행됨

        Debug.Log("OnLeftRoom() 콜백 호출");
        //아고라 엔진을 재시작하기 위해 컴포넌트를 끈다.
        //PhotonNetwork.JoinLobby();
    }


    #region options
    //나가기 버튼을 눌렀을때 발생해야할 이벤트
    public void OnRoomExitClick()
    {
        Debug.Log("ExitButton Click");
        //Destroy(this.gameObject);
        //PhotonNetwork.Destroy(gameObject);
        PhotonNetwork.LeaveRoom(); 
        SceneManager.LoadScene("ExitLoading");
    }

    //RPC를 사용한 인스턴스 삭제 -> 현재 사용하지 않음
    [PunRPC]
    private void LocalDestroy()
    {
        GameObject.Destroy(photonView);
    }
    [PunRPC]
    private void RemoteDestroy(int viewID)
    {
        GameObject.Destroy(PhotonView.Find(viewID).gameObject);
    }
    #endregion
}
