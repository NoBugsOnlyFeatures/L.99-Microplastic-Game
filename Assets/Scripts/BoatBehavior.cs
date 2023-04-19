using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoatBehavior : MonoBehaviour
{
    [SerializeField] public uint NumberOfUrchins {get; set;} = 0;
    public UnityEvent<BoatBehavior> OnDepositUrchins;
    
    bool isBoatFull = false;

    public void DepositUrchins(uint numberOfUrchins){
        NumberOfUrchins += numberOfUrchins;
        isBoatFull = true;
        OnDepositUrchins.Invoke(this);
        Debug.Log("Urchins deposited");
    }
}
