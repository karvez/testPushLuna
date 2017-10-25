using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieSounds : MonoBehaviour {

    public AudioClip zombieSquish;
    AudioSource audioSource;
   
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(zombieSquish, 0.8F);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
