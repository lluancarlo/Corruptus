using UnityEngine;

public class SpawnItens : MonoBehaviour
{
    // Public variables
    public GameObject[] scoreItens;
    public GameObject[] boostItens;
    public GameObject healthItem;
    public AudioClip stage2Audio;
    public AudioClip stage3Audio;
    public AudioClip stage4Audio;
    //Private variables
    GameObject sceneManager;
    float screenHeight;
    float screenWidth;
    float timer;
    bool spawnItens;
    bool stage2;
    bool stage3;
    bool stage4;

    // Public functions
    public void StartSpawnItens()
    {
        this.spawnItens = true;
        InvokeRepeating("CreateScoreItem",0f,1.2f);
        InvokeRepeating("CreateHealthItem",60f,60f);
    }

    public void StopSpawnItens()
    {
        this.timer = 0f;
        this.spawnItens = false;
        CancelInvoke("CreateScoreItem");
        CancelInvoke("CreateHealthItem");
        CancelInvoke("createBoostItem");
    }

    // Private functions
    private void CreateScoreItem()
    {
        if(this.scoreItens != null)
        {
            int itemNum = Random.Range(0, this.scoreItens.Length);
            this.SpawnItem(this.scoreItens[itemNum]);
        }
    }

    private void createBoostItem()
    {
        if(this.boostItens != null)
        {
            int itemNum = Random.Range(0, this.boostItens.Length);
            this.SpawnItem(this.boostItens[itemNum]);
        }
    }

    private void CreateHealthItem()
    {
        if(this.healthItem != null)
            this.SpawnItem(this.healthItem);
    }

    private void SpawnItem(GameObject item)
    {
        var screenHeight = Camera.main.orthographicSize*2f;
        var screenWidth = screenHeight/Screen.height*Screen.width;
        float randomMin = screenWidth / 2 - screenWidth;
        float randomMax = screenHeight / 2;
        float randomPosition = Random.Range(randomMin, randomMax);
        
        GameObject newItem = Instantiate(item);
        newItem.transform.position = new Vector2(randomPosition, screenHeight);
        newItem.transform.SetParent(this.gameObject.transform);
    }

    private void Start() 
    {
        this.sceneManager = GameObject.FindGameObjectWithTag("Manager");
    }

    void Update()
    {
        timer += Time.deltaTime;
        float seconds = timer % 60;

        if(spawnItens)
        {
            if(seconds > 20 && !stage2){
                this.sceneManager.SendMessage("PlaySound", stage2Audio);
                CancelInvoke("CreateScoreItem");
                InvokeRepeating("CreateScoreItem",0f,0.8f);
                InvokeRepeating("createBoostItem",50f,50f);
                this.stage2 = true;
            }else if (seconds > 60 && !stage3){
                this.sceneManager.SendMessage("PlaySound", stage3Audio);
                CancelInvoke("CreateScoreItem");
                InvokeRepeating("CreateScoreItem",0f,0.5f);
                this.stage3 = true;
            }else if (seconds > 300 && !stage4){
                this.sceneManager.SendMessage("PlaySound", stage4Audio);
                CancelInvoke("CreateScoreItem");
                InvokeRepeating("CreateScoreItem",0f,0.2f);
                this.stage4 = true;
            }
        }
    }

    void OnDestroy() 
    {
        this.StopSpawnItens();
    }
}
