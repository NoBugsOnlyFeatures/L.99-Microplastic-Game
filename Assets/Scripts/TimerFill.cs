using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class TimerFill : MonoBehaviour
{
    [SerializeField]
    private Image _playerFill;

    [SerializeField]
    private TimerCircleTick _tick;

    private float _perfectMinAngle = 89.8f; 
    private float _perfectMaxAngle = 118.2f;
    
    private float _normalMinAngle = 118.3f;
    private float _exitMaxAngle = 270.0f;
    private float _exitMinAngle = 255f;
    
    [SerializeField] private bool _isHoldingSpace, _canAddToFill, _hitFillBonus = false;

    [SerializeField] private float _fillAmount, _totalFill = 0.0f;
    private float _fillRate = 5.0f;

    private float _fillMax = 100.0f;

    [SerializeField] private Slider _oxygenBar;
    [SerializeField] private Gradient _oxygenGradient;
    [SerializeField] private Image _oxygenBarForeground;

    // Start is called before the first frame update
    void Start()
    {
        _playerFill.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        var playerAngle = _tick.GetTickAngle();
    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Ensure the player is in the do something zone
            if (playerAngle >= _perfectMinAngle && playerAngle < _exitMinAngle)
            {
                _isHoldingSpace = true;
                _canAddToFill = true;

                if (playerAngle <= _perfectMaxAngle)
                {
                    _fillAmount = 10.0f;
                    _hitFillBonus = true;
                }
                else
                {
                    _fillAmount = 0.0f;
                }

                SetFillAmount(playerAngle);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isHoldingSpace = false;

            if (playerAngle > _perfectMinAngle && playerAngle < _exitMaxAngle)
            {
                // lower fill by half if land in red zone
                if (playerAngle > _exitMinAngle)
                {
                    _fillAmount *= 0.5f;
                }

                if (_canAddToFill)
                {
                    _totalFill += _fillAmount * Time.deltaTime;

                    var oxygenFill = _totalFill / _fillMax;
                    _oxygenBar.value = oxygenFill;
                    _oxygenBarForeground.color = _oxygenGradient.Evaluate(oxygenFill);
                }
            }

            _tick.Reset();
            _playerFill.fillAmount = 0.0f;
            _hitFillBonus = false;
            _canAddToFill = false;
        }

        if (_isHoldingSpace)
        {
            if (playerAngle >= _perfectMinAngle && playerAngle < _exitMaxAngle)
            {
                _fillAmount += _fillRate;
                SetFillAmount(playerAngle);
            }
        }
    }


    private void SetFillAmount(float angle)
    {
        var fillAmount = (angle - 90.0f) / 360.0f;
        _playerFill.fillAmount = fillAmount;
    }
}
