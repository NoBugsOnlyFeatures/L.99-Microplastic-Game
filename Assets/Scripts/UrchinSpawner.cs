using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinSpawner : MonoBehaviour
{
    int xMax = 42;
    int xMin = -4;
    int y = 0;

    [SerializeField] uint numberofUrchins = 60;

    [SerializeField] GameObject urchin;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnUrchins()
    {
        for(int i = 0; i < numberofUrchins; i++){
            int x = Random.Range(xMin,xMax);

            Instantiate(urchin, new Vector3(x,y,0), Quaternion.identity);
        }
    }



    public void DeleteAllUrchins()
    {
        var urchins = GameObject.FindGameObjectsWithTag("Urchin");
        foreach (var urchin in urchins)
        {
            Destroy(urchin);
        }
    }
}
