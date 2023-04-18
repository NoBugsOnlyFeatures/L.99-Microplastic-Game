using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] float underWaterDrag = 2f;
    [SerializeField] float underwaterAngularDrag = 1f;
    [SerializeField] float swimForce = 300f;
    Rigidbody2D _rb;
    Vector2 swimDirection;
    [SerializeField] public uint NumberOfUrchins {get; set;}

    public UnityEvent<PlayerBehavior> OnGetUrchin;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.drag = underWaterDrag;
        _rb.angularDrag = underwaterAngularDrag;

        NumberOfUrchins = 0;
    }

    void Update()
    {
        keyDirection();
    }

    void FixedUpdate()
    {
        _rb.AddForce(swimDirection * swimForce * Time.deltaTime,ForceMode2D.Force);
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
        } else if (Input.GetKey(KeyCode.A)){
            swimDirection += Vector2.left;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Urchin" && other.gameObject.activeSelf){
            GetUrchin();
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "Boat"){
            DepositUrchin();
        }
    }

    public void GetUrchin(){
        NumberOfUrchins += 1;
        OnGetUrchin.Invoke(this);
    }

    public void DepositUrchin(){
        // Set urchins count in boat
        NumberOfUrchins = 0;
    }

}
