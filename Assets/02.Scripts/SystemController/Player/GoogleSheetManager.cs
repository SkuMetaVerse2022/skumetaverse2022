using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class GoogleSheetManager : MonoBehaviour
{

    const string URL = "https://docs.google.com/spreadsheets/d/1g7f_3jIjHKkAK1aNUzoJfHgK3nsRAfhE_JP1b4Tau7Q/export?format=tsv&range=B2:D";

    public TMP_Text M1, M2, M3;

    void Start()
    {
        StartCoroutine(ObtainSheetData());
    }

    IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        //Debug.Log(data);

        SetMission(data);
    }

    void SetMission(string tsv)
    {
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            for (int j = 0; j < columnSize; j++)
            {
            

                M1.text = column[0];
                M2.text = column[1];
                M3.text = column[2];
            }
        }
    }

    public void OnClick()
    {
        StartCoroutine(ObtainSheetData());
    }
    
}
