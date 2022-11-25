using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private string localChooseInfo;
    public static string confirmChoose;

    [Header("- 미리보기 캐릭터 모집단")]
    [SerializeField]
    private GameObject[] previewChar;           //미리보기용 캐릭터 배열

    [Header("- 현재 미리보기 캐릭터")]
    [SerializeField]
    private GameObject currentChar;

    private Renderer mainChar;                  //게임씬에서 보여줄 캐릭터

    [Header("- 현재 선택된 색")]
    [SerializeField]
    private Color listSelected;                 //리스트 팔레트의 색상 정보를 저장

    public Image tmpColor;

    //public GameObject cube;

    void Start()
    {
        
    }

    //리스트 팔레트에서 선택한 색상을 저장, 현재 여기가 작동이 불량함
    public void OnClickPalette()
    {
        //현재 선택한 게임 오브젝트의 정보를 가져온다
        Image tmp = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();    //이미지 컴포넌트에 접근하여 미리보기 이미지에 랜더링
        localChooseInfo = EventSystem.current.currentSelectedGameObject.name;               //클릭한 팔레트의 오브젝트 이름이 곧 캐릭터 이름이 되도록 설정해둠
        listSelected = tmp.color;                                                           //이미지 컴포넌트에 접근하여 미리보기 이미지에 랜더링
        //Debug.Log(localChooseInfo);
        NetworkManager.chooseCharName = localChooseInfo;
        //Debug.Log(NetworkManager.chooseCharName);

        //Debug.Log($"listName is : {tmp.color.r}, {tmp.color.g}, {tmp.color.b}");
        ColorPicker.selectedColor = listSelected;                                           //컬러 픽커와 선택된 색깔을 동기화하도록 설정한다.
        PrevChanger(localChooseInfo);
    }

    /*
    public void ApplyPrev_old()
    {
    //오류 주석 참고자료
    //https://mij9929.tistory.com/entry/Material-%EC%97%90%EB%9F%AC-Material-does-not-have-a-MainTex-texture-property

        //Debug.Log(previewChar.name);
        SkinnedMeshRenderer[] children = previewChar.GetComponentsInChildren<SkinnedMeshRenderer>();

        for(int i = 0; i < children.Length; i++)
        {
            //Debug.Log(children[i].name);
            Debug.Log(children[i].materials[0].name);
            children[i].materials[0].color = ColorPicker.selectedColor;
        }
    }
    */

    //Confirm 버튼을 누르면 해당 오브젝트 이름을 캐릭터 이름으로 넘긴다.
    //OnclickInfo에서 localChooseInfo 변수의 내용을 받는다
    public void ConfirmCustom()
    {
        if (NetworkManager.chooseCharName != null)
        {
            NetworkManager.chooseCharName = localChooseInfo;
        }
        else
        {
            NetworkManager.chooseCharName = "MisoCharT_Blue";
        }
        NetworkManager.InitiliazeRoom(0);
    }
    /*
     * 에러발생
    Not allowed to access Renderer.materials on prefab object. Use Renderer.sharedMaterials instead
    UnityEngine.Renderer:get_materials()
    ColorChanger:ConfirmCustom
    -> 엑세스가 허용되지 않아 NRE가 계속해서 발생

    https://answers.unity.com/questions/1372065/not-allowed-to-access-renderermaterial-on-prefab-o-1.html
    */

    /*
    public void DemoApply()
    {
        MeshRenderer[] dummy = cube.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < dummy.Length; i++)
        {
            Debug.Log(dummy[i].gameObject.name);
            dummy[i].materials[0].color = ColorPicker.selectedColor;
        }
    }
    */
    //미리보기 캐릭터를 색 클릭과 동시에 바꿔주기
    public void PrevChanger(string chooseOnClickPalette)
    {
        currentChar.gameObject.SetActive(false);
        Debug.Log($"파라미터 : {chooseOnClickPalette}");
        for (int i = 0; i < previewChar.Length; i++)
        {
            //Debug.Log($"탐색 내용 : {previewChar[i].name}");          //탐색 과정 확인 완료 -> 정상
            if (chooseOnClickPalette == previewChar[i].gameObject.name)
            {
                Debug.Log("상위 if문 작동");
                if (previewChar[i].activeSelf == true)
                {
                    //Debug.Log($"activeSelf : {previewChar[i].activeSelf}");   //이제 여기서 걸리진 않는다. -> 의도와 합치
                    //Debug.Log($"if : {previewChar[i].name}");
                    continue;
                }
                else
                {
                    previewChar[i].gameObject.SetActive(true);
                    Debug.Log($"활성화된 캐릭터 : {previewChar[i].name}");
                    //currentChar.gameObject.SetActive(false);
                    currentChar = previewChar[i];
                    Debug.Log($"지금 선택된 캐릭터 : {currentChar.name}");
                }
            }
        }
    }
}
