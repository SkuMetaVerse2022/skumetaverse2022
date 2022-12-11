using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AdminCtrl : MonoBehaviour
{
    [Header("- 패스워드입력창")]
    public GameObject Password;
    [Header("- PDF 랜더링 오브젝트")]
    public GameObject PdfPanel;
    public GameObject PdfBtn;
    [Header("- 로그인 실패시 보여줄 오브젝트")]
    public GameObject Fail;
    [Header("- 로비 경고창 텍스트")]
    public TMP_Text lobbyFailed; //로비에선 못띄운다고 알려주기
    [Header("- 패스워드 입력 필드")]
    public InputField PwdText;

    private string password = "221115";

    //로비에선 PDF를 사용할 수 없음을 알림
    public void SceneChecker()
    {
        //초기엔 텍스트 객체를 인스펙터에서 비활성화 시켜둠
        if(SceneManager.GetActiveScene().name == "SKU_MVLobby")
        {
            Debug.Log("PDF는 로비에선 사용불가");
            StartCoroutine(TextTimer());
        }
        else
        {
            //강의실 Scene임이 확인되면 거기서 PDF 오브젝트를 동적으로 찾아온다.
            PdfPanel = GameObject.Find("PDF");
        }
    }

    //경고창 타이머
    private IEnumerator TextTimer()
    {
        float timer = 30.0f;
        while (timer >= 0.0f)
        {
            timer -= Time.deltaTime;
            lobbyFailed.gameObject.SetActive(true);
            yield return null;
            if (timer < 0.0f)
                lobbyFailed.gameObject.SetActive(false);
        }
    }

    public void OnClickPwdButton()
    {
        if (PwdText.text == password)
        {
            Password.SetActive(false);
            PdfPanel.transform.Find("Canvas").transform.Find("PdfBackground").gameObject.SetActive(true);
            PwdText.text = "";
        }
        else
        {
            Password.SetActive(false);
            Fail.SetActive(true);
            PwdText.text = "";
        }
    }
}