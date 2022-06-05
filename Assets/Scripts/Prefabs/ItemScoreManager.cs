using UnityEngine;

public class ItemScoreManager : MonoBehaviour
{
    // Public variables
    public int value = 0;
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
            sceneManager.SendMessage("AddScore", this.value);
            this.DestroyItem();
        }
    }
}
