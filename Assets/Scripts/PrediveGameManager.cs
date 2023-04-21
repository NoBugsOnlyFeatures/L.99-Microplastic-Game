using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions.Comparers;

enum GameState
{
    TITLE,
    BREATHING,
    DIVING,

    LOADING
}
public class PrediveGameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas _sceneCanvas;
    private GameObject _playerObject;
    [SerializeField] private PlayerBehavior _player;
    private GameObject _airBubbleGameObject, _timerCircleGameObject, _oxygenBarGameObject;
    private OxygenManager _oxygenManager;
    private BubbleManager _airBubbleManager;
    private TimerCircleTick _timerCircleManager;

    private TMP_Text _countdownTimerText;
    [SerializeField] private float _breathingGameLength = 10.0f;
    private float _miniGameCountdownTimer = 0.0f;

    private GameState _currentState;

    private bool _playerHasStoppedMoving = false;
    private float _diveStartTime, _diveSpeed, _diveLength;

    private Vector2 _diverStartPosition, _diverEndPositon;
    
    void Awake()
    {
        _playerObject = GameObject.Find("Player");
        _player = _playerObject.GetComponent<PlayerBehavior>();

        _airBubbleGameObject = GameObject.Find("AirBubble");
        _timerCircleGameObject = GameObject.Find("TimerCircles");
        _oxygenBarGameObject = GameObject.Find("OxygenBar");

        _airBubbleManager = _airBubbleGameObject.GetComponent<BubbleManager>();
        _timerCircleManager = _timerCircleGameObject.GetComponent<TimerCircleTick>();
        _oxygenManager = _oxygenBarGameObject.GetComponent<OxygenManager>();

        _countdownTimerText = GameObject.Find("BreathingCountdown").GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentState = GameState.TITLE;

        _playerObject.SetActive(false);

        _airBubbleGameObject.SetActive(false);
        _timerCircleGameObject.SetActive(false);
        _oxygenBarGameObject.SetActive(false);
        _countdownTimerText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == GameState.BREATHING)
        {
            _miniGameCountdownTimer += Time.deltaTime;
            _countdownTimerText.text = Mathf.FloorToInt(_breathingGameLength - _miniGameCountdownTimer).ToString();
            if (_miniGameCountdownTimer >= _breathingGameLength)
            {
                StartCoroutine(StartDive());
                _miniGameCountdownTimer = 0.0f;
            }

            
        }
        
        if (_currentState == GameState.DIVING)
        {
            if (_playerObject.transform.position.y <= -10.0f && !_playerHasStoppedMoving)
            {
                _playerHasStoppedMoving = true;

                StartCoroutine(LoadLevel());
                // _player.StopMoving();

                // _airBubbleGameObject.SetActive(true);
                // _oxygenManager.SetIsUnderWater(true);
            }
            else if (!_playerHasStoppedMoving)
            {
                float distCovered = (Time.time - _diveStartTime) * _diveSpeed;

                // Fraction of journey completed equals current distance divided by total distance.
                float fractionOfJourney = distCovered / _diveLength;

                // Set our position as a fraction of the distance between the markers.
                _playerObject.transform.position = Vector2.Lerp(_diverStartPosition, _diverEndPositon, fractionOfJourney);
            }

        }
    }
    
    private IEnumerator StartDive()
    {
        // _player.TriggerDiveAnimation();
        _countdownTimerText.text = string.Empty;

        while(_player.IsDiving)
        {
            yield return null;
        }

        _diverStartPosition = _playerObject.transform.position;
        _diverEndPositon = new Vector3(_diverStartPosition.x, -10);

        _diveStartTime = Time.time;
        _diveSpeed = 3.0f;
        _diveLength = Vector2.Distance(_diverStartPosition, _diverEndPositon);

        _timerCircleGameObject.SetActive(false);

        _currentState = GameState.DIVING;
    }

    private IEnumerator LoadLevel()
    {
         //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainGame");
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;

        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Wait to you press the space key to activate the Scene
                if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void StartGame()
    {
        // _titleScreenUI.SetActive(false);

        _playerObject.SetActive(true);
        _timerCircleGameObject.SetActive(true);
        _oxygenBarGameObject.SetActive(true);

        _countdownTimerText.text = _breathingGameLength.ToString();

        _currentState = GameState.BREATHING;
    }
}
