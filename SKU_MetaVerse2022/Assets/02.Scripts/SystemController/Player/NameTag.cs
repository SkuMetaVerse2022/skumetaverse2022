using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NameTag : MonoBehaviourPunCallbacks
{
    public TMP_Text name;
    private string getName;
    //public GameObject nameTag;
    private PhotonView pv;
    private Player player;
    void Start()
    {
        //name.gameObject.SetActive(false);
        pv = GetComponent<PhotonView>();
        
        //pv.Owner.NickName = PlayerPrefs.GetString("Name");

        if (pv.IsMine)
        {
            //name = GetComponent<TextMeshPro>();
            //Debug.Log(pv.Owner.NickName);
            //Debug.Log(name.gameObject.name);
            name.text = pv.Owner.NickName;
        }
        else
        {
            //pv.RPC("RemoteName", RpcTarget.All);
            //name.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        pv = GetComponent<PhotonView>();

        //Debug.Log(pv.Owner.NickName);
        name.text = pv.Owner.NickName;
        Debug.Log("OnEnable: " + getName);
    }

    private IEnumerator GetName()
    {
        yield return new WaitForSeconds(1.0f);
        name.gameObject.SetActive(true);
    }

    [PunRPC]
    private void RemoteName()
    {
        name.text = pv.Owner.NickName;
    }

    string GetNickNameByActorNumber(int actorNumber)
    {
        foreach(Player player in PhotonNetwork.PlayerListOthers)
        {
            if(player.ActorNumber == actorNumber)
            {
                Debug.Log(player.NickName);
                return player.NickName;
            }
        }
        return "Ghost";
    }
}
