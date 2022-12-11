using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMoveTest : MonoBehaviour
{
    //float speed = 3.0f;
    float rotSpeed = 120f;
    public Animator anim;
    void Start()
    {
        
    }

    void Update()
    {
        /*float horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontal, Space.World);
        transform.Translate(Vector3.up * Time.deltaTime * speed * Vertical, Space.World);*/
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("Walk", true);
            this.transform.Rotate(0.0f,-90.0f*Time.deltaTime,0.0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("Walk", true);
            this.transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetBool("Walk", true);
            this.transform.Translate(0.0f,0.0f, -0.7f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetBool("Walk", true);
            this.transform.Translate(0.0f, 0.0f, 0.7f);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            anim.SetBool("Sitdown", true);
            anim.SetBool("Walk", false);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            anim.SetBool("Getup", true);
            anim.SetBool("Sitdown", false);
            Invoke("Idle", 1.5f);

        }
        
        



        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Idle", true);
        }

        /*        else
                {
                    anim.SetBool("Walk", false);
                }*/

    }
    public void Idle()
    {
        anim.SetBool("Getup", false);
        anim.SetBool("Idle", true);
    }
}

