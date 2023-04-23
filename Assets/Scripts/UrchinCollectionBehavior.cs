using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UrchinCollectionBehavior : MonoBehaviour
{
    [SerializeField] PlayerBehavior _player;
    [SerializeField] AudioClip firstScrape;
    [SerializeField] AudioClip secondScrape;
    [SerializeField] AudioClip thirdScrape;
    int mod = 0;
    AudioSource audio;

    void Awake(){
        audio = GetComponent<AudioSource>(); 
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Urchin"){
            
            // Display E over the top of urchin
            // Check for down key
            if (Input.GetKey(KeyCode.E)){
                switch (mod % 3){
                    case 0:
                    audio.PlayOneShot(firstScrape);
                    break;
                    case 1:
                    audio.PlayOneShot(secondScrape);
                    break;
                    case 2:
                    audio.PlayOneShot(thirdScrape);
                    break;
                    default:
                    audio.PlayOneShot(firstScrape);
                    break;
                }
                _player.GetUrchin();
                Destroy(other.gameObject);
            }
        }
    }
}
