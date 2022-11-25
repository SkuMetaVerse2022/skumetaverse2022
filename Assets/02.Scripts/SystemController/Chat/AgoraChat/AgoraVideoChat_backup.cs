﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using Photon.Pun;
using Photon.Realtime;
//using ExitGames.Client.Photon;


/* NOTE: 
 *
 * This script handles the Agora-related functionality:
 * - Joining / Leaving Channels
 * - Creating / Deleting VideoSurface objects that enable us to see the camera feed of Agora party chat
 * - Managing the UI that contains the VideoSurface objects 
 *
 */



public class AgoraVideoChat_backup : MonoBehaviourPun
{
    private PhotonView pv;

    [Header("Agora Properties")]

    // *** ADD YOUR APP ID HERE BEFORE GETTING STARTED *** //
    [SerializeField] private string appID = "99a2d8ab31e7472ba3e49e3c3ad728f3";
    [SerializeField] private string channel = "unity3d";
    private string originalChannel;
    private IRtcEngine mRtcEngine;
    private uint myUID = 0;

    [Header("Player Video Panel Properties")]
    [SerializeField] private GameObject userVideoPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private RectTransform content;
    [SerializeField] private float spaceBetweenUserVideos = 150f;
    private List<GameObject> playerVideoList;

    public delegate void AgoraCustomEvent();
    public static event AgoraCustomEvent PlayerChatIsEmpty;
    public static event AgoraCustomEvent PlayerChatIsPopulated;

    //Added From https://www.agora.io/en/blog/implementing-spatial-audio-chat-in-unity-using-agora/
    private uint networkedUID;
    private Room roomInfo;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        Debug.Log("AgoraVideoChat start");
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        playerVideoList = new List<GameObject>();

