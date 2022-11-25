using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using agora_utilities;
using agora_gaming_rtc;
using UnityEngine.UI;

public class AgoraManager : MonoBehaviour
{
    public string AppID;
    public string ChannelName;

    VideoSurface myView;
    VideoSurface remoteView;
    IRtcEngine mRtcEngine;

    void Awake()
    {
        SetupUI();
    }
    void Start()
    {
        SetupAgora();
    }
    void Join()
    {
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        myView.SetEnable(true);
        mRtcEngine.JoinChannel(ChannelName, "", 0);
    }
    void Leave()
    {
        mRtcEngine.LeaveChannel();
        mRtcEngine.DisableVideo();
        mRtcEngine.DisableVideoObserver();
    }
    // 이하 전부 콜백함수들
    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        //로컬 유저가 채널에 성공적으로 접속했을 때, 아고라 엔진이 호출할 콜백
        //사업적으로 필요한 로직들을 추가하여 구성 가능하다, 현재는 학습용이므로 로그 정보만을 출력한다

        Debug.LogFormat($"Joined channel {0} successful, my uid = {1}", channelName, uid);
    }
    void OnLeaveChannelHandler(RtcStats stats)
    {
        //여기선 다른 기능은 넣지 않고, 로그만 넘기도록 한다. 필요시 기능 추가 가능
        myView.SetEnable(false);
        if (remoteView != null)
        {
            remoteView.SetEnable(false);
        }
    }

    void OnUserJoined(uint uid, int elapsed)
    {
        //원격에서 유저가 접속했을 경우 이 콜백함수가 누가 들어왔는지 알려준다.
        GameObject go = GameObject.Find("RemoteView");
        if (remoteView == null)
        {
            remoteView = go.AddComponent<VideoSurface>();
        }
        remoteView.SetForUser(uid);
        remoteView.SetEnable(true);
        remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        remoteView.SetGameFps(30);
    }
    void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        //OnUserJoined의 반대 기능을 수행한다.
        //유저가 나갔을 때 사업적으로 필요한 기능을 추가하면 된다.
        remoteView.SetEnable(false);
    }
    void OnApplicationQuit()
    {
        //아고라 엔진은 C++ 네이티브 라이브러리에서 돌아가기 때문에 가비지 컬렉터가 작동하지 않을 수 있다
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
            mRtcEngine = null;
        }
    }
    void SetupUI()
    {
        GameObject go = GameObject.Find("camFace");
        myView = go.AddComponent<VideoSurface>();
        go = GameObject.Find("LeaveButton");
        go?.GetComponent<Button>()?.onClick.AddListener(Leave);
        go = GameObject.Find("JoinButton");
        go?.GetComponent<Button>()?.onClick.AddListener(Join);
    }
    void SetupAgora()
    {
        mRtcEngine = IRtcEngine.GetEngine(AppID);

        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;
    }
}