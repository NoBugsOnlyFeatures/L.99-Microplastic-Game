using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TimerFill : MonoBehaviour
{
    [SerializeField]
    private Image _playerFill;

    [SerializeField]
    private TimerCircleTick _tick;

    private float _perfectMinAngle = 89.8f; 
    private float _perfectMaxAngle = 118.2f;
    
    private float _exitMaxAngle = 270.0f;
    private float _exitMinAngle = 255f;
    
    [SerializeField] private bool _isHoldingSpace, _canAddToFill, _hitFillBonus = false;

    [SerializeField] private float _fillBySecond = 0.0f;

    /* [SerializeField] private Slider _oxygenBar;
    [SerializeField] private Gradient _oxygenGradient;
    [SerializeField] private Image _oxygenBarForeground; */

    [SerializeField]
    private const float PERFECT_TIMING_BONUS = 0.1f;

    [SerializeField]
    private const float INCORRECT_RELEASE_PENALTY = 0.03f;

    [SerializeField]
    private OxygenManager _oxygenManager;
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
                    // _fillBySecond = 10.0f;
                    _fillBySecond = PERFECT_TIMING_BONUS;
                    _hitFillBonus = true;
                }
                else
                {
                    _fillBySecond = 0.0f;
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
                    // _fillBySecond *= 0.5f;
                    _fillBySecond -= INCORRECT_RELEASE_PENALTY;
                }

                if (_canAddToFill)
                {
                    // _totalFill += _fillAmount * Time.deltaTime;

                    /* var oxygenFill = _totalFill / _fillMax;
                    _oxygenBar.value = oxygenFill;
                    _oxygenBarForeground.color = _oxygenGradient.Evaluate(oxygenFill);*/

                    _oxygenManager.AddOxygenBySec(_fillBySecond);
                }
            }

            _playerFill.fillAmount = 0.0f;
            _hitFillBonus = false;
            _canAddToFill = false;
            _fillBySecond = 0f;
        }

        if (_isHoldingSpace)
        {
            if (playerAngle >= _perfectMinAngle && playerAngle < _exitMaxAngle)
            {
                // _fillAmount += _fillRate;
                _fillBySecond += Time.deltaTime;
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
