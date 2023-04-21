using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private MainGameManager _gameManager;
    private GameObject _titleScreenUI;
    private GameObject _startButtonGameObject, _instructionsGameObject, _quitGameObject;
    private Button _startButton, _instructionsButton, _quitButton;

    private GameObject _oxygenCanvasGameObject, _airBubbleGameObject, _oxygenGaugeGameObject, _oxygenBarGameObject, _countDownTimerGameObject, _countDownBackgroundObject;

    private GameObject _urchinCanvasGameObject;
    private TextMeshProUGUI urchinText;
    private Image _breathingCountDownImage;

    private float _currentBreathingTime = 0.0f;
    [SerializeField] private float _breathingGameLength = 10.0f;
    [SerializeField] private Gradient _countDownGradient;

    private bool _isBreathingGameActive = false;
    public bool IsBreathingGameActive => _isBreathingGameActive;
    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();

        // Set Title Screen UI
        _titleScreenUI = GameObject.Find("TitleScreen");

        _startButtonGameObject = _titleScreenUI.transform.GetChild(1).gameObject;
        _instructionsGameObject = _titleScreenUI.transform.GetChild(2).gameObject;
        _quitGameObject = _titleScreenUI.transform.GetChild(3).gameObject;
        
        _startButton = _startButtonGameObject.GetComponent<Button>();
        _instructionsButton = _instructionsGameObject.GetComponent<Button>();
        _quitButton = _quitGameObject.GetComponent<Button>();

        // Set Oxygen Related UI
        _oxygenCanvasGameObject = GameObject.Find("OxygenCanvas");
        _airBubbleGameObject = GameObject.Find("AirBubble");
        _oxygenGaugeGameObject = GameObject.Find("OxygenGauge");
        _oxygenBarGameObject = GameObject.Find("OxygenBar");

        _countDownBackgroundObject = GameObject.Find("BreathingCountdownBackground");
        _countDownTimerGameObject = GameObject.Find("BreathingGameCountdownTimer");
        _breathingCountDownImage = _countDownTimerGameObject.GetComponent<Image>();


        // Set Urchin Related UI
        _urchinCanvasGameObject = GameObject.Find("UrchinCanvas");
        urchinText = _urchinCanvasGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonClicked);

        _oxygenCanvasGameObject.SetActive(false);
        _urchinCanvasGameObject.SetActive(false);
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

        _countDownTimerGameObject.SetActive(true);
        _isBreathingGameActive = true;

    }

    private void EnableDive()
    {
        EnableOxygenGaugeUI(false);
        EnableAirBubbleUI(true);

        _urchinCanvasGameObject.SetActive(true);
        _gameManager.BeginDive();
    }

    void UpdateBreathingGameCountdown()
    {
        var fillAmount = (_breathingGameLength - _currentBreathingTime) / _breathingGameLength;
        _breathingCountDownImage.fillAmount = fillAmount;
        _breathingCountDownImage.color = _countDownGradient.Evaluate(fillAmount);
    }
}
