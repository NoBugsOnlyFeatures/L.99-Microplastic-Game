using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class UrchinCounterManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _urchinCounterSprites;
    private Sprite _currentSpirte;

    [SerializeField]
    private int _maxUrchinCount = 60;
    private int _updateInterval;
    private Image _currentImage;
    // Start is called before the first frame update

    void Awake()
    {
        _currentImage = GetComponent<Image>();
    }
    void Start()
    {
        _updateInterval = Mathf.FloorToInt(_maxUrchinCount / _urchinCounterSprites.Length);
        _currentImage.sprite = _urchinCounterSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerCollectUrchin(PlayerBehavior player)
    {
        var urchinCount = (int)player.NumberOfUrchinsOnPlayer;
        _currentImage.sprite = GetSpriteByUrchinCount(urchinCount);
    }

    private Sprite GetSpriteByUrchinCount(int urchinCount)
    {
        var index = Mathf.FloorToInt(urchinCount / _updateInterval);
        Debug.Log("Urchin sprite index: " + index);
        if (index >= _urchinCounterSprites.Length - 1)
        {
            index = _urchinCounterSprites.Length - 1;
        }
        
        return _urchinCounterSprites[index];
    }
}
