using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public bool GameStarted { get; set;}
    private Vector3 _initalPosition;

    void Awake()
    {
        _initalPosition = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStarted)
        {
            transform.position = player.transform.position + new Vector3(0,0,-10);
        }
    }
}
