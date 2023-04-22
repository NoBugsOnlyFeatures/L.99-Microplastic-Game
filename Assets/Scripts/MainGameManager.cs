using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;

    private GameObject _diverGameObject;
    private PlayerBehavior _player;

    private UrchinSpawner _urchinSpawner;
    private OxygenManager _oxygenManager;

    private Vector3 _initialDiverPosition;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _diverGameObject = GameObject.Find("Diver");
        _initialDiverPosition = _diverGameObject.transform.position;

        _player = _diverGameObject.GetComponent<PlayerBehavior>();
        _oxygenManager = GameObject.Find("OxygenBar").GetComponent<OxygenManager>();

        _urchinSpawner = GameObject.Find("UrchinSpawner").GetComponent<UrchinSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _diverGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        _diverGameObject.transform.position = _initialDiverPosition;
        _diverGameObject.SetActive(true);
    }

    public void BeginDive()
    {
        _urchinSpawner.SpawnUrchins();
        _player.IsDiving = true;
        _oxygenManager.IsUnderWater = _player.IsDiving;
    }

    public void ResetGame()
    {
        _diverGameObject.SetActive(false);

        _player.ResetUrchinCount();
        _player.IsDiving = false;
        _oxygenManager.IsUnderWater = false;

        _urchinSpawner.DeleteAllUrchins();
    }
}
