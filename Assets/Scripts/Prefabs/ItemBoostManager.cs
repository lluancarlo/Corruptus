using UnityEngine;

public class ItemBoostManager : MonoBehaviour
{
    // Public variables
    public bool boostSpeed = false;
    public bool boostScore = false;
    public float multiplier = 0;
    public AudioClip pickUpAudio;
    // Private variables
    new string name;
    GameObject sceneManager;
    float limitToDestroy;
    
    // Public function
    public void DestroyItem(){
        Destroy(this.gameObject);
    }

    // Private function
    void Start()
    {
        this.boostScore = !this.boostSpeed;
        this.name = this.gameObject.transform.name;
        this.limitToDestroy = (Camera.main.orthographicSize*2f / 2) * -1;
        this.sceneManager = GameObject.FindGameObjectWithTag("Manager");
    }

    void Update() 
    {
        if(this.gameObject.transform.position.y < this.limitToDestroy)
            Destroy(this.gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player"){
           
            if (this.pickUpAudio!=null)
                sceneManager.SendMessage("PlaySound", this.pickUpAudio);

            if(this.boostScore)
                sceneManager.SendMessage("MultiplierScore", Mathf.RoundToInt(this.multiplier));
            else
                sceneManager.SendMessage("MultiplierSpeed", this.multiplier);

            this.DestroyItem(); 
        }
    }
}
