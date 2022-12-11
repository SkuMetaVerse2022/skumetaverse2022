using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPun, IPunObservable
{
    #region 변수
    //일반변수
    private float h, v, rh, rv;
    private bool jDown;
    private bool isJump;
    private Rigidbody rigid;
    private Transform tr;
    private Animator anim;

    public Transform Head;

    public float speed = 15.0f;
    public float speedup = 25.0f;
    public float Rotspeed = 60.0f;

    //레이캐스트
    private RaycastHit hit;
    private int layerMask;
    private int CarMask;
    private Vector3 hitpos;
    private Vector3 Locatepos;

    //포톤 변수
    private PhotonView pv;
    private Vector3 receivePos;
    private Quaternion receiveRot;
    public float damping = 10.0f;

    //UI 변수
    [Header("- 내꺼인지 구분하기")]
    [SerializeField]
    private Canvas controller;
    private TMP_Text nameTag;

    //카메라
    private CinemachineVirtualCamera virCam;
    private CinemachineComposer AimOffset;
    [Header("- 미니맵 카메라")]
    [SerializeField]
    private Camera miniMapCam;

    //애니메이션
    private bool isWalk = false;
    private bool isSit = false;
    private bool isChair;
    private bool Sit = false;
    private bool isHandsUp = false;
    private bool isCar = false;

    //AI
    private NavMeshAgent agent;
    private GameObject target;
    GameObject nearMission;
    public GameObject MissionPanel;
    public GameObject[] Missions;
    public bool[] hasMissions;
    int MissionIndex;
    #endregion

    void Start()
    {
        //이동
        pv = GetComponent<PhotonView>(); 
        tr = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();

        //애니메이션
        anim = GetComponent<Animator>();

        //레이캐스트
        layerMask = 1 << 9;
        CarMask = 1 << 10;

        //AI
        agent = GetComponent<NavMeshAgent>();

        //UI
        controller = GetComponentInChildren<Canvas>();

        //카메라
        virCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        AimOffset = virCam.GetComponentInChildren<CinemachineComposer>();
        miniMapCam = GameObject.FindObjectOfType<Camera>();

        if (pv.IsMine)
        {
            virCam.Follow = tr;
            virCam.LookAt = Head;
            controller.gameObject.SetActive(true);
            miniMapCam.gameObject.SetActive(true);
            
        }
        else
        {
            controller.gameObject.SetActive(false);
            miniMapCam.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        if(pv.IsMine)
        {
            if (Sit == false)
            {
                Move();
            }
            Rotation();
            Jump();
            Ray();
            AnimCtrl();
            Mission();

        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, receivePos, Time.deltaTime * damping);
            tr.rotation = Quaternion.Slerp(tr.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    void Move()
    {
        //이동
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        Vector3 moveDir = (Vector3.forward * -v) + (Vector3.right * -h);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            tr.Translate(moveDir * Time.deltaTime * speedup);
        }
        else
        {
            tr.Translate(moveDir * Time.deltaTime * speed);
        }
        
        
    }

    void Jump()
    {
        jDown = Input.GetButtonDown("Jump");

        if(jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * 6, ForceMode.Impulse);
            isJump = true;

            anim.SetTrigger("doJump");
        }
    }

    void Rotation()
    {
        //회전
        rh = Input.GetAxis("Mouse X");
        rv = Input.GetAxis("Mouse Y");

        if(!Input.GetKey(KeyCode.LeftAlt))
        {
            tr.Rotate(Vector3.up * Time.deltaTime * rh * Rotspeed);
            Head.Translate(Vector3.up * Time.deltaTime * rv * speed);
        }
        
        //AimOffset.m_TrackedObjectOffset.y = rv;
    }

    void AnimCtrl()
    {

        //이동
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        anim.SetBool("isWalk", isWalk);

        //앉기
        if (Input.GetKeyDown(KeyCode.E))
        {
            //일반 앉기
            if (isChair == true)
            {
                if (Sit == false)
                {
                    anim.SetInteger("isSit", 1);
                    Sit = true;
                }
                else
                {
                    anim.SetInteger("isSit", 0);
                    Sit = false;
                }
            }

            //자동차 앉기
            if (isCar == true)
            {
                if (Sit == false)
                {
                    Locatepos = tr.position;
                    Debug.Log(Locatepos);
                    tr.position = hitpos;
                    Sit = true;
                }
                else if (Sit == true)
                {
                    tr.position = Locatepos;
                    Sit = false;
                }

            }
        }

        //손들기
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isHandsUp == false)
            {
                anim.SetInteger("isHandsup", 1);
                isHandsUp = true;
            }
            else
            {
                anim.SetInteger("isHandsup", 0);
                isHandsUp = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
            anim.SetTrigger("doClap");

        if (Input.GetKeyDown(KeyCode.X))
            anim.SetTrigger("doHello");
    }

    void Ray()
    {
        //일반
        if (Physics.Raycast(transform.position + Vector3.up * 2f, transform.forward * -1, out hit, 5, layerMask))
        {
            isChair = true;
        }
        else
        {
            isChair = false;
        }

        //자동차
        if (Physics.Raycast(transform.position + Vector3.up * 2f, transform.forward * -1, out hit, 5, CarMask))
        {
            hitpos = hit.transform.position;

            isCar = true;
        }
        else
        {
            isCar = false;
        }
    }

    void Mission()
    {
        if (Input.GetKey(KeyCode.F) && nearMission != null)
        {
            if (nearMission.tag == "Mission")
            {
                Item item = nearMission.GetComponent<Item>();
                MissionIndex = item.value;

                hasMissions[MissionIndex] = true;
                MissionPanel.SetActive(true);
                Missions[MissionIndex].SetActive(true);

                Destroy(nearMission);
            }
        }
    }

    public void Path1()
    {
        target = GameObject.Find("1");

        if(Input.GetKeyDown(KeyCode.Alpha1))
            tr.position = target.transform.position;
    }

    public void Path2()
    {
        target = GameObject.Find("2");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            tr.position = target.transform.position;
    }

    public void Path3()
    {
        target = GameObject.Find("3");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            tr.position = target.transform.position;
    }

    public void Path4()
    {
        target = GameObject.Find("4");

        if (Input.GetKeyDown(KeyCode.Alpha4))
            tr.position = target.transform.position;
    }

    
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Mission")
        {
            nearMission = other.gameObject;
        }

       
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Mission")
        {
            nearMission = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            isJump = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
