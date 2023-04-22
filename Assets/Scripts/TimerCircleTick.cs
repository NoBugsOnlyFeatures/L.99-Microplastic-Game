using UnityEngine;
using UnityEngine.UI;

public class TimerCircleTick : MonoBehaviour
{
    [SerializeField]
    private RectTransform _tick;

    [SerializeField]
    private Transform _rotationPivot;

    private float _currentAngle;

    
    [SerializeField]
    private Image _tickImage;

    [SerializeField]
    private float _tickSpeed = 5f;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    void Awake()
    {
        _tickImage = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = _tick.transform.position;
        _initialRotation = _tick.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        var angle = _tickSpeed * 360.0f * Time.deltaTime; // move x degrees a second
        _tick.transform.RotateAround(_rotationPivot.position, Vector3.back, angle);
        
        var tickAngle = 360 - _tick.transform.rotation.eulerAngles.z;
        _currentAngle = tickAngle;
    }

    public float GetTickAngle()
    {
        return _currentAngle;
    }

    public float GetSpeed()
    {
        return _tickSpeed;
    }

    public void Reset()
    {
        _tick.transform.position = _initialPosition;
        _tick.transform.rotation = _initialRotation;
    }
}
