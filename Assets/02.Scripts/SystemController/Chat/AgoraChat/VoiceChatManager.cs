using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using agora_gaming_rtc;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class VoiceChatManager : MonoBehaviourPunCallbacks
{
	string appID = "9cb76e7170744d50ad648c3e99ab0174";

	public static VoiceChatManager Instance;

	IRtcEngine rtcEngine;

	void Awake()
	{
		
		if (Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start()
	{
		
		if (string.IsNullOrEmpty(appID))
		{
			Debug.LogError("App ID not set in VoiceChatManager script");
			return;
		}

		rtcEngine = IRtcEngine.GetEngine(appID);

		rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
		rtcEngine.OnLeaveChannel += OnLeaveChannel;
		rtcEngine.OnError += OnError;

		rtcEngine.EnableSoundPositionIndication(true);
	}

	void OnError(int error, string msg)
	{
		//Debug.LogError("Error with Agora: " + msg);
		Debug.LogError($"Error with Agora: {msg}");
	}

	void OnLeaveChannel(RtcStats stats)
	{
		Debug.Log("Left channel with duration " + stats.duration);
	}

	void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
	{
		Debug.Log("Joined channel " + channelName);

		Hashtable hash = new Hashtable();
		hash.Add("agoraID", uid.ToString());
		PhotonNetwork.SetPlayerCustomProperties(hash);
	}

	public IRtcEngine GetRtcEngine()
	{
		return rtcEngine;
	}

	public override void OnJoinedRoom()
	{
		rtcEngine.JoinChannel(PhotonNetwork.CurrentRoom.Name);
	}

	public override void OnLeftRoom()
	{
		rtcEngine.LeaveChannel();
	}

	void OnDestroy()
	{
		IRtcEngine.Destroy();
	}

	private void SpatialAudioAwake()
	{
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

	private void SpatialAudioInit()
	{
        if (string.IsNullOrEmpty(appID))
        {
            Debug.LogError("App ID not set in VoiceChatManager script");
            return;
        }

        rtcEngine = IRtcEngine.GetEngine(appID);

        rtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
        rtcEngine.OnLeaveChannel += OnLeaveChannel;
        rtcEngine.OnError += OnError;

        rtcEngine.EnableSoundPositionIndication(true);
    }

    //버전 4.0
    //https://api-ref.agora.io/en/voice-sdk/unity/4.x/API/class_irtcengine.html#api_irtcengine_disableaudio
    //버전 3.7.1
    //https://api-ref.agora.io/en/voice-sdk/unity/3.x/classagora__gaming__rtc_1_1_i_rtc_engine.html#ab9827ded693dc70beefbfc55f0afc040
    public void LocalMicOff()
	{
		rtcEngine.EnableLocalAudio(false);
		//Debug.Log($"EnableLocalAudio : {rtcEngine.EnableLocalAudio(false)}");
	}

	public void LocalMicOn()
	{
		rtcEngine.EnableLocalAudio(true);
        //Debug.Log($"EnableLocalAudio : {rtcEngine.EnableLocalAudio(true)}");
    }

	public void LocalVideoOff()
	{

		rtcEngine.EnableLocalVideo(false);
		
	}

	public void LocalVideoOn()
	{
		rtcEngine.EnableLocalVideo(true);
	}
}
