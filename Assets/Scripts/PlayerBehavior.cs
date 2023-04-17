using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehavior : MonoBehaviour
{
    float underWaterDrag = 3f;
    float underwaterAngularDrag = 1f;
    float airDrag = 0f;
    float airAngularDrag = 0.05f;
    float waterHeight = 0f;
    Rigidbody2D _rb;
    bool underwater;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rotatePlayer();

    }

    void rotatePlayer()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    225);

            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                -45);
            }
            else
            {
                transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                -90);
            }

        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                135);

            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                45);
            }
            else
            {
                transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                90);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.eulerAngles = new Vector3(
                    transform.eulerAngles.x,
                    transform.eulerAngles.y,
                    180);
            }
        }
    }
}
