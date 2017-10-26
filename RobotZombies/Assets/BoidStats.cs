using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidStats : MonoBehaviour {


    private bool displayStats = false;
    Vector3 screenPosition;
    AudioSource audioSource;
    [SerializeField]AudioClip zombieDying;
    public Image Fill;
    public Texture sizeTexture;

    private Slider zombieStatsSlider;

    public Color maxHealth = Color.green;
    public Color minHealth = Color.red;
    public float maxValue = 10.0f;

    [Range(0.0f, 10.0f)]
    public float size;          //fitness
    [Range(0.0f, 10.0f)]
    public float wealth;        //Hats
    [Range(0.0f, 10.0f)]
    public float heatlh;        //desease
    //[Range(0.0f, 10.0f)]
    public Color color;

    
    [SerializeField]
    private MeshRenderer coloredRegion;
    public GameObject boidPrefab;
    public Transform hatPlace;

    public bool squished = false;

    void OnMouseOver()
    {
        displayStats = true;
    }
    void OnMouseExit()
    {
        displayStats = false;
        zombieStatsSlider.gameObject.SetActive(false);
    }

    void OnGUI()
    {

        GUI.contentColor = Color.black;
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition.y = Screen.height - screenPosition.y;

        //zombieStatsSlider

        
        if (displayStats == true && squished == false)
        {
            //GUI.Label(new Rect(screenPosition.x, screenPosition.y + 10, 50, 50), "Size");
            // size = GUI.HorizontalSlider(new Rect(screenPosition.x, screenPosition.y + 25, 50, 50), size, 0.0f, 10.0f);


            zombieStatsSlider.gameObject.SetActive(true);
            zombieStatsSlider.value = size;
            Fill.color = Color.Lerp(minHealth, maxHealth, (float)size / maxValue);



           // GUI.Label(new Rect(screenPosition.x, screenPosition.y + 35, 50, 50), "Wealth");
           // wealth = GUI.HorizontalSlider(new Rect(screenPosition.x, screenPosition.y + 50, 50, 50), wealth, 0.0f, 10.0f);

           // GUI.Label(new Rect(screenPosition.x, screenPosition.y + 60, 50, 50), "Health");
            //heatlh = GUI.HorizontalSlider(new Rect(screenPosition.x, screenPosition.y + 75, 50, 50), heatlh, 0.0f, 10.0f);

            //GUI.Box(new Rect(screenPosition.x, screenPosition.y + 25, 110, 50), "Size: " + size + "\nWealth: " + wealth + "\nHealth: " + heatlh);
        }
    }

    public void generateStats()
    {
        size = Random.Range(0.0f, 10.0f);
        wealth = Random.Range(0.0f, 10.0f);
        heatlh = Random.Range(0.0f, 10.0f);
        color = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        coloredRegion.material.color = color;
    }

    public void ApplyStatsVisuals()
    {
        coloredRegion.material.color = color;
        float mappedScale = Utils.Map(size, 0.0f, 10.0f, 0.5f, 1.5f);
        transform.localScale = new Vector3(mappedScale, mappedScale, mappedScale);
        hatPlace.localPosition = new Vector3(0.0f, .5f, 0.0f);
    }

    public void hatSelect()
    {
        string hatLocation = "Hats/";
        if (wealth < 2.0f)
            hatLocation += "Beanie";
        else if(wealth >= 2.0f && wealth < 4.0f)
            hatLocation += "Cap";
        else if (wealth >= 4.0f && wealth < 6.0f)
            hatLocation += "None";
        else if (wealth >= 6.0f && wealth < 8.0f)
            hatLocation += "Bowler";
        else
            hatLocation += "TopHat";

        GameObject hat = Resources.Load(hatLocation) as GameObject;
        GameObject hatInst = Instantiate(hat, hatPlace.position, hatPlace.rotation, transform);
    }

    // Use this for initialization
    void Start() {
        generateStats();
        ApplyStatsVisuals();
        hatSelect();
        audioSource = GetComponent<AudioSource>();
        zombieStatsSlider = gameObject.GetComponent<Slider>();

        zombieStatsSlider.minValue = 0.0f;
        zombieStatsSlider.maxValue = 10.0f;

    }

    // Update is called once per frame
    void Update() {
  
    }

    public static GameObject breed(BoidStats boid1, BoidStats boid2, GameObject boidPrefab)
    {
        Vector3 spawnLoc = (boid1.transform.position + boid2.transform.position) / 2;
        spawnLoc.y = RobotZombieBehaviour.Instance.spawnHeight;
        GameObject newBorn = Instantiate(boidPrefab, spawnLoc, Quaternion.identity);

        var nbStats = newBorn.GetComponent<BoidStats>();
        nbStats.size = Mathf.Max(0.0f, Mathf.Min(10.0f, (boid1.size + boid2.size / 2) + Random.Range(-0.1f, 0.1f)));
        nbStats.wealth = Mathf.Max(0.0f, Mathf.Min(10.0f, (boid1.wealth + boid2.wealth / 2) + Random.Range(-0.1f, 0.1f)));
        nbStats.heatlh = Mathf.Max(0.0f, Mathf.Min(10.0f, (boid1.heatlh + boid2.heatlh / 2) + Random.Range(-0.1f, 0.1f)));
        nbStats.color.r = Mathf.Max(0.0f, Mathf.Min(1.0f, (boid1.color.r + boid2.color.r / 2) + Random.Range(-0.01f, 0.01f)));
        nbStats.color.g = Mathf.Max(0.0f, Mathf.Min(1.0f, (boid1.color.g + boid2.color.g / 2) + Random.Range(-0.01f, 0.01f)));
        nbStats.color.b = Mathf.Max(0.0f, Mathf.Min(1.0f, (boid1.color.b + boid2.color.b / 2) + Random.Range(-0.01f, 0.01f)));
        //nbStats.StartCoroutine(nbStats.babyTime(nbStats, nbStats.size));
        return newBorn;
    }

    IEnumerator babyTime(BoidStats newBorn, float size)
    {
        newBorn.size /= 2;
        newBorn.ApplyStatsVisuals();
        yield return new WaitForSeconds(3.0f);
        newBorn.size = size;
        newBorn.ApplyStatsVisuals();
        yield return null;
    }

    public IEnumerator squash(GameObject obj)
    {
        //audioSource.plyaf
        obj.transform.localScale = new Vector3(obj.transform.localScale.x, 0.01f, obj.transform.localScale.z);
        obj.transform.position = new Vector3(obj.transform.position.x, 0.01f, obj.transform.position.z);
        obj.transform.localRotation = Quaternion.identity;
        obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        squished = true;

        yield return new WaitForSeconds(3.0f);
        //obj.SetActive(false);
        yield return null;
    }
}
