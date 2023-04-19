using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI urchinText;
    void Start()
    {
        urchinText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdatePlayerUrchinsText(BoatBehavior boat){
        urchinText.text = boat.NumberOfUrchins.ToString();
    }
}
