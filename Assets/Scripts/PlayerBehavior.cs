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
    [SerializeField]
    float floatingPower = 15f;
    float waterHeight = 0f;

    Rigidbody2D _rb;
    bool underwater;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float difference = transform.position.y - waterHeight;

        if (difference < 0){
            _rb.AddForceAtPosition(Vector2.up * floatingPower * Mathf.Abs(difference), transform.position, ForceMode2D.Force);

            if(!underwater){
                underwater = true;
                SwitchState(underwater);
            }
        } else if (underwater){
            underwater = false;
            SwitchState(underwater);
        }
    }

    void SwitchState(bool isUnderwater){
        if (isUnderwater){
            _rb.drag = underWaterDrag;
            _rb.angularDrag = underwaterAngularDrag;
        } else {
            _rb.drag = airDrag;
            _rb.angularDrag = airAngularDrag;
        }
    }
}
