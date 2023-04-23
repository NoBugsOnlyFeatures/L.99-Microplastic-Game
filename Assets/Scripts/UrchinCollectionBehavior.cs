using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinCollectionBehavior : MonoBehaviour
{
    [SerializeField] PlayerBehavior _player;
    [SerializeField] AudioClip[] _scrape;
    private int _audioRotation = 0;
    AudioSource _audio;

    void Awake(){
        _audio = GetComponent<AudioSource>(); 
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Urchin"){
            
            // Display E over the top of urchin
            // Check for down key
            if (Input.GetKey(KeyCode.E)){
                _audio.PlayOneShot(_scrape[_audioRotation%_scrape.Length]);
                _audioRotation += 1;
                _player.GetUrchin();
                Destroy(other.gameObject);
            }
        }
    }
}
