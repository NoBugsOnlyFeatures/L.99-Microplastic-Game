using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinSpawner : MonoBehaviour
{
    int xMax = 42;
    int xMin = -4;
    int y = 2;

    [SerializeField] GameObject urchin;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 150; i++){
            int x = Random.Range(xMin,xMax);

            Instantiate(urchin, new Vector3(x,y,0), Quaternion.identity);
        }
    }
}