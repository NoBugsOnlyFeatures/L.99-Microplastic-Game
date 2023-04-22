using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinCollectionBehavior : MonoBehaviour
{
    [SerializeField] PlayerBehavior _player;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Urchin"){
            
            // Display E over the top of urchin
            // Check for down key
            if (Input.GetKey(KeyCode.E)){
                Debug.Log("Interacting with Urchin");
                Debug.Log("player in urchincollection event" + _player);
                _player.GetUrchin();
                Destroy(other.gameObject);
            }
        }
    }
}
