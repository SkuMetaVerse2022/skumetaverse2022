using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionRotation : MonoBehaviour
{
    public float turnSpeed = 30;

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

}
