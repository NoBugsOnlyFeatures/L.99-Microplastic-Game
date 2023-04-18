using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private TextMeshProUGUI urchinText;
    // Start is called before the first frame update
    void Start()
    {
        urchinText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateDiamondText(PlayerBehavior player){
        urchinText.text = player.NumberOfUrchins.ToString();
    }
}
