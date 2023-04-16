using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(BubbleManager))]
public class OxygenManager : MonoBehaviour
{
    [SerializeField] private Slider _oxygenBar;
    [SerializeField] private Gradient _oxygenGradient;
    [SerializeField] private Image _oxygenBarForeground;
    [SerializeField] private readonly float _totalOxygen = 50.0f;
    [SerializeField] private float _currentOxygen = 50.0f;
    [SerializeField] private float _depletionRate = 0.1f; // rate we deplete oxygen at
    [SerializeField] private float _oxygeDepletionTime = 5.0f; // how often we decrease oxygen

    private bool _isAlive = true;
    private bool _countdownStarted = false;
    [SerializeField] private bool _isBubbleInRange = false;
    [SerializeField] private BubbleManager _currentBubble;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OxygenTick());
        _countdownStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        _isAlive = _currentOxygen > 0 ? true : false;

        if (!_isAlive)
        {
            EditorApplication.isPlaying = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Player clicked");
            var deltaOxygen = _currentBubble.BubbleInRange ? 5.0f : -5.0f;
            Debug.Log(_currentBubble.BubbleInRange ? "Gained Oxygen!" : "Lost Oxygen");
            _currentOxygen += deltaOxygen;
            _currentOxygen = Mathf.Clamp(_currentOxygen, 0, _totalOxygen);
            UpdateOxygenBar();
        }
    }

    private void UpdateOxygenBar()
    {
        var fillAmount = (_totalOxygen - (_totalOxygen - _currentOxygen)) / _totalOxygen;
        _oxygenBar.value = fillAmount;
        _oxygenBarForeground.color = _oxygenGradient.Evaluate(fillAmount);
    }

    private IEnumerator OxygenTick()
    {
        while (_isAlive)
        {
            DepleteOxygen();
            yield return new WaitForSeconds(_oxygeDepletionTime);
        }
    }

    private void DepleteOxygen()
    {
        if (_countdownStarted)
        {
            _currentOxygen -= (_totalOxygen * _depletionRate);
            UpdateOxygenBar();
        }
    }
}
