using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

[System.Serializable]
public class OnGetUrchinEvent : UnityEvent<int>
{
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] float underWaterDrag = 2f;
    [SerializeField] float underwaterAngularDrag = 1f;
    [SerializeField] float swimForce = 300f;
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    bool isRightFacing; 
    Vector2 swimDirection;
    public uint NumberOfUrchinsOnPlayer {get; set;}
    public UnityEvent<PlayerBehavior> OnGetUrchin;

    GameObject _urchinCollectionAura;
    

    private Animator _animator;
    private bool _isDiving = false;

    private GameObject _airBubbleObject;
    [SerializeField] private Vector3 _airBubblePosition;

    public bool IsDiving
    {
        get => _isDiving;
        set => _isDiving = value;
    }


    void Awake() 
    {
        _animator = GetComponent<Animator>();    
        _airBubbleObject = GameObject.Find("AirBubble");
        _airBubblePosition = _airBubbleObject.transform.localPosition;
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();

        _urchinCollectionAura = GameObject.FindGameObjectWithTag("UrchinCollectionAura");

        _rb.drag = underWaterDrag;
        _rb.angularDrag = underwaterAngularDrag;

        NumberOfUrchinsOnPlayer = 0;
    }

    void Update()
    {
        if (IsDiving)
        {
            keyDirection();
        }
    }

    void FixedUpdate()
    {
        if (IsDiving)
        {
            _rb.AddForce(swimDirection * swimForce * Time.deltaTime,ForceMode2D.Force);
        }
    }

    void keyDirection()
    {
        swimDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.S))
        {
            swimDirection += Vector2.down;
        } else if (Input.GetKey(KeyCode.W)){
            swimDirection += Vector2.up;
        } 
 
        if (Input.GetKey(KeyCode.D)){
            swimDirection += Vector2.right;
            if(_sr.flipX){
                _sr.flipX = false;
                _airBubbleObject.transform.localPosition = new Vector3(-_airBubblePosition.x ,
                _airBubblePosition.y,
                _airBubblePosition.z);
                SwapCollectionAuraPosition();
            }
        } else if (Input.GetKey(KeyCode.A)){
            swimDirection += Vector2.left;
            if(!_sr.flipX){
                _sr.flipX = true;
                _airBubbleObject.transform.localPosition = new Vector3(_airBubblePosition.x ,
                _airBubblePosition.y,
                _airBubblePosition.z);
                SwapCollectionAuraPosition();
            }
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            GetUrchin();
        }
    }

    public void GetUrchin(){
        NumberOfUrchinsOnPlayer += 1;
        Debug.Log("OnGetUrchin event"+OnGetUrchin);
        OnGetUrchin.Invoke(this);
    }

    private void SwapCollectionAuraPosition(){
        var pos = _urchinCollectionAura.transform.localPosition;
        _urchinCollectionAura.transform.localPosition = new Vector3(-pos.x,pos.y,pos.z);
    }

    public void ResetUrchinCount()
    {
        NumberOfUrchinsOnPlayer = 0;
    }
}
