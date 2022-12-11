using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbySceneManager : MonoBehaviourPunCallbacks
{
    [Header("- 캐릭터 초기위치")]
    [SerializeField]
    private GameObject loadZone;

    [Header("- 로비 진입 로딩")]
    [SerializeField]
    private Image loadImage;

    [Header("- 채팅 Canvas 활성화용")]
    [SerializeField]
    private GameObject controllCanvas;

    [Header("- 도움말 버튼과 메시지")]
    [SerializeField]
    private GameObject uiPack;

    private string playerName;

    void Start()
    {
        //controllCanvas.gameObject.SetActive(false);   //로딩이 끝나기 전까지 chatting 창을 가린다.
        StartCoroutine(CreateCharacter());                //PeerState 오류 방지를 위해 2초 고의 지연
        StartCoroutine(LoadSceneOff());                  //고의 지연시간동안 화면이 보이지 않도록 일부러화면을 가린다.
        //Invoke("InitUi", 3.0f);                         //각종 UI들을 고의 지연 시간동안 가린 후 활성화한다.
    }

    public IEnumerator CreateCharacter()
    {
        yield return new WaitForSeconds(3.0f);
        Vector3 loadPos = loadZone.transform.position;

        PhotonNetwork.Instantiate(NetworkManager.chooseCharName, loadPos, Quaternion.identity); //전역변수에 저장된 캐릭터 정보를 저장
        Debug.Log($"Instantiate now : {NetworkManager.chooseCharName}");
        //helpBtn.gameObject.SetActive(true);
    }

    private IEnumerator LoadSceneOff()
    {
        yield return new WaitForSeconds(3.0f);
        loadImage.gameObject.SetActive(false);
    }

    private void InitLobbyChat()
    {
        controllCanvas.gameObject.SetActive(true);
    }
}
