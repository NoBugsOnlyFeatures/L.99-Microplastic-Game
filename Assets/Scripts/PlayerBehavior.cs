using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehavior : MonoBehaviour
{
    float underWaterDrag = 3f;
    float underwaterAngularDrag = 1f;
    Vector2 swimDirection;
    float swimForce = 10f;
    float waterHeight = 0f;
    Rigidbody2D _rb;
    bool underwater;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // _rb.drag = underWaterDrag;
        // _rb.angularDrag = underwaterAngularDrag;
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
            Debug.Log("S");
            swimDirection += Vector2.down;
        } else if (Input.GetKey(KeyCode.W)){
            swimDirection += Vector2.up;
            Debug.Log("W");
        } 
 
        if (Input.GetKey(KeyCode.D)){
            swimDirection += Vector2.right;
            Debug.Log("D");
        } else if (Input.GetKey(KeyCode.A)){
            swimDirection += Vector2.left;
            Debug.Log("A");
        }
    }

    //     if (Input.GetKey(KeyCode.S))
    //     {
    //         if (Input.GetKey(KeyCode.A))
    //         {
    //             transform.eulerAngles = new Vector3(
    //                 transform.eulerAngles.x,
    //                 transform.eulerAngles.y,
    //                 225);

    //         }
    //         else if (Input.GetKey(KeyCode.D))
    //         {
    //             transform.eulerAngles = new Vector3(
    //             transform.eulerAngles.x,
    //             transform.eulerAngles.y,
    //             -45);
    //         }
    //         else
    //         {
    //             transform.eulerAngles = new Vector3(
    //             transform.eulerAngles.x,
    //             transform.eulerAngles.y,
    //             -90);
    //         }

    //     }
    //     else if (Input.GetKey(KeyCode.W))
    //     {
    //         if (Input.GetKey(KeyCode.A))
    //         {
    //             transform.eulerAngles = new Vector3(
    //             transform.eulerAngles.x,
    //             transform.eulerAngles.y,
    //             135);

    //         }
    //         else if (Input.GetKey(KeyCode.D))
    //         {
    //             transform.eulerAngles = new Vector3(
    //             transform.eulerAngles.x,
    //             transform.eulerAngles.y,
    //             45);
    //         }
    //         else
    //         {
    //             transform.eulerAngles = new Vector3(
    //             transform.eulerAngles.x,
    //             transform.eulerAngles.y,
    //             90);
    //         }
    //     }
    //     else
    //     {
    //         if (Input.GetKey(KeyCode.D))
    //         {
    //             transform.eulerAngles = new Vector3(
    //                 transform.eulerAngles.x,
    //                 transform.eulerAngles.y,
    //                 0);
    //         }
    //         else if (Input.GetKey(KeyCode.A))
    //         {
    //             transform.eulerAngles = new Vector3(
    //                 transform.eulerAngles.x,
    //                 transform.eulerAngles.y,
    //                 180);
    //         }
    //     }
    // }
}
