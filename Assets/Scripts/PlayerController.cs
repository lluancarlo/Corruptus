using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables
    public AudioClip[] voices;

    // Private variables
    AudioSource mouth;
    new Rigidbody2D rigidbody;
    Animator anim;
    SpriteRenderer sprite;
    bool moveLeft;
    bool moveRight;
    bool canMove = true;
    bool canSpeak = false;
    float speed = 2000f;
    float speedBoost = 1f;
    Vector3 lastPosition;

    // Public functions
    public void buttonLeft(bool press) => this.moveLeft = press;
    public void buttonRight(bool press) => this.moveRight = press;
    public void boostSpeed(float value) => this.speedBoost = value;
    public void playerCanMove(bool value) => this.canMove = value;
    public void playerCanSpeak(bool value) => this.canSpeak = value;

    // Private function
    private bool CharMoved()
    {
        var displacement = this.rigidbody.transform.position - lastPosition;
        lastPosition = this.rigidbody.transform.position;
        return displacement.magnitude > 0.01;
    }

    private void speakSomething()
    {
        if (this.canSpeak){
            int voiceNum = Random.Range(0, this.voices.Length);
            this.mouth.PlayOneShot(this.voices[voiceNum]);
        }
    }

    private void Start() {
        this.mouth = this.gameObject.GetComponent<AudioSource>();
        this.rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
        this.anim = this.gameObject.GetComponent<Animator>();
        this.sprite = this.gameObject.GetComponent<SpriteRenderer>();

        float screenHeight = Camera.main.orthographicSize*2f;
        float ScreenWidth = screenHeight/Screen.height*Screen.width;

        InvokeRepeating("speakSomething",2f,30f);
    }

    private void Update() {

        if (canMove)
        {
            // Axis control
            var axis = Input.GetAxis("Horizontal");

            if (axis != 0){
                if (axis < 0){
                    this.rigidbody.AddRelativeForce(new Vector2(speed*speedBoost*Time.deltaTime*axis, 0));
                    this.sprite.flipX = true;
                }else if (axis > 0){
                    this.rigidbody.AddRelativeForce(new Vector2(speed*speedBoost*Time.deltaTime*axis, 0));
                    this.sprite.flipX = false;
                }
            }else{
                if (moveLeft){
                    this.rigidbody.AddRelativeForce(new Vector2(-speed*speedBoost*Time.deltaTime, 0));
                    this.sprite.flipX = true;
                }else if (moveRight){
                    this.rigidbody.AddRelativeForce(new Vector2(speed*speedBoost*Time.deltaTime, 0));
                    this.sprite.flipX = false;
                }
            }
        }
    }

    private void FixedUpdate() {
        if (Mathf.Abs(this.rigidbody.velocity.x) > 0.1){
            this.anim.SetBool("moving", true);
            this.anim.speed = Mathf.Abs(this.rigidbody.velocity.x);
        }else{
            this.anim.SetBool("moving", false);
            this.anim.speed = 1;
        }
    }
}
