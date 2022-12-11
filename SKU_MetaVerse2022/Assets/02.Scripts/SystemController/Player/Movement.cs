using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

    public class Movement : MonoBehaviourPun, IPunObservable
    {
    #region 변수
        private float hAxis => Input.GetAxis("Horizontal");
        private float vAxis => Input.GetAxis("Vertical");
        public float speed;
        Vector3 moveVec;

        private Animator animator;
        //private CinemachineVirtualCamera virtualCamera;
        private CinemachineFreeLook virtualCameraFreeLook;
        //private new Transform transform;
        private new Camera camera;
        #endregion

    #region PhotonController

        private PhotonView pv;
        private Vector3 receivePos;
        private Quaternion receiveRot;
        public float damping = 10.0f;
    [SerializeField]
    private GameObject chatPanel;
    #endregion

    #region AgoraConroller

    private AgoraVideoChat agoraVideoChat;

    #endregion

    void Start()
    {
        agoraVideoChat = GetComponent<AgoraVideoChat>();
        animator = GetComponent<Animator>();
        virtualCameraFreeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();
        pv = GetComponent<PhotonView>();
        camera = Camera.main;

        if (pv.IsMine)
        {
            virtualCameraFreeLook.Follow = transform;
            virtualCameraFreeLook.LookAt = transform;
        }
        //강의실 입장시 아래 두줄 활성화되지 않음
        agoraVideoChat.gameObject.SetActive(true);
        Debug.Log("agoraVideoChat SetActived");
    }

    void Update()
    {

    if (pv.IsMine)
    {
                Move();
                Turn();
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);
                transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
            }
        }


            //hAxis => Input.GetAxis("Horizontal");
            //vAxis => Input.GetAxis("Vertical");
        

        void Move()
        {
            moveVec = new Vector3(hAxis, 0, vAxis).normalized;

            transform.position += moveVec * speed * Time.deltaTime;
        }

        void Turn()
        {
            transform.LookAt(moveVec + transform.position);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                receivePos = (Vector3)stream.ReceiveNext();
                receiveRot = (Quaternion)stream.ReceiveNext();
            }
        }

    private void ChatPanelController()
    {
        if (chatPanel.gameObject.activeSelf == true)
        {
            chatPanel.gameObject.SetActive(false);
        }
        if (chatPanel.gameObject.activeSelf == false)
        {
            chatPanel.gameObject.SetActive(true);
        }
    }
}

