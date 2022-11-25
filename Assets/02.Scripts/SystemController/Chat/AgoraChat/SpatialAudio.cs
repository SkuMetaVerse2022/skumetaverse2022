using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using agora_gaming_rtc;
using System.Linq;

using System;
using agora_utilities;

public class SpatialAudio : MonoBehaviourPun
{
    [Header("Agora Attribute")]
    private IRtcEngine agoraEngine;

    [SerializeField]
    private List<Transform> players;
    [SerializeField]
    private List<uint> playerUIDs;
    [Header("연결된 사람이 있다면 표시될 사항")]
    public GameObject signal;

    private IAudioEffectManager agoraAudioEffects;
    private SphereCollider agoraChatRadius;
    private AgoraVideoChat agoraScript;
    private PhotonView pv;

    private const float MAX_CHAT_PROXIMITY = 1.5f;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            agoraScript = GetComponent<AgoraVideoChat>();
            agoraChatRadius = GetComponent<SphereCollider>();

            agoraEngine = agoraScript.GetRtcEngine();
            agoraAudioEffects = agoraEngine.GetAudioEffectManager();

            agoraEngine.OnUserJoined += OnUserJoinedHandler;
            agoraEngine.OnUserOffline += OnUserOfflineHandler;

            players = new List<Transform>();
            playerUIDs = new List<uint>();
        }
    }

    private void Update()
    {
        //player List에서 한명도 없다면 signal을 비활성화, 있으면 활성화
        if(players.Count <= 0)
        {
            signal.gameObject.SetActive(false);
        }
        else
        {
            signal.gameObject.SetActive(true);
        }
    }

    private void OnUserJoinedHandler(uint uid, int elapsed)
    {
        if (photonView.IsMine)
        {
            StartCoroutine(AddNetworkedPlayersToSpatialAudioList());
        }
    }

    private void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        if (photonView.IsMine)
        {
            for(int i = 0; i < playerUIDs.Count; i++)
            {
                if (uid == playerUIDs[i])
                {
                    playerUIDs.RemoveAt(i);
                    players.RemoveAt(i);
                    break;
                }
            }
        }
    }

    IEnumerator AddNetworkedPlayersToSpatialAudioList()
    {
        if (photonView.IsMine == false)
        {
            yield break;
        }

        GameObject[] otherPlayers = GameObject.FindGameObjectsWithTag("Player");

        if (otherPlayers.Length >= 1)
        {
            Vector3 thisPosition = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z);
            Instantiate(signal, thisPosition, this.gameObject.transform.rotation);
        }

        foreach (GameObject player in otherPlayers)
        {
            if (player == this.gameObject)
            {
                continue;
            }

            uint playerNetworkID = player.GetComponent<AgoraVideoChat>().GetNetworkedUID();

            float networkTimer = 2f;
            float networkWaitTime = 0f;
            while (playerNetworkID == 0)
            {
                playerNetworkID = player.GetComponent<AgoraVideoChat>().GetNetworkedUID();
                networkWaitTime += Time.deltaTime;
                if (networkWaitTime >= networkTimer)
                {
                    Debug.Log("PhotonNetwork time out for finding player network ID on player : " + player.name);
                    break;
                }
                yield return new WaitForSeconds(.05f);
            }

            bool isPlayerAlreadyInList = false;
            foreach (uint playerID in playerUIDs)
            {
                if (playerNetworkID == playerID)
                {
                    isPlayerAlreadyInList = true;
                    break;
                }
            }
            if (isPlayerAlreadyInList == false)
            {
                players.Add(player.transform);
                playerUIDs.Add(playerNetworkID);
            }
        }
    }

    void UpdateSpatialAudio()
    {
        if (photonView.IsMine)
        {
            for(int i = 0; i < players.Count; i++)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, players[i].position);
                float gain = GetGainByPlayerDistance(distanceToPlayer);
                float pan = GetPanByPlayerOrientation(players[i]);

                agoraAudioEffects.SetRemoteVoicePosition(playerUIDs[i], pan, gain);
            }
        }
    }

    float GetPanByPlayerOrientation(Transform otherPlayer)
    {
        Vector3 directionToRemotePlayer = otherPlayer.position - transform.position;
        directionToRemotePlayer.Normalize();
        float pan = Vector3.Dot(transform.right, directionToRemotePlayer);
        return pan;
    }

    float GetGainByPlayerDistance(float distanceToPlayer)
    {
        distanceToPlayer = Mathf.Clamp(distanceToPlayer, MAX_CHAT_PROXIMITY, agoraChatRadius.radius);
        float gain = (distanceToPlayer - agoraChatRadius.radius) / (MAX_CHAT_PROXIMITY - agoraChatRadius.radius);
        gain *= 100;
        return gain;
    }
}