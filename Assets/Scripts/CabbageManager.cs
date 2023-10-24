using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbageManager : MonoBehaviour
{
    public GameObject[] cabbagePrefab = new GameObject[5];
    [Space]
    public GameObject[] spawnPoints;
    public GameObject group;
    public List<GameObject> lastAdded;

    public void SpawnCabbage(int amount)
    {
        lastAdded.Clear();

        int cabbageColor;
        switch(amount)
        {
            case 1:
                cabbageColor = 1;
                break;
            case 2:
                cabbageColor = 2;
                break;
            case 3:
                cabbageColor = 3;
                break;
            case 4:
                cabbageColor = 4;
                break;
            default:
                cabbageColor = 0;
                break;
        }

        for(int i = amount; i > 0; i--)
        {
            int spawnpoint = Random.Range(0, spawnPoints.Length - 1);
            lastAdded.Add(Instantiate(cabbagePrefab[cabbageColor], spawnPoints[spawnpoint].transform.position, spawnPoints[spawnpoint].transform.rotation, spawnPoints[spawnpoint].transform));
        }
    }

    public void UndoCabbage()
    {
        for(int i = 0; i < lastAdded.Count; i++)
        {
            Destroy(lastAdded[i]);
        }
        lastAdded.Clear();
    }
}
