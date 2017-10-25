using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieHoverStats : MonoBehaviour
{

    // Use this for initialization

    // Attach to zombies

    public static bool queriesHitTriggers;

    //GameObject zombie;
    Vector3 zombieCurrentPos;
    Vector3 startingPos;
    Vector3 screenPosition;
    bool displayStats = false;

    private float zombieHealth = 3;
    private string zombieStringHealth = "3";

    zombieHoverStats zomb;
    Rigidbody2D rb;

    // Add to zo

    void Start()
    {
        startingPos.x = 1;
        startingPos.y = 1;
        startingPos.z = 1;
        zomb = GetComponent<zombieHoverStats>();
        rb = GetComponent<Rigidbody2D>();
        //zombie = GetComponent<GameObject>();
        Physics.queriesHitTriggers = true;
        //zombieCurrentPos = this.transform.position;
        transform.position = startingPos;
    }
    void OnMouseOver()
    {

        displayStats = true;
    }
    
    void OnMouseExit()
    {
        displayStats = false;
    }


    void Update()
    {
        
        //zombieCurrentPos = rb.transform.position;
        zombieStats();

    }

    void OnGUI()
    {

        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition.y = Screen.height - screenPosition.y;

        if (displayStats == true)
         {
        //zombieCurrentPos = transform.position;
        GUI.Box(new Rect(screenPosition.x-10, screenPosition.y-40, 100, 100), zombieStringHealth);
        }
        
    }
    public void zombieStats()
    {
        zombieHealth = 3;
        string zombieStringHealth = zombieHealth.ToString("3");
    }


    // Update is called once per frame
   
}
