using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private MainGameManager _gameManager;
    private GameObject _titleScreenUI, _titleScreenButtonUI;
    private GameObject _gameOverPanel;

    private AudioSource _audio;
    [SerializeField] private AudioClip _stampSound;

    private GameObject _startButtonGameObject, _testRunGameObject, _quitGameObject;
    private Button _startButton, _testRunButton, _titleQuitButton;

    private Button _retryButton, _gameOverQuitButton;

    private GameObject _oxygenCanvasGameObject, _airBubbleGameObject,
    _oxygenGaugeGameObject, _oxygenBarGameObject, _countDownTimerGameObject, 
    _countDownBackgroundObject;

    private GameObject _urchinCounterGameObject;
    private TextMeshProUGUI urchinText;
    private Image _breathingCountDownImage;

    private float _currentBreathingTime = 0.0f;
    [SerializeField] private float _breathingGameLength = 60.0f;

    private bool _isBreathingGameActive = false;

    private TMP_Text _gameOverText;
    public bool IsBreathingGameActive => _isBreathingGameActive;
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();

        // Set Title Screen UI
        _titleScreenUI = GameObject.Find("TitleScreen");
        _titleScreenButtonUI = GameObject.Find("TitleButtons");
        _gameOverPanel = GameObject.Find("GameOverPanel");

        _startButtonGameObject = _titleScreenButtonUI.transform.GetChild(0).gameObject;
        _testRunGameObject = _titleScreenButtonUI.transform.GetChild(1).gameObject;
        _quitGameObject = _titleScreenButtonUI.transform.GetChild(2).gameObject;
        
        _startButton = _startButtonGameObject.GetComponent<Button>();
        _testRunButton = _testRunGameObject.GetComponent<Button>();
        _titleQuitButton = _quitGameObject.GetComponent<Button>();

        _gameOverText = _gameOverPanel.transform.GetChild(1).gameObject.GetComponent<TMP_Text>();
        _retryButton = _gameOverPanel.transform.GetChild(2).gameObject.GetComponent<Button>();
        _gameOverQuitButton = _gameOverPanel.transform.GetChild(3).gameObject.GetComponent<Button>();

        // Set Oxygen Related UI
        _oxygenCanvasGameObject = GameObject.Find("OxygenCanvas");
        _airBubbleGameObject = GameObject.Find("AirBubble");
        _oxygenGaugeGameObject = GameObject.Find("OxygenGauge");
        _oxygenBarGameObject = GameObject.Find("OxygenBar");

        _countDownBackgroundObject = GameObject.Find("BreathingCountdownBackground");
        _countDownTimerGameObject = GameObject.Find("BreathingGameCountdownTimer");
        _breathingCountDownImage = _countDownTimerGameObject.GetComponent<Image>();

        // Set Audio
        _audio = GetComponent<AudioSource>();

        // Set Urchin Related UI
        _urchinCounterGameObject = GameObject.Find("UrchinCounter");
        urchinText = _urchinCounterGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        
        _startButton.onClick.AddListener(() => OnStartButtonClicked(/* isTestRun */ false));
        _titleQuitButton.onClick.AddListener(OnQuitButtonClicked);
        _testRunButton.onClick.AddListener(() => OnStartButtonClicked(/* isTestRun */ true));

        _retryButton.onClick.AddListener(OnRetryButtonClicked);
        _gameOverQuitButton.onClick.AddListener(OnQuitButtonClicked);

        _gameOverPanel.SetActive(false);

        _oxygenCanvasGameObject.SetActive(false);
        _urchinCounterGameObject.SetActive(false);

        OxygenManager.OnOxygenEmpty += HandleOxygenDepleted;
    }

    void Update()
    {
        if (_isBreathingGameActive)
        {
            _currentBreathingTime += Time.deltaTime;
            if (_currentBreathingTime >= _breathingGameLength)
            {
                _currentBreathingTime = _breathingGameLength;
                _isBreathingGameActive = false;

                _countDownBackgroundObject.SetActive(false);
                _countDownTimerGameObject.SetActive(false);
                EnableDive();
            }
            else
            {
                UpdateBreathingGameCountdown();
            }
        }
    }

    public void UpdatePlayerUrchinsText(PlayerBehavior player){
        urchinText.text = player.NumberOfUrchinsOnPlayer.ToString();
    }

    public void EnableOxygenBarUI(bool isEnabled)
    {
        _oxygenBarGameObject.SetActive(isEnabled);
    }

    public void EnableAirBubbleUI(bool isEnabled)
    {
        _airBubbleGameObject.SetActive(isEnabled);
    }

    public void EnableOxygenGaugeUI(bool isEnabled)
    {
        _oxygenGaugeGameObject.SetActive(isEnabled);
    }

    public void HandlePlayerWin()
    {
        ResetUI(/* isGameOver */ false);
    }

    void OnStartButtonClicked(bool isTestRun)
    {
        _audio.PlayOneShot(_stampSound);
        _titleScreenUI.SetActive(false);

        EnableBreathingMinigame();
        _gameManager.StartGame(isTestRun);
    }

    void EnableBreathingMinigame()
    {
        _oxygenCanvasGameObject.SetActive(true);

        EnableOxygenBarUI(true);
        EnableOxygenGaugeUI(true);
        EnableAirBubbleUI(false);

        _countDownBackgroundObject.SetActive(true);
        _countDownTimerGameObject.SetActive(true);

        _isBreathingGameActive = true;

    }

    private void EnableDive()
    {
        EnableOxygenGaugeUI(false);
        EnableAirBubbleUI(true);

        _urchinCounterGameObject.SetActive(true);
        _gameManager.BeginDive();
    }

    void UpdateBreathingGameCountdown()
    {
        var fillAmount = (_breathingGameLength - _currentBreathingTime) / _breathingGameLength;
        _breathingCountDownImage.fillAmount = fillAmount;
    }

    private void HandleOxygenDepleted()
    {
        ResetUI(/* isGameOver */ true);
    }

    private void ResetUI(bool isGameOver)
    {
        _oxygenCanvasGameObject.SetActive(false);
        _urchinCounterGameObject.SetActive(false);

        EnableOxygenBarUI(false);
        EnableOxygenGaugeUI(false);
        EnableAirBubbleUI(false);
        _currentBreathingTime = 0.0f;

        _gameManager.ResetGame();

        urchinText.text = "0";

        _gameOverText.text = isGameOver ? "Game Over" : "You Win!";
        _gameOverPanel.SetActive(true);
    }

    private void OnQuitButtonClicked()
    {
        _audio.PlayOneShot(_stampSound);
        Application.Quit();
    }

    private void OnRetryButtonClicked()
    {
        _audio.PlayOneShot(_stampSound);
        _gameOverPanel.SetActive(false);

        OnStartButtonClicked(/*isTestRun */ false);
    }
}
