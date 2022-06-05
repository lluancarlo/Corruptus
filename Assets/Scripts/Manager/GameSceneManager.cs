using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    // Public
    public GameObject dilminha;
    public GameObject lulinha;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public AudioClip gameOverSpeak;
    public AudioClip menuClick;
    public AudioClip oneHundredScoreHighlight;
    public AudioClip fiveHundredScoreHighlight;
    public AudioClip oneThousandScoreHighlight;
    public AudioClip oneThousandHalfScoreHighlight;
    public AudioClip twoThousandScoreHighlight;
    public AudioClip twoThousandHalfScoreHighlight;
    public AudioClip threeThousandScoreHighlight;
    // Private
    private int currentHealth;
    private int currentScore;
    private int boostScoreValue = 1;
    private GameObject player;
    private GameObject scoreUI;
    private GameObject healthUI;
    private GameObject boostSpeedBar;
    private GameObject boostScoreBar;
    private AudioSource audioSource;
    private AudioSource voiceSource;
    private AudioSource musicSource;
    private Image[] heartImgList;
    
    // Public functions
    public void buttonLeft(bool press) => this.player.SendMessage("buttonLeft", press);
    public void buttonRight(bool press) => this.player.SendMessage("buttonRight", press);

    public void AddHealth()
    {
        if(currentHealth <= 3){
            currentHealth++;
            this.UpdateHealthOnScreen();
        }
    }

    public void AddDamage()
    {
        if(currentHealth > 0){
            currentHealth--;
            this.UpdateHealthOnScreen();
        }

        if(currentHealth == 0)
            this.GameOver();
    }

    public void AddScore(int value)
    {
        if (value > 0){
            this.currentScore += value * this.boostScoreValue;
        }else{
            this.currentScore += value;
        }
        
        if(this.currentScore < 0)
            this.currentScore = 0;

        this.UpdateScore();
        this.HighlightScore();
    }

    public void MultiplierScore(int value)
    {
        CancelInvoke("UpdateBoostScoreUI");
        this.boostScoreValue = value;
        this.boostScoreBar.SetActive(true);
        this.boostScoreBar.GetComponent<Slider>().value = 10;
        InvokeRepeating("UpdateBoostScoreUI", 0f, 0.1f);
    }

    public void MultiplierSpeed(float value)
    {
        CancelInvoke("UpdateBoostSpeedUI");
        player.SendMessage("boostSpeed", value);
        this.boostSpeedBar.SetActive(true);
        this.boostSpeedBar.GetComponent<Slider>().value = 10;
        InvokeRepeating("UpdateBoostSpeedUI", 0f, 0.1f);
    }

    public void GameOver()
    {
        this.audioSource.Stop();
        this.musicSource.Stop();

        var listFallingItens = GameObject.FindGameObjectsWithTag("Item");
        foreach (var FallingItem in listFallingItens)
        {
            FallingItem.gameObject.SendMessage("DestroyItem");
        }
        
        this.boostScoreBar.SetActive(false);
        this.boostSpeedBar.SetActive(false);
        
        this.gameObject.GetComponent("SpawnItens").SendMessage("StopSpawnItens");
        this.player.SendMessage("playerCanMove", false);

        var gameOverGUI = GameObject.FindGameObjectWithTag("GameOverUI");
        gameOverGUI.GetComponentInChildren<Animator>().SetBool("showMenu", true);

        this.PlaySound(this.gameOverSpeak);

        var fob = gameOverGUI.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        fob.anchoredPosition = new Vector2(0,0);
    }

    public void NewGame()
    {
        this.musicSource.Play();
        
        // Set default game config
        this.currentScore = 0;
        this.currentHealth = 3;
        this.boostScoreValue = 1;
        this.player.SendMessage("boostSpeed", 1);
        this.player.SendMessage("playerCanMove", true);
        this.player.gameObject.transform.position = new Vector3(0, this.player.gameObject.transform.position.y, 0);

        // Update GUI
        this.UpdateScore();
        this.UpdateHealthOnScreen();
        
        this.gameObject.GetComponent("SpawnItens").SendMessage("StartSpawnItens");
    }

    public void ResetGame()
    {
        this.PlaySound(menuClick);
        
        var gameOverGUI = GameObject.FindGameObjectWithTag("GameOverUI");
        gameOverGUI.GetComponentInChildren<Animator>().SetBool("showMenu", false);

        var fob = gameOverGUI.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        fob.anchoredPosition = new Vector2(0,-1000);

        this.NewGame();
    }
    
    public void PlaySound(AudioClip audio)
    {
        this.audioSource.volume = 0.5f;
        this.audioSource.PlayOneShot(audio);
    }

    // Private functions
    void Start()
    {
        var menu = SceneManager.GetSceneByName("MenuScene");
        if(menu.isLoaded)
            SceneManager.UnloadSceneAsync("MenuScene");

        this.musicSource = GameObject.Find("InGameMusic").GetComponent<AudioSource>();
        this.audioSource = GetComponent<AudioSource>();
        this.scoreUI = GameObject.FindGameObjectWithTag("ScoreUI");
        this.healthUI = GameObject.FindGameObjectWithTag("HealthUI");
        this.boostScoreBar = GameObject.FindGameObjectWithTag("BoostScoreUI");
        this.boostScoreBar.SetActive(false);
        this.boostSpeedBar = GameObject.FindGameObjectWithTag("BoostSpeedUI");
        this.boostSpeedBar.SetActive(false);
        this.heartImgList = this.healthUI.transform.GetComponentsInChildren<Image>();

        this.SetAndConfigureCharacter();
        this.NewGame();
    }

    void UpdateBoostScoreUI()
    {
        var slider = this.boostScoreBar.GetComponent<Slider>();
        slider.value -= 0.1f;
        if(slider.value == 0)
        {
            CancelInvoke("UpdateBoostScoreUI");
            this.boostScoreBar.SetActive(false);
            this.boostScoreValue = 1;
        }
    }

    void UpdateBoostSpeedUI()
    {
        var slider = this.boostSpeedBar.GetComponent<Slider>();
        slider.value -= 0.1f;
        if(slider.value == 0)
        {
            CancelInvoke("UpdateBoostSpeedUI");
            this.boostSpeedBar.SetActive(false);
            this.player.SendMessage("boostSpeed", 1);
        }
    }

    void UpdateHealthOnScreen()
    {
        for (int i = 0; i < heartImgList.Length; i++)
        {
            if(i < this.currentHealth)
                heartImgList[i].sprite = fullHeart;
            else
                heartImgList[i].sprite = emptyHeart;
        }
    }

    void UpdateScore()
    {
        var scoreDisplay = this.scoreUI.GetComponent<TextMeshProUGUI>();
        scoreDisplay.text = this.currentScore.ToString();
    }

    void HighlightScore()
    {
        switch (this.currentScore)
        {
            case 100:
                this.PlaySound(this.oneHundredScoreHighlight);
                break;
            case 500:
                this.PlaySound(this.fiveHundredScoreHighlight);
                break;
            case 1000:
                this.PlaySound(this.oneThousandScoreHighlight);
                break;
            case 1500:
                this.PlaySound(this.oneThousandHalfScoreHighlight);
                break;
            case 2000:
                this.PlaySound(this.twoThousandScoreHighlight);
                break;
            case 2500:
                this.PlaySound(this.twoThousandHalfScoreHighlight);
                break;
            case 3000:
                this.PlaySound(this.threeThousandScoreHighlight);
                break;
            default:
                break;
        }
    }

    void SetAndConfigureCharacter()
    {
        GameObject playerObj = null;
        Vector3 spawn = Vector3.zero;

        if(ProjectScript.selectedCharacter == ProjectScript.CharacterEnum.Dilminha)
        {
            playerObj = this.dilminha;
            spawn = new Vector3(0, -3.8f, 0);
        }else{
            playerObj = this.lulinha;
            spawn = new Vector3(0, -3.8f, 0);
        }

        this.player = Instantiate(playerObj, spawn, Quaternion.identity);
        this.player.tag = "Player";
    }
}
