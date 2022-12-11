using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PartyJoiner : MonoBehaviourPun
{
    [Header("Local Player Stats")]
    [SerializeField]
    public Button inviteButton;
    [SerializeField]
    public GameObject joinButton;
    [SerializeField]
    public GameObject leaveButton;

    [Header("Remote Player Stats")]
    [SerializeField]
    private int remotePlayerViewID;
    [SerializeField]
    private string remoteInviteChannelName = null;

    private AgoraVideoChat agoraVideo;
    private PhotonView pv;

    private void Awake()
    {
        agoraVideo = GetComponent<AgoraVideoChat>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
        {
            // Disable the Canvas of remote users - If we didn't, everyone's canvas would render to all screens.
            transform.GetChild(0).gameObject.SetActive(false);
        }

        inviteButton.interactable = false;
        joinButton.SetActive(false);
        leaveButton.SetActive(false);
    }

    private void OnEnable()
    {
        AgoraVideoChat.PlayerChatIsEmpty += DisableLeaveButton;
        AgoraVideoChat.PlayerChatIsPopulated += EnableLeaveButton;
    }

    private void OnDisable()
    {
        AgoraVideoChat.PlayerChatIsEmpty -= DisableLeaveButton;
        AgoraVideoChat.PlayerChatIsPopulated -= EnableLeaveButton;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine || !other.CompareTag("Player"))
        {
            return;
        }

        // Get a reference to the player that we are standing next to in our Trigger Volume.
        PhotonView otherPlayerPhotonView = other.GetComponent<PhotonView>();
        if (otherPlayerPhotonView != null)
        {
            remotePlayerViewID = otherPlayerPhotonView.ViewID;
            inviteButton.interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine || !other.CompareTag("Player"))
        {
            return;
        }

        // Remove that player reference when they walk away.
        remoteInviteChannelName = null;
        inviteButton.interactable = false;
        joinButton.SetActive(false);
    }

    public void OnInviteButtonPress()
    {
        // Sends out a ping across the Photon network to check if any players have this "Invite" function attached.
        PhotonView.Find(remotePlayerViewID).RPC("InvitePlayerToPartyChannel", RpcTarget.All, remotePlayerViewID, agoraVideo.GetCurrentChannel());
    }

    [PunRPC]
    public void InvitePlayerToPartyChannel(int invitedID, string channelName)
    {
        // When the invited ID matches the correct player ID, update their canvas and tell them what Agora channel to join.
        if (photonView.IsMine && invitedID == photonView.ViewID)
        {
            joinButton.SetActive(true);
            remoteInviteChannelName = channelName;
        }
    }



    public void OnJoinButtonPress()
    {
        if (photonView.IsMine && remoteInviteChannelName != null)
        {
            agoraVideo.JoinRemoteChannel(remoteInviteChannelName);
            joinButton.SetActive(false);
            leaveButton.SetActive(true);
        }
    }

    public void OnLeaveButtonPress()
    {
        if (photonView.IsMine)
        {
            agoraVideo.JoinOriginalChannel();
            leaveButton.SetActive(false);
        }
    }

    private void EnableLeaveButton()
    {
        if (photonView.IsMine)
        {
            leaveButton.SetActive(true);
        }
    }

    private void DisableLeaveButton()
    {
        if (photonView.IsMine)
        {
            leaveButton.SetActive(false);
        }
    }
}