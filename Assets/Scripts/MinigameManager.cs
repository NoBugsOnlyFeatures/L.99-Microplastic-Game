using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;

public enum NoteType
{
    QUARTER,
    HALF,
    WHOLE,
    NONE,
}
public class Note
{
    private int _beatIndex;
    private NoteType _noteType;
    public Note (int beatIndex, NoteType noteType)
    {
        _beatIndex = beatIndex;
        _noteType = noteType;
    }

    public int BeatIndex
    {
        get => _beatIndex;
        private set =>  _beatIndex = value;
    }

    public NoteType NoteType
    {
        get => _noteType;
    }
}

public class MinigameManager : MonoBehaviour
{
    [SerializeField] private Slider _oxygenBar;
    [SerializeField] private Gradient _oxygenGradient;
    [SerializeField] private Image _oxygenBarForeground;

    [SerializeField] private Image _hitBoxImage;
    [SerializeField] private GameObject _beatPrefab;
    [SerializeField] private Transform _beatSpawner;
    [SerializeField] private readonly float _maxOxygenAmount = 100.0f;

    [SerializeField] private Transform _minHitBox, _maxHitBox;

    private GameObject _canvas;

    private float _currentOxygen = 0.0f;

    private float _lastTime, _deltaTime, _currentTime;
    private float bpm = 20f;
    private float sampleLength = 60f; // in seconds;

    [SerializeField] private GameObject _triangle;
    private float initalX;
    [SerializeField] private Transform _startingTransform;
    [SerializeField] private Transform _endingTransform;

    private float _movementAmplitude, omega, secondsPerMeasure, period, _timePassed;
    private float _minHitX, _maxHitX;

    private bool _inRange = false;
    private float _rangeThreshold = 1.5f;

    // Start is called before the first frame update
    /*void Start()
    {
        UpdateOxygenBar();

        _lastTime = 0f;
        _deltaTime = 0f;
        _currentTime = 0f;

        _triangle = GameObject.Find("Triangle");
        secondsPerMeasure = 240f / bpm;

        _movementAmplitude = Mathf.Abs(_endingTransform.position.x - _startingTransform.position.x);
        Debug.Log("Movement Amplitude: " + _movementAmplitude);
        period = (2 * secondsPerMeasure); // a period of a quarter note
        omega = (2 * Mathf.PI) / period;

        _minHitX = _minHitBox.position.x;
        _maxHitX = _maxHitBox.position.x;
    }*/

    // Update is called once per frame
    /*void Update()
    {
        Oscillate();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var triangleX = _triangle.transform.position.x;

            
            if (triangleX >= _minHitX &&  triangleX <= _maxHitX)
            {
                _currentOxygen += 10;
                Debug.Log("Current Oxygen: " + _currentOxygen);
                UpdateOxygenBar();
            }
        }
    }*/

    private void FixedUpdate() 
    {
        
    }

    private void CreateRhythm()
    {
        // create a list of ints that should the note the player has to hit the 
        var numMeasures = (bpm * sampleLength) / 240f;
        

        for (int i = 0; i < numMeasures; i++)
        {
            Debug.Log($"Start beat number for measure {i} is {(i * secondsPerMeasure)}," +
            $"end beat number for measure {i} is {(i * secondsPerMeasure)  + secondsPerMeasure}");
        }
    }

    private void UpdateOxygenBar()
    {
        var fillAmount = _currentOxygen / _maxOxygenAmount;
        _oxygenBar.value = fillAmount;
        _oxygenBarForeground.color = _oxygenGradient.Evaluate(fillAmount);
    }

    private void Oscillate()
    {   
        // var updatedScaleFactor = Mathf.Abs((float)Mathf.Sin(omega * Time.time) * _movementAmplitude) * Time.deltaTime;
        /* var updatedScaleFactor = (_triangle.transform.position.x > _endingTransform.position.x) ? - secondsPerMeasure : secondsPerMeasure;

        _triangle.transform.position = new Vector3(_triangle.transform.position.x + updatedScaleFactor * Time.fixedDeltaTime,
            _startingTransform.position.y,
            _startingTransform.position.z);*/
        _timePassed += Time.deltaTime;
        _triangle.transform.position = Vector3.Lerp(_startingTransform.position, _endingTransform.position, Mathf.PingPong(_timePassed, 1));

        //_timePassed += Time.deltaTime;
        // _triangle.transform.position = Vector3.Lerp(_startingTransform.position, _endingTransform.position, updatedScaleFactor);
    }

    [SerializeField] private Transform _playerRingTransfrom;
    [SerializeField] private Transform _entryTransform;
    [SerializeField] private Transform _exitTransform;

    private float _period = 3.0f; // how long to hold on the button;

    [SerializeField] private float _currentPoint;
    [SerializeField] private float _entryPointMin, _entryPointMax;
    [SerializeField] private float _exitPointMin, _exitPointMax;
    private float _playerVelocity;
    private Vector3 _initialPoint;

    [SerializeField]
    private bool _isHoldingSpace, _canAddToFill, _hitFillBonus = false;

    [SerializeField] private float _fillAmount, _totalFill = 0.0f;
    private float _fillRate = 5.0f;

    private float _fillMax = 100.0f;

    private readonly float EntryScale = 3f;
    private readonly float StartingScale = 1f;
    private readonly float ExitScale = 6f;


    void Start()
    {
        // Set up the intial positions of circles
        _playerRingTransfrom.localScale = StartingScale * Vector3.one;
        _entryTransform.localScale = EntryScale * Vector3.one;
        _exitTransform.localScale = ExitScale * Vector3.one;

        // Set up values for checking
        _currentPoint = StartingScale;
        
        _entryPointMin = EntryScale;
        _entryPointMax = EntryScale + 0.08f;

        _exitPointMin = ExitScale;
        _exitPointMax = ExitScale + _rangeThreshold;

        _playerVelocity = (_exitPointMax - StartingScale) / (_period);
    }

    void Update()
    {
        var scaleDelta = _playerVelocity * Time.deltaTime;
        
        var currentPoint = _currentPoint;
        currentPoint += scaleDelta;

        if (currentPoint >= _exitPointMax && currentPoint >= _exitPointMax)
        {
            currentPoint = StartingScale;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isHoldingSpace = true;
            if (currentPoint > _entryPointMin)
            {
                _canAddToFill = true;
                if (currentPoint >= _entryPointMin && currentPoint <= _entryPointMax)
                {
                    _fillAmount = 10.0f;
                    _hitFillBonus = true;
                }
                else
                {
                    _fillAmount = 0.0f;
                }
            }
            else
            {
                _canAddToFill = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isHoldingSpace = false;
            if (currentPoint > _entryPointMin)
            {
                // Debug.Log("Release point: " + currentPoint);

                // Check if the player is within the exit range
                // calculate distance from exit min, if the distance
                // is greater than halfway between min/max diminish fill
                // amount

                // Within exit range
                // only check the x's since everything changes uniformly
                if (currentPoint >= _exitPointMin && currentPoint <= _exitPointMax)
                {
                    var distanceFromMax = _exitPointMax - currentPoint;
                    Debug.Log("Distance from max: " + distanceFromMax);
                    if (distanceFromMax >= (_rangeThreshold * 0.5f))
                    {
                        Debug.Log("Halfway or greater away from the exit ring max");
                        _fillAmount = 0.0f;
                    }
                }

                if (_canAddToFill)
                {
                    _totalFill += (_fillAmount * Time.deltaTime);
                    // Debug.Log("Fill amount: " + _totalFill);
                    
                    var fillAmount = _totalFill / _fillMax;
                    _oxygenBar.value = fillAmount;
                    _oxygenBarForeground.color = _oxygenGradient.Evaluate(fillAmount);
                }
            }

            currentPoint = StartingScale;
            _canAddToFill = false;
            _hitFillBonus = false;
        }

        if (_isHoldingSpace)
        {
            _fillAmount += _fillRate;
        }
        
        _currentPoint = currentPoint;
        _playerRingTransfrom.localScale = _currentPoint * Vector3.one;
    }

}

 /* _currentPoint = _playerRingTransfrom.localScale;
        _initialPoint = _currentPoint;

        _entryPointMin = _entryTransform.localScale;
        Debug.Log("Entry mag: " + _entryPointMin / Vector3.one);
        _entryPointMax = _entryTransform.localScale + (0.08f * Vector3.one);

        _exitPointMin = _exitTransform.localScale;

        // _playerVelocity = (_exitPointMax - _initialPoint).magnitude / (2 * _period);
        _playerVelocity = 5.0f / (2 * _period);
        Debug.Log("Player Velocity: " + _playerVelocity);

        // Add buffer range for exit
        _exitPointMax = _exitTransform.localScale + (_rangeThreshold * Vector3.one);*/



        /* var scaleDelta = _playerVelocity * Time.deltaTime;
        _currentPoint = _playerRingTransfrom.localScale;
        
        _currentPoint += scaleDelta * Vector3.one;
        
        if (_currentPoint.x >= _exitPointMax.x && _currentPoint.y >= _exitPointMax.y)
        {
            _currentPoint = _initialPoint;
        }

         if (Input.GetKeyDown(KeyCode.Space))
        {
            _isHoldingSpace = true;

            if (_currentPoint.x >= _entryPointMin.x && _currentPoint.x <= _entryPointMax.x)
            {
                _fillAmount = 10.0f;
            }
            else
            {
                _fillAmount = 0.0f;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isHoldingSpace = false;

            // Check if the player is within the exit range
            // calculate distance from exit min, if the distance
            // is greater than halfway between min/max diminish fill
            // amount

            // Within exit range
            // only check the x's since everything changes uniformly
            if (_currentPoint.x >= _exitPointMin.x && _currentPoint.x <= _exitPointMax.x)
            {
                
                var distanceFromMin = _exitPointMin - _currentPoint;
                if (distanceFromMin.magnitude >= (_rangeThreshold * 0.5f))
                {
                    Debug.Log("Halfway or greater away from the exit ring max");
                }
            }

           
            _totalFill += _fillAmount;
        }
        
        */
