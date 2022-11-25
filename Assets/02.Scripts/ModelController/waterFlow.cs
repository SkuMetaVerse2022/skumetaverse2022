using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class waterFlow : MonoBehaviour
{
    // Scroll main texture based on time

    //float scrollSpeed = 0.5f;
    // Renderer rend;
    float x = 0.3f;
    float xOffset;
   // MaterialProperty MainTexture = ShaderGUI.FindProperty("_MainTexture", properties);


    void Start()
    {
        //rend = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //float offset = Time.time * scrollSpeed;
        //rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        xOffset -= (Time.deltaTime * x) / 10f;
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(xOffset, 0);
        gameObject.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(xOffset, 0);
        gameObject.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(xOffset, 0));
    }
}