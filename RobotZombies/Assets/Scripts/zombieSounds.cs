using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieSounds : MonoBehaviour {

    public AudioClip zombieSquish;
    AudioSource audioSource;
   
	// Use this for initialization
	void Start () {
		
    }
	
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            audioSource.PlayOneShot(zombieSquish);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
