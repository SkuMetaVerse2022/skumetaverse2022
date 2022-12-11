using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Security.Cryptography; //암호화를 위한 내용
using System.Net;
using System.IO;
using System.Text;
using System;

public class SKU_MetaVerseLogin : MonoBehaviour
{
    #region InputFields
    public InputField studentIdInput;
    public InputField pwInput;

    public InputField registerIdInput;
    public InputField registerPwInput;
    public InputField classDivInput;
    public InputField registerNameInput;
    public InputField registerMailInput;
    #endregion

    //키로 사용하기 위한 암호 정의 대충 아무거나 쳐봄
    private static readonly string PASSWORD = "214jhk123b4jv2134khj12k3j4v";
    //인증키 정의
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

    // 로그인 정보를 임시 저장하는 구조체, 아직은 사용처가 없음
    private struct loginData
    {
        //로그인 정보용 멤버변수
        public string loginStudentID;
        public string loginUserPW;

        //회원가입 정보용 멤버변수
        public string mRegisterId;
        public string mRegisterPw;
        public string mClassDiv;
        public string mRegisterName;
        public string mRegisterMail;
    }

    //전송은 Coroutine을 할당받아서 하나의 쓰레드만 사용하도록 유도한다.
    public void userLogin()
    {
        //각 학생의 ID는 학번으로 사용한다.
        string inputId = studentIdInput.text;
        string inputPw = pwInput.text;
        StartCoroutine(Login(inputId, inputPw));
    }

    public void userRegister()
    {
        string registerInputId = registerIdInput.text;
        string registerInputPw = registerPwInput.text;
        string registerInputClassDiv = classDivInput.text;
        string registerInputName = registerNameInput.text;
        string registerInputMail = registerMailInput.text;
        StartCoroutine(Register(registerInputId, registerInputPw, registerInputClassDiv, registerInputName, registerInputMail));
    }
    
    //여기선 보내기만 한다.
    private IEnumerator Login(string _studentId, string _userPw)
    {
        //웹 통신 시작
        WWWForm form = new WWWForm();
        //post(header, body)
        form.AddField("studentId", _studentId);
        form.AddField("userPw", _userPw);
        
        //웹 리퀘스트를 보내서 서버상에서 받아줘야한다. 해당하는 서버 주소에 맞게 보내줘야한다.
        UnityWebRequest www = UnityWebRequest.Post("localhost:7705", form);

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload completed");
        }
        //웹 통신 종료
    }

    //회원가입 코루틴 제공
    private IEnumerator Register(string mRegisterId, string mRegisterPw, string mRegisterclassDiv, string mRegisterName, string mRegisterMail)
    {
        WWWForm form = new WWWForm();
        form.AddField("registerId", mRegisterId);
        form.AddField("registerPw", mRegisterPw);
        form.AddField("registerClassDiv", mRegisterclassDiv);
        form.AddField("registerName", mRegisterName);
        form.AddField("registerMail", mRegisterMail);

        //차후 추가될 웹서버 주소를 넣어야함
        UnityWebRequest www = UnityWebRequest.Post("localhost:7705", form);

        yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form Upload Completed");
        }
    }
    
    //암호화
    public static string AESEncrypt128(string plain)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = myRijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();
        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    //복호화
    public static string AESDecrypt(string encrypt)
    {
        byte[] encryptBytes = Convert.FromBase64String(encrypt);

        RijndaelManaged myRijndael = new RijndaelManaged();
        myRijndael.Mode = CipherMode.CBC;
        myRijndael.Padding = PaddingMode.PKCS7;
        myRijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();
        ICryptoTransform decryptor = myRijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        cryptoStream.Close();

        return plainString;
    }
}
