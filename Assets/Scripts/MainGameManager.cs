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
    private UIManager _uiManager;
    private AudioSource _audioSource;
    private CameraManager _cameraManager;


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
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        _audioSource = GetComponent<AudioSource>();
        _cameraManager = Camera.main.gameObject.GetComponent<CameraManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _diverGameObject.SetActive(false);
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame(bool isTestRun)
    {
        _diverGameObject.transform.position = _initialDiverPosition;
        _diverGameObject.SetActive(true);

        _cameraManager.GameStarted = true;
        _oxygenManager.SetOxygenLimit(isTestRun);
    }

    public void BeginDive()
    {
        _player.StartDiving();
        _urchinSpawner.SpawnUrchins();
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

    public void OnPlayerCollectUrchin(PlayerBehavior player)
    {
        if (player.NumberOfUrchinsOnPlayer >= _urchinSpawner.GetSpwanedUrchinCount())
        {
            _uiManager.HandlePlayerWin();
        }
    }
}