        // Setup Agora Engine and Callbacks.
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
        }

        originalChannel = channel;

        // -- These are all necessary steps to initialize the Agora engine -- //
        // Initialize Agora engine
        mRtcEngine = IRtcEngine.getEngine(appID);

        // Setup square video profile
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();
        config.dimensions.width = 480;
        config.dimensions.height = 480;
        config.frameRate = FRAME_RATE.FRAME_RATE_FPS_15;
        config.bitrate = 800;
        config.degradationPreference = DEGRADATION_PREFERENCE.MAINTAIN_QUALITY;
        mRtcEngine.SetVideoEncoderConfiguration(config);

        // Setup our callbacks (there are many other Agora callbacks, however these are the calls we need).
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnUserJoined = OnUserJoinedHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;
        mRtcEngine.OnUserOffline = OnUserOfflineHandler;

        // Your video feed will not render if EnableVideo() isn't called. 
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();

        // By setting our UID to "0" the Agora Engine creates a unique UID and returns it in the OnJoinChannelSuccess callback. 
        mRtcEngine.JoinChannel(channel, null, 0);
    }

    public uint GetNetworkedUID() => networkedUID;

    public IRtcEngine GetRtcEngine()
    {
        if (mRtcEngine != null)
        {
            return mRtcEngine;
        }
        return null;
    }

    [PunRPC]
    public void UpdateNetworkedPlayerUID(string newUID)
    {
        networkedUID = uint.Parse(newUID);
        Debug.Log(networkedUID);
    }

    public string GetCurrentChannel() => channel;

    public void JoinRemoteChannel(string remoteChannelName)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        mRtcEngine.LeaveChannel();

        mRtcEngine.JoinChannel(remoteChannelName, null, myUID);
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();

        channel = remoteChannelName;
    }

    /// <summary>
    /// Resets player Agora video chat party, and joins their original channel.
    /// </summary>
    public void JoinOriginalChannel()
    {
        if (!photonView.IsMine)
        {
            return;
        }


        /* NOTE:
         * Say I'm in my original channel - "myChannel" - and someone joins me.
         * If I want to leave the party, and go back to my original channel, someone is already in it!
         * Therefore, if someone is inside "myChannel" and I want to be alone, I have to join a new channel that has the name of my unique Agora UID "304598093" (for example).
         */
        if (channel != originalChannel || channel == myUID.ToString())
        {
            channel = originalChannel;
        }
        else if (channel == originalChannel)
        {
            channel = myUID.ToString();
        }

        JoinRemoteChannel(channel);
    }

    #region Agora Callbacks
    // Local Client Joins Channel.
    private void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        Debug.Log("OnJoinChannelSuccessHandler, 로컬 클라이언트가 아고라 채널에 접속함");
        if (!photonView.IsMine)
        {
            return;
        }

        myUID = uid;

        CreateUserVideoSurface(uid, true);

        this.photonView.RPC("UpdateNetworkedPlayerUID", RpcTarget.All, myUID.ToString()); //원본 PhotonTargets.All
    }

    // Remote Client Joins Channel.
    private void OnUserJoinedHandler(uint uid, int elapsed)
    {
        Debug.Log("OnUserJoinedHandler, 원격 클라이언트가 아고라 채널에 접속함");
        if (!photonView.IsMine)
        {
            return;
        }

        CreateUserVideoSurface(uid, false);
        this.photonView.RPC("UpdateNetworkedPlayerUID", RpcTarget.All, myUID.ToString()); //원본 PhotonTargets.All
    }

    // Local user leaves channel.
    private void OnLeaveChannelHandler(RtcStats stats)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        foreach (GameObject player in playerVideoList)
        {
            Destroy(player.gameObject);
        }
        playerVideoList.Clear();
    }

    // Remote User Leaves the Channel.
    private void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (playerVideoList.Count <= 1)
        {
            PlayerChatIsEmpty();
        }

        RemoveUserVideoSurface(uid);
    }
    #endregion

    //여기를 랜더링이 camface로 되도록 수정해야함 -> 시간적 여유를 두고 길게 보고 수행해야함
    // Create new image plane to display users in party.
    private void CreateUserVideoSurface(uint uid, bool isLocalUser)
    {
        // Avoid duplicating Local player VideoSurface image plane.
        for (int i = 0; i < playerVideoList.Count; i++)
        {
            if (playerVideoList[i].name == uid.ToString())
            {
                return;
            }
        }

        // Get the next position for newly created VideoSurface to place inside UI Container.
        float spawnY = playerVideoList.Count * spaceBetweenUserVideos;
        Vector3 spawnPosition = new Vector3(0, -spawnY, 0);

        // Create Gameobject that will serve as our VideoSurface.
        GameObject newUserVideo = Instantiate(userVideoPrefab, spawnPosition, spawnPoint.rotation);
        if (newUserVideo == null)
        {
            Debug.LogError("CreateUserVideoSurface() - newUserVideoIsNull");
            return;
        }
        newUserVideo.name = uid.ToString();
        newUserVideo.transform.SetParent(spawnPoint, false);
        newUserVideo.transform.rotation = Quaternion.Euler(Vector3.right * -180);

        playerVideoList.Add(newUserVideo);

        // Update our VideoSurface to reflect new users
        VideoSurface newVideoSurface = newUserVideo.GetComponent<VideoSurface>();
        if (newVideoSurface == null)
        {
            Debug.LogError("CreateUserVideoSurface() - VideoSurface component is null on newly joined user");
            return;
        }

        if (isLocalUser == false)
        {
            newVideoSurface.SetForUser(uid);
        }
        newVideoSurface.SetGameFps(30);

        // Update our "Content" container that holds all the newUserVideo image planes
        content.sizeDelta = new Vector2(0, playerVideoList.Count * spaceBetweenUserVideos + 140);

        UpdatePlayerVideoPostions();
        UpdateLeavePartyButtonState();
    }

    private void RemoveUserVideoSurface(uint deletedUID)
    {
        foreach (GameObject player in playerVideoList)
        {
            if (player.name == deletedUID.ToString())
            {
                playerVideoList.Remove(player);
                Destroy(player.gameObject);
                break;
            }
        }

        // update positions of new players
        UpdatePlayerVideoPostions();

        Vector2 oldContent = content.sizeDelta;
        content.sizeDelta = oldContent + Vector2.down * spaceBetweenUserVideos;
        content.anchoredPosition = Vector2.zero;

        UpdateLeavePartyButtonState();
    }

    private void UpdatePlayerVideoPostions()
    {
        for (int i = 0; i < playerVideoList.Count; i++)
        {
            playerVideoList[i].GetComponent<RectTransform>().anchoredPosition = Vector2.down * spaceBetweenUserVideos * i;
        }
    }

    private void UpdateLeavePartyButtonState()
    {
        if (playerVideoList.Count > 1)
        {
            PlayerChatIsPopulated();
        }
        else
        {
            PlayerChatIsEmpty();
        }
    }

    private void TerminateAgoraEngine()
    {
        if (mRtcEngine != null)
        {
            mRtcEngine.LeaveChannel();
            mRtcEngine = null;
            IRtcEngine.Destroy();
        }
    }

    private IEnumerator OnLeftRoom()
    {
        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while (PhotonNetwork.CurrentRoom != null || PhotonNetwork.IsConnected == false)
        {
            yield return 0;
        }

        TerminateAgoraEngine();
    }

    // Cleaning up the Agora engine during OnApplicationQuit() is an essential part of the Agora process with Unity. 
    private void OnApplicationQuit()
    {
        TerminateAgoraEngine();
    }
}