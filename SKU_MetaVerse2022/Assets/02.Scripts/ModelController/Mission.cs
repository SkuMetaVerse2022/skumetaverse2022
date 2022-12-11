using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public GameObject[] Chest;
    Transform[] points;
    int idx;
    int i = 0;

    void Start()
    {
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        StartCoroutine(ItemSpawn());
    }

    void Update()
    {
        
    }

    IEnumerator ItemSpawn()
    {
        while(true)
        {
            normalItemSpawn();
            i++;
            yield return new WaitForSeconds(1200.0f);
        }
    }

    void normalItemSpawn()
    {
        idx = Random.Range(1, points.Length);
        Instantiate(Chest[i], points[idx].position, points[idx].rotation);
        
    }
}
