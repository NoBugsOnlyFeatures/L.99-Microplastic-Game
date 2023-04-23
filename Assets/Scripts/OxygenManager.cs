using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Assertions.Comparers;
using Mono.Cecil.Cil;
using System;

public enum OxygenLevel
{
    TEST,
    DAY1,
    DAY2,
    DAY3
}

public class OxygenManager : MonoBehaviour
{
    public static Action OnOxygenEmpty;
    [SerializeField] private Slider _oxygenBar;
    [SerializeField] private Gradient _oxygenGradient;
    [SerializeField] private Image _oxygenBarForeground;
    [SerializeField] private float _breathingGameLength = 60.0f;
    [SerializeField] private int _miniGameFillFactor = 6; 
    private float _breathingMaxFill;

    private bool _isAlive = true;
    private bool _countdownStarted = false;
    private bool _hitBubbleRange = false;
    [SerializeField] private BubbleManager _currentBubble;


    [SerializeField]
    private float TEST_FULL_OXYGEN_IN_MINUTES = 1.0f;

    [SerializeField]
    private float DAY1_FULL_OXYGEN_IN_MINUTES = 2.5f;

    [SerializeField]
    private const float DAY2_FULL_OXYGEN_IN_MINUTES = 3.0f;
    [SerializeField]
    private const float DAY3_FULL_OXYGEN_IN_MINUTES = 3.5f;

    [SerializeField]
    private float _currentOxygenTankInSeconds, _currentTimerFillInSeconds = 0.0f;

    [SerializeField]
    private const float OXYGEN_DEPLETION_RATE_PER_SECOND = 0.01f;

    [SerializeField]
    private const float OXYGEN_DEPLETION_TIME = 1.0f;

    private OxygenLevel _currentOxygenLevel;

    private float _currentLevelOxygenMaxInSeconds;

    private bool _isUnderWater = false;

    private bool _initialized = false;

    public bool IsUnderWater
    {
        get => _isUnderWater;
        set
        {
            if (_isUnderWater != value && value)
            {
                StartCoroutine(OxygenTick());
                _countdownStarted = true;
            }
            _isUnderWater = value;
        }
    }

    void Awake()
    {
        _currentBubble = GameObject.Find("AirBubble").GetComponent<BubbleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_initialized)
        {
            _isAlive = _currentOxygenTankInSeconds > 0;

            if (!_isAlive && _isUnderWater)
            {
                // EditorApplication.isPlaying = false;
                _currentTimerFillInSeconds = 0.0f;
                OnOxygenEmpty?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Space) && _isUnderWater)
            { 
                _hitBubbleRange = _currentBubble.IsInRange();
            }
        }
    }

    public void AddOxygenBySec(float fillSeconds)
    {
        _currentTimerFillInSeconds += fillSeconds;
        if (_currentTimerFillInSeconds >= _breathingMaxFill)
        {
            _currentTimerFillInSeconds = _breathingMaxFill;
        }

        _currentOxygenTankInSeconds +=  (_currentLevelOxygenMaxInSeconds / _breathingMaxFill) * _currentTimerFillInSeconds ;
        if (_currentOxygenTankInSeconds >= _currentLevelOxygenMaxInSeconds)
        {
            _currentOxygenTankInSeconds = _currentLevelOxygenMaxInSeconds;
        }
        else if (_currentOxygenTankInSeconds <= 0.0f)
        {
            _currentOxygenTankInSeconds = 0.0f;
        }

        UpdateOxygenBar();
    }

    public void SetOxygenLimit(bool isTestRun)
    {
         _currentOxygenLevel = isTestRun ? OxygenLevel.TEST : OxygenLevel.DAY1;
        SetOxygenLevelForLevel();
        FillOxygenBarByPercent(/* percent */ 0.01f);
        _initialized = true;
    }

    public void ResetOxygen()
    {
        _initialized = false;
        _hitBubbleRange = false;
        _countdownStarted = false;
        _isAlive = true;

        _currentOxygenTankInSeconds = 0.0f;
        _currentTimerFillInSeconds = 0.0f;
    }

    private void UpdateOxygenBar()
    {
        var fillAmount = (_currentLevelOxygenMaxInSeconds - (_currentLevelOxygenMaxInSeconds - _currentOxygenTankInSeconds)) / _currentLevelOxygenMaxInSeconds;
        _oxygenBar.value = fillAmount;
        _oxygenBarForeground.color = _oxygenGradient.Evaluate(fillAmount);
    }

    private IEnumerator OxygenTick()
    {
        while (_isAlive)
        {
            DepleteOxygen();
            _hitBubbleRange = false;
            yield return new WaitForSeconds(OXYGEN_DEPLETION_TIME);
        }
    }

    private void DepleteOxygen()
    {
        if (_countdownStarted)
        {
            if (!_hitBubbleRange)
            {
                _currentOxygenTankInSeconds -= 1;
                UpdateOxygenBar();
            }
        }
    }

    private void SetOxygenLevelForLevel()
    {
        _currentLevelOxygenMaxInSeconds = _currentOxygenLevel switch
        {
            OxygenLevel.TEST => TEST_FULL_OXYGEN_IN_MINUTES * 60.0f,
            OxygenLevel.DAY1 => DAY1_FULL_OXYGEN_IN_MINUTES * 60.0f,
            OxygenLevel.DAY2 => DAY2_FULL_OXYGEN_IN_MINUTES * 60.0f,
            OxygenLevel.DAY3 => DAY3_FULL_OXYGEN_IN_MINUTES * 60.0f,
            _ => DAY1_FULL_OXYGEN_IN_MINUTES * 60.0f,
        };

        _breathingMaxFill = _currentLevelOxygenMaxInSeconds / _miniGameFillFactor;
    }

    private void FillOxygenBarByPercent(float percent)
    {
        _currentOxygenTankInSeconds = _currentLevelOxygenMaxInSeconds * percent;
        _oxygenBar.value = percent;
        _oxygenBarForeground.color = _oxygenGradient.Evaluate(percent);
    }
}
