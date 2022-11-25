using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphabetRotation1 : MonoBehaviour
{
    public float turnSpeed = 2f;

    void Update()
    {

        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
    }

    
}
