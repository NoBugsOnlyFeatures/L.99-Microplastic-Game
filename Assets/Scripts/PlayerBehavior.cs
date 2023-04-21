using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

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
    [SerializeField] public uint NumberOfUrchinsOnPlayer {get; set;}
    public UnityEvent<PlayerBehavior> OnGetUrchin;

    private Animator _animator;
    private bool _isDiving = false;

    public bool IsDiving
    {
        get => _isDiving;
        set => _isDiving = value;
    }


    void Awake() 
    {
        _animator = GetComponent<Animator>();    
    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();

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
            }
        } else if (Input.GetKey(KeyCode.A)){
            swimDirection += Vector2.left;
            if(!_sr.flipX){
                _sr.flipX = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Urchin" && other.gameObject.activeSelf){
            GetUrchin();
            Destroy(other.gameObject);
        } 
    }

    public void GetUrchin(){
        NumberOfUrchinsOnPlayer += 1;
        // Debug.Log("Player Urchins: " + NumberOfUrchinsOnPlayer);
        OnGetUrchin.Invoke(this);
    }
}
