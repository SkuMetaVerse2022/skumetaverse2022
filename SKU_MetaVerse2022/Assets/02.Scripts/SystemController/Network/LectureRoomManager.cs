using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LectureRoomManager : MonoBehaviourPunCallbacks
{
    [Header("- 캐릭터 초기 위치 지정")]
    public GameObject loadZone;        //오브젝트 위치에 바인딩됨 

    [Header("- 출석 신호 발송")]
    public GameObject attandanceButton;//웹 서버 DB로 Post함

    [Header("- 로딩 이미지")]
    public Image loadImage;

    private string attName;             //출석한 학생의 이름
    private string attId;               //출석한 학생의 학번
    private string lectureCode;         //출석한 강의명, 현재 사용하지 않음

    private void Start()
    {
        //PhotonNetwork.JoinLobby();
        //Invoke("roomConfirm", 2.0f);                    //PeerState 오류 방지를 위한 고의지연 recv와 write사이의 IO타임이 원인 해결은 사실상 불가
        if (SceneManager.GetActiveScene().name == "RoomModern")
        {
            Debug.Log("RoomModern 씬 감지");
            Invoke("roomConfirmModern", 1.0f);
        }
        else if (SceneManager.GetActiveScene().name == "RoomMedieval")
        {
            Debug.Log("RoomMedieval 씬 감지");
            StartCoroutine(roomConfirmMedieval());
        }
        //attandanceButton.gameObject.SetActive(false);   //안써도 되서 비활성화
        StartCoroutine(CreatePlayer());                   //위와 동일한 사유로 고의지연
    }

    private IEnumerator CreatePlayer()
    {
        yield return new WaitForSeconds(3.0f);
        Vector3 loadPos = loadZone.transform.position;

        //Debug.Log(loadPos);
        PhotonNetwork.Instantiate(NetworkManager.chooseCharName, loadPos, Quaternion.identity);
        loadImage.gameObject.SetActive(false);
    }

    public void AttandanceButtonClick()
    {
        CreatePlayer();
        attandanceButton.gameObject.SetActive(false);
        attName = LoginInfo.NamePickUserName;
        //attId = LoginInfo.userId;
        Debug.Log(attName);
        Debug.Log(attId);
        StartCoroutine(AttendTime(attId, attName, DateTime.Now.ToString("mm-dd-hh-m")));
    }
    
    public IEnumerator roomConfirmMedieval()
    {
        lectureCode = LCGateManager.roomCode;
        Debug.Log("RoomConfirm 함수 실행");
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 100;
        ro.IsOpen = true;
        ro.IsVisible = true;

        yield return new WaitForSeconds(1.0f);

        //새로 만들어진 방에 입장시키는 코드 여기서 현재 에러 발생 -> 버그 해결 완료 LeaveRoom을 다른 씬에서 실행해야 큰 원활하게 발생한다. 충돌 이벤트 등에 넣으면 함수가 실행되지 않는다.
        //JoinOrCreateRoom failed. Client is on GameServer (must be Master Server for matchmaking)but not ready for operations (State: Leaving). Wait for callback: OnJoinedLobby or OnConnectedToMaster.
        PhotonNetwork.JoinOrCreateRoom("Medieval", ro, TypedLobby.Default);
        //포톤 이벤트 콜백이 호출되지 않는 문제가 발생
        //conn.OnJoinedRoom();
        Debug.Log(lectureCode + "강의실에 입장했습니다");

        //PhotonNetwork.Instantiate("Miso_character_anim",instantPos, Quaternion.identity);
        //SceneManager.LoadScene("RoomScene01");
    }

    //현대맵용 방 초기화
    public IEnumerator roomConfirmModern()
    {
        lectureCode = LCGateManager.roomCode;
        Debug.Log("RoomConfirm 함수 실행");
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 100;
        ro.IsOpen = true;
        ro.IsVisible = true;

        yield return new WaitForSeconds(1.0f);

        //새로 만들어진 방에 입장시키는 코드 여기서 현재 에러 발생 -> 버그 해결 완료 LeaveRoom을 다른 씬에서 실행해야 큰 원활하게 발생한다. 충돌 이벤트 등에 넣으면 함수가 실행되지 않는다.
        //JoinOrCreateRoom failed. Client is on GameServer (must be Master Server for matchmaking)but not ready for operations (State: Leaving). Wait for callback: OnJoinedLobby or OnConnectedToMaster.
        PhotonNetwork.JoinOrCreateRoom("Modern", ro, TypedLobby.Default);
        //포톤 이벤트 콜백이 호출되지 않는 문제가 발생
        //conn.OnJoinedRoom();
        Debug.Log(lectureCode + "강의실에 입장했습니다");

        //PhotonNetwork.Instantiate("Miso_character_anim",instantPos, Quaternion.identity);
        //SceneManager.LoadScene("RoomScene01");
    }

    private IEnumerator AttendTime(string _sid, string _sName, string _attandTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("aStudentId", _sid);
        form.AddField("aStudentName", _sName);
        form.AddField("aAttendTime", _attandTime);

        UnityWebRequest www = UnityWebRequest.Post("https://localhost:7295/api/Attendance/AttendUser", form);

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload completed");
        }
    }

    public IEnumerator ExitTime(string _sid, string _sName, string _exitTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("studentId", _sid);
        form.AddField("studentName", _sName);
        form.AddField("exitTime", _exitTime);

        UnityWebRequest www = UnityWebRequest.Post("localhost:7705", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form Upload Completed");
        }
    }
}
