using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballCollision : MonoBehaviour {

    // Use this for initialization
    
    // Attach this to players golf ball

    [SerializeField] AudioClip ballHit;
    [SerializeField] AudioSource source;

    // the hitpower below was just for testing
    // get actual value from players hit of ball
    private float hitPower = 1.0f;


    void Awake() {

        source = GetComponent<AudioSource>();

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // if power above 50% play hard sound, else play soft sound
        // or  convert hitPower to a normalized value, between
        // 0.5f and 1.5f for pitch

        // hard hit
        if (hitPower >= 1f)
        {
        source.pitch = 0.5f;
        source.PlayOneShot(ballHit, 1f);
        }
        //soft hit
        else if (hitPower < 1f)
        {
        source.pitch = 1.5f;
            source.PlayOneShot(ballHit, 1f);
        }
        else
        {
            // ball to ball collision hit

            source.pitch = 1f;
            source.PlayOneShot(ballHit, 1f);
        }

    }

	// Update is called once per frame
	void Update () {
		
	}
}
