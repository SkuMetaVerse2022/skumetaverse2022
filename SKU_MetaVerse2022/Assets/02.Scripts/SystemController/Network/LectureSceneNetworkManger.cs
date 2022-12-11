using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LectureSceneNetworkManger : MonoBehaviourPunCallbacks
{
    private string lectureCode;
    private float loadTimer = 5.0f;
    private string sceneName;

    public GameObject loadButton;

    void Start()
    {
        /*
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.JoinLobby();
        Debug.Log("EnterLC Function: Lecture Room Enter process was started");

        loadButton.gameObject.SetActive(false);

        
        while (true)
        {
            //Debug.Log(loadTimer);
            if (loadTimer < 0.0f)
            {
                //roomConfirm();
                loadButton.gameObject.SetActive(true);
                break;
            }
            loadTimer -= Time.deltaTime;
        }
        */
        Invoke("RejoinLobby", 2.0f);
        Invoke("ReturnLobby", 3.0f);
    }

    public void roomConfirm()
    {
        Debug.Log("RoomConfirm 함수 실행");
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 100;
        ro.IsOpen = true;
        ro.IsVisible = true;

        //새로 만들어진 방에 입장시키는 코드 여기서 현재 에러 발생 -> 버그 해결 완료 LeaveRoom을 다른 씬에서 실행해야 큰 원활하게 발생한다. 충돌 이벤트 등에 넣으면 함수가 실행되지 않는다.
        //JoinOrCreateRoom failed. Client is on GameServer (must be Master Server for matchmaking)but not ready for operations (State: Leaving). Wait for callback: OnJoinedLobby or OnConnectedToMaster.
        PhotonNetwork.JoinOrCreateRoom("testCode", ro, TypedLobby.Default);
        //포톤 이벤트 콜백이 호출되지 않는 문제가 발생
        //conn.OnJoinedRoom();
        Debug.Log(lectureCode + "강의실에 입장했습니다");
        
        //PhotonNetwork.Instantiate("Miso_character_anim",instantPos, Quaternion.identity);
        //SceneManager.LoadScene("RoomScene01");
    }

    public void LoadLectureScene()
    {
        sceneName = LCGateManager.roomCode;
        roomConfirm();
        SceneManager.LoadScene(sceneName);
    }

    private void RejoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public void ReturnLobby()
    {
        sceneName = "CharacterCustomization";
        //roomConfirm();
        SceneManager.LoadScene(sceneName);
    }
}
