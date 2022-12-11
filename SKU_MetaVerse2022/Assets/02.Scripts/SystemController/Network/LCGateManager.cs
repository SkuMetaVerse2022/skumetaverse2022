using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LCGateManager : MonoBehaviourPunCallbacks
{
    public static string roomCode;

    //네트워크 캐릭터
    private GameObject selectedChar;

    public void OnTriggerEnter(Collider collision)
    {
        /*if (collision.gameObject.tag == "Rooms")
        {
            Debug.Log(collision.gameObject.name);
            Debug.Log("Collision to Room object");
            PhotonNetwork.Destroy(this.gameObject);
            //Debug.Log(this.gameObject.name);
            roomCode = collision.gameObject.name;
            Destroy(this.gameObject);
            LeaveLC();
        }*/
        if (collision.gameObject.tag == "Rooms")
        {
            PhotonNetwork.LeaveRoom();
            string rc;
            rc = collision.gameObject.name;
            //LeaveLobby2(rc);
            LeaveLobby();
            EnterLectureRoom(rc);
            //LeaveLC();
        }
    }

    public void LeaveLobby()
    {
        GameObject chatManager = GameObject.Find("ManagerGroup");
        Destroy(chatManager);
        //로딩씬을 따로 호출하여 포톤 컴포넌트 로드 시간을 확보한다.
        //SceneManager.LoadScene("LoadingScene");
        //lcrm.ExitTime(LoginInfo.userId, LoginInfo.Name, DateTime.Now.ToString("mm-dd-hh-m"));
    }

    private void LeaveLobby2(string roomName)
    {
        PhotonNetwork.Destroy(selectedChar);
        Destroy(selectedChar);
    }

    public void EnterLectureRoom(string collRoomCode)
    {
        //yield return new WaitForSeconds(1.0f);
        PhotonNetwork.JoinLobby();
        Debug.Log(collRoomCode);
        Debug.Log("Collision to Room object");
        roomCode = collRoomCode;
        //PhotonNetwork.Destroy(selectedChar);
        //Destroy(selectedChar);
        SceneManager.LoadScene(roomCode);
    }

    public void LeaveLecture()
    {
        SceneManager.LoadScene("ExitLoading");
    }
}
