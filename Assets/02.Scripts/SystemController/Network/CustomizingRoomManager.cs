using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CustomizingRoomManager : MonoBehaviour
{
    public static string localChooseInfo;  //색 팔레트를 누르면 해당 색 캐릭터를 로드하도록 저장

    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("SKU_MVLobby");
    }
}
