using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class UrchinSpawner : MonoBehaviour
{
    float xMax = 42f;
    float xMin = -4f;
    float yStandard = 0.5f;

    [SerializeField] uint numberofUrchins = 60;

    [SerializeField] GameObject urchin;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SpawnUrchins()
    {
        for(int i = 0; i < numberofUrchins; i++){
            float x = Random.Range(xMin,xMax);
            var y = yStandard;

            //A stupid hack to get urchins to spawn below the screen
            if(x >= 8.95f && x <= 20.5f){
                y = -2.76f;
            }

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

    public int GetSpwanedUrchinCount()
    {
        return (int)numberofUrchins;
    }
}
