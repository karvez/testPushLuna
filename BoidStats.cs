﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidStats : MonoBehaviour {

    private bool displayStats = false;
    Vector3 screenPosition;
       
    public Slider wealthSlider;

    Texture2D zomSizeImg;
    Texture2D zomWealthImg;
    Texture2D zomHealthImg;
    Texture2D zomSizeIcon;
    Gradient healthGradient;
    GradientColorKey[] gck;
    GradientAlphaKey[] gak;

    [Range(0.0f, 10.0f)]
    public float size;          //fitness
    [Range(0.0f, 10.0f)]
    public float wealth;        //Hats
    [Range(0.0f, 10.0f)]
    public float heatlh;        //desease
    //[Range(0.0f, 10.0f)]
    public Color color;

    public bool hasBred = false; 

    [SerializeField]
    private MeshRenderer coloredRegion;
    public GameObject boidPrefab;
    public Transform hatPlace;
	public GameObject hatInst;

    public GameObject lovedOne;
    public GameObject breedingParticles;
    public GameObject fireParts;

    public Material fryMat1;
    public Material fryMat2;

    public GameObject charParticles;

    public bool squished = false;

    void OnMouseOver()
    {
        displayStats = true;
    }
    void OnMouseExit()
    {
        displayStats = false;
    }

    void Start()
    {
        
        
        healthGradient = new Gradient();
        gck = new GradientColorKey[2];

        gck[0].color = Color.red;
        gck[0].time = 0.0F;
        gck[1].color = Color.green;
        gck[1].time = 1.0F;

        gak = new GradientAlphaKey[2];
        gak[0].alpha = 1.0F;
        gak[0].time = 0.0F;
        gak[1].alpha = 1.0F;
        gak[1].time = 1.0F;
        healthGradient.SetKeys(gck, gak);

    }

    void OnGUI()
    {
       
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition.y = Screen.height - screenPosition.y;
 
        
        if (displayStats == true)
        {
            
            GUI.skin.label.fontSize = 11;

            GUI.Label(new Rect(screenPosition.x, screenPosition.y + 5, 50, 20), "Health: ");
            GUI.color = healthGradient.Evaluate(heatlh / 10);
            GUI.DrawTexture(new Rect(screenPosition.x, screenPosition.y + 20, heatlh*10, 10), zomHealthImg);
           
            //GUI.Slider(new Rect(screenPosition.x, screenPosition.y + 25, 110, 20),size,10,0.0f,size,wealthSlider)
            GUI.color = Color.white;
            GUI.Label(new Rect(screenPosition.x, screenPosition.y + 27, 50, 20), "Wealth: ");
            GUI.DrawTexture(new Rect(screenPosition.x, screenPosition.y + 42, wealth * 10, 10), zomWealthImg);

            //GUI.Label(new Rect(screenPosition.x, screenPosition.y +49, 50, 20), "Size: ");
            GUI.DrawTexture(new Rect(screenPosition.x, screenPosition.y + 53, 100, 15), zomSizeIcon);
            GUI.DrawTexture(new Rect(screenPosition.x, screenPosition.y + 68, size * 10, 10), zomSizeImg);

        }
    }

    public void loadZomStats()
    {
        zomSizeIcon = Resources.Load("size icon") as Texture2D;
        zomSizeImg = Resources.Load("Circle") as Texture2D;
        zomHealthImg = Resources.Load("zombieHealthSprite") as Texture2D;
        zomWealthImg = Resources.Load("money background") as Texture2D;

    }

    public void generateStats()
    {
        size = Random.Range(0.0f, 10.0f);
        wealth = Random.Range(0.0f, 10.0f);
        heatlh = Random.Range(0.0f, 10.0f);
		//color = new Color (Utils.Map (heatlh, 0.0f, 10.0f, 0.0f, 0.42f), 0.42f, Utils.Map (heatlh, 0.0f, 10.0f, 0.0f, 0.42f));
        color = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        coloredRegion.material.color = color;
    }

    public void ApplyStatsVisuals()
    {
        coloredRegion.material.color = color;
        float mappedScale = Utils.Map(size, 0.0f, 10.0f, 0.5f, 1.5f);
        transform.localScale = new Vector3(mappedScale, mappedScale, mappedScale);
        hatPlace.localPosition = new Vector3(0.0f, .5f, 0.0f);
		//color = new Color (Utils.Map (heatlh, 0.0f, 10.0f, 0.0f, 0.42f), 0.42f, Utils.Map (heatlh, 0.0f, 10.0f, 0.0f, 0.42f));
    }

    public void hatSelect()
    {
        string hatLocation = "Hats/";
        if (wealth < 2.0f)
            hatLocation += "None";
        else if(wealth >= 2.0f && wealth < 4.0f)
            hatLocation += "Beanie";
        else if (wealth >= 4.0f && wealth < 6.0f)
            hatLocation += "Cap";
        else if (wealth >= 6.0f && wealth < 8.0f)
            hatLocation += "Bowler";
        else
            hatLocation += "TopHat";

        GameObject hat = Resources.Load(hatLocation) as GameObject;
        hatInst = Instantiate(hat, hatPlace.position, hatPlace.rotation, transform);
    }

	public void removeHat()
	{
		Destroy (hatInst);
	}

    // Use this for initialization
    void Awake() {
        generateStats();
        ApplyStatsVisuals();
        hatSelect();
        loadZomStats();

    }

    // Update is called once per frame
    void Update() {

    }

    public GameObject findClosestZombot()
    {
        GameObject closestZombot = null;
        float dist = float.MaxValue;
        foreach (var zombot in RobotZombieBehaviour.Instance.robotZombies)
        {
            BoidStats zombotBS = zombot.GetComponent<BoidStats>();
            if (zombotBS != this && zombotBS.lovedOne == null)
            {
                float currDist = Vector3.Distance(transform.position, zombot.transform.position);
                if (currDist < dist)
                {
                    dist = currDist;
                    closestZombot = zombot;
                }
            }
        }
        return closestZombot;
    }

    public static GameObject breed(BoidStats boid1, BoidStats boid2, GameObject boidPrefab)
    {
        Vector3 spawnLoc = (boid1.transform.position + boid2.transform.position) / 2;
        spawnLoc.y = RobotZombieBehaviour.Instance.spawnHeight;
        GameObject newBorn = Instantiate(boidPrefab, spawnLoc, Quaternion.identity);
        GameObject parts = Instantiate(boid1.breedingParticles, spawnLoc, boid1.breedingParticles.transform.rotation);

        var nbStats = newBorn.GetComponent<BoidStats>();
		nbStats.size = Mathf.Max(0.0f, Mathf.Min(10.0f, ((boid1.size + boid2.size) / 2) + Utils.NextGaussian()));
		nbStats.wealth = Mathf.Max(0.0f, Mathf.Min(10.0f, ((boid1.wealth + boid2.wealth) / 2) + Utils.NextGaussian()));
		nbStats.heatlh = Mathf.Max(0.0f, Mathf.Min(10.0f, ((boid1.heatlh + boid2.heatlh) / 2) + Utils.NextGaussian()));
		nbStats.color.r = Mathf.Max(0.0f, Mathf.Min(1.0f, ((boid1.color.r + boid2.color.r) / 2) + Utils.NextGaussian()/10));
		nbStats.color.g = Mathf.Max(0.0f, Mathf.Min(1.0f, ((boid1.color.g + boid2.color.g) / 2) + Utils.NextGaussian()/10));
		nbStats.color.b = Mathf.Max(0.0f, Mathf.Min(1.0f, ((boid1.color.b + boid2.color.b) / 2) + Utils.NextGaussian()/10));
        nbStats.StartCoroutine(nbStats.babyTime(nbStats, nbStats.size));
        nbStats.removeHat();
        nbStats.ApplyStatsVisuals();
        nbStats.hatSelect();
        return newBorn;
    }

    IEnumerator babyTime(BoidStats newBorn, float size)
    {
        Timer timer = new Timer(1.0f);
        while (!timer.Trigger())
        {
            newBorn.size =  size / Utils.Map(timer.timeLeft, 0.0f, 1.0f, 1.0f, 5.0f);
            newBorn.ApplyStatsVisuals();
            yield return new WaitForSeconds(0.1f);
        }
        newBorn.size = size;
        newBorn.ApplyStatsVisuals();
        yield return null;
    }

    public IEnumerator squash(GameObject obj)
    {
        obj.transform.localScale = new Vector3(obj.transform.localScale.x, 0.01f, obj.transform.localScale.z);
        obj.transform.position = new Vector3(obj.transform.position.x, 0.01f, obj.transform.position.z);
        obj.transform.localRotation = Quaternion.identity;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        squished = true;

        yield return new WaitForSeconds(3.0f);
        //obj.SetActive(false);
        yield return null;
    }

    public IEnumerator fry(GameObject obj)
    {
        Material[] originalMats = obj.GetComponent<MeshRenderer>().materials;
        Material[] fryMats1 = new Material[originalMats.Length];
        Material[] fryMats2 = new Material[originalMats.Length];
        for (int i = 0; i < fryMats1.Length; i++)
        {
            fryMats1[i] = new Material(fryMat1);
        }
        for (int i = 0; i < fryMats2.Length; i++)
        {
            fryMats2[i] = new Material(fryMat2);
        }
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Timer time = new Timer(1.3f);
        while (!time.Trigger())
        {
            if (obj == null)
            {
                yield return null;
                break;
            }
            obj.transform.position += new Vector3((Random.value - 0.5f) / 5, 0.0f, (Random.value - 0.5f) / 5);
            int val = Random.Range(0, 3);
            if (val == 0)
            {
                obj.GetComponent<MeshRenderer>().materials = fryMats1;
            }
            else if (val == 1)
            {
                obj.GetComponent<MeshRenderer>().materials = fryMats2;
            }
            else
            {
                //obj.GetComponent<MeshRenderer>().materials = originalMats;
            }
            if (obj == null)
            {
                yield return null;
                break;
            }
            yield return new WaitForSeconds(0.01f);
            if (obj == null)
            {
                yield return null;
                break;
            }
        }
        if (obj == null)
        {
            yield return null;
        }
        obj.GetComponent<MeshRenderer>().materials = fryMats1;

        obj.GetComponent<MeshRenderer>().enabled = false;
        foreach(var rend in obj.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
        obj.GetComponent<Collider>().enabled = false;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        var part = Instantiate(charParticles, obj.transform.position, Quaternion.identity, obj.transform);
        part.transform.localScale = transform.localScale;
        yield return null;
    }

    public IEnumerator OnFire(GameObject obj)
    {
        squished = true;
        var fireInstance = Instantiate(fireParts, obj.transform);
        fireInstance.transform.localScale = obj.transform.localScale;

        Timer timer = new Timer(3.0f);
        while (!timer.Trigger())
        {
            GetComponent<Rigidbody>().velocity = new Vector3((Random.value - .5f )*20, 0.0f, (Random.value - .5f)* 20);
            yield return new WaitForSeconds(.5f);
        }


        Destroy(fireInstance);

        obj.GetComponent<MeshRenderer>().enabled = false;
        foreach (var rend in obj.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
        obj.GetComponent<Collider>().enabled = false;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        var part = Instantiate(charParticles, obj.transform.position, Quaternion.identity, obj.transform);
        part.transform.localScale = transform.localScale;
        yield return null;
    }
}
