using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPalette : MonoBehaviour
{
    //private Color[] colorArr;
    private Color[] colorPalette;
    [SerializeField]
    private GameObject motherObject;
    private Image[] paletteRenderer;
    private GameObject[] paletteCharName;

    void Start()
    {
        ColorMapping();
    }

    // n/255f를 안붙이면 적용이 안돼는 아주 거지같은 특성이 있음에 주의
    private void ColorMapping()
    {
        Color[] colorArr = new Color[]
        {
            new Color(255/255f, 0/255f, 0/255f),    //Red
            new Color(255/255f, 255/255f, 255/255f),//white
            new Color(0/255f, 0/255f, 255/255f),    //Blue
            new Color(255/255f, 255/255f,  0/255f), //Yellow
            new Color(180/255f,  85/255f,  162/255f),//Purple
    };
        //자식객체의 컬러 컴포넌트
        paletteRenderer = motherObject.GetComponentsInChildren<Image>();
        Debug.Log($"팔레트 길이 : {paletteRenderer.Length}");

        //자식객체에 모두 접근하여 Color값 할당
        for (int i = 0; i < colorArr.Length; i++)
        {
            //Debug.Log($"{paletteRenderer[i].name}, ({colorArr[i].r}, {colorArr[i].g}, {colorArr[i].b})");
            paletteRenderer[i].color = colorArr[i];
            //Debug.Log($"{ paletteRenderer[i]}");
        }
    }
}
