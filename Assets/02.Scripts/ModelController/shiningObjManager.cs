using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shiningObjManager : MonoBehaviour
{

    Vector3 pos; //현재위치

    float delta = 0.05f;

    public float speed = 2.0f;




    void Start()
    {

        pos = transform.position;

    }


    void Update()
    {

        Vector3 v = pos;

        v.y += delta * Mathf.Sin(Time.time * speed);


        transform.position = v;

    }
}
