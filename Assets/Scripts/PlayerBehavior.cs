using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] float underWaterDrag = 2f;
    [SerializeField] float underwaterAngularDrag = 1f;
    [SerializeField] float swimForce = 150f;
    Rigidbody2D _rb;
    Vector2 swimDirection;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.drag = underWaterDrag;
        _rb.angularDrag = underwaterAngularDrag;
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
}
