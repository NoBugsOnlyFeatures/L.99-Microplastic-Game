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

    private GameObject _startButtonGameObject, _instructionsGameObject, _quitGameObject;
    private Button _startButton, _instructionsButton, _titleQuitButton, _instructionsBackButton;

    private Button _retryButton, _gameOverQuitButton;

    private GameObject _oxygenCanvasGameObject, _airBubbleGameObject,
    _oxygenGaugeGameObject, _oxygenBarGameObject, _countDownTimerGameObject, 
    _countDownBackgroundObject, _instructionsObject;

    private GameObject _urchinCounterGameObject;
    private TextMeshProUGUI urchinText;
    private Image _breathingCountDownImage;

    private float _currentBreathingTime = 0.0f;
    [SerializeField] private float _breathingGameLength = 60.0f;
    [SerializeField] private Gradient _countDownGradient;

    private bool _isBreathingGameActive = false;
    public bool IsBreathingGameActive => _isBreathingGameActive;
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();

        // Set Title Screen UI
        _titleScreenUI = GameObject.Find("TitleScreen");
        _titleScreenButtonUI = GameObject.Find("TitleButtons");
        _gameOverPanel = GameObject.Find("GameOverPanel");
        _instructionsBackButton = GameObject.Find("BackButton").GetComponent<Button>();
        _instructionsObject = GameObject.Find("Instructions");

        _startButtonGameObject = _titleScreenButtonUI.transform.GetChild(0).gameObject;
        _instructionsGameObject = _titleScreenButtonUI.transform.GetChild(1).gameObject;
        _quitGameObject = _titleScreenButtonUI.transform.GetChild(2).gameObject;
        
        _startButton = _startButtonGameObject.GetComponent<Button>();
        _instructionsButton = _instructionsGameObject.GetComponent<Button>();
        _titleQuitButton = _quitGameObject.GetComponent<Button>();

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


        // Set Urchin Related UI
        _urchinCounterGameObject = GameObject.Find("UrchinCounter");
        urchinText = _urchinCounterGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _titleQuitButton.onClick.AddListener(OnQuitButtonClicked);
        _instructionsButton.onClick.AddListener(() => ToggleInstructions(true));

        _retryButton.onClick.AddListener(OnRetryButtonClicked);
        _gameOverQuitButton.onClick.AddListener(OnQuitButtonClicked);
        _instructionsBackButton.onClick.AddListener(() => ToggleInstructions(false));

        _instructionsObject.SetActive(false);
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

    void OnStartButtonClicked()
    {
        _titleScreenUI.SetActive(false);

        EnableBreathingMinigame();
        _gameManager.StartGame();
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
        _breathingCountDownImage.color = _countDownGradient.Evaluate(fillAmount);
    }

    private void HandleOxygenDepleted()
    {
        ResetUI();
    }

    private void ResetUI()
    {
        _oxygenCanvasGameObject.SetActive(false);
        _urchinCounterGameObject.SetActive(false);

        EnableOxygenBarUI(false);
        EnableOxygenGaugeUI(false);
        EnableAirBubbleUI(false);
        _currentBreathingTime = 0.0f;

        _gameManager.ResetGame();

        urchinText.text = "0";

        _gameOverPanel.SetActive(true);
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnRetryButtonClicked()
    {
        _gameOverPanel.SetActive(false);
        
        // EnableBreathingMinigame();
        // _gameManager.StartGame();
        OnStartButtonClicked();
    }

    private void ToggleInstructions(bool instructionsVisible)
    {
        _titleScreenButtonUI.SetActive(!instructionsVisible);
        _instructionsObject.SetActive(instructionsVisible);
    }
}
