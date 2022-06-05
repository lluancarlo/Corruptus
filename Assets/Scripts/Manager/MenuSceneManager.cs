using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneManager : MonoBehaviour
{
    // Public
    public GameObject mainMenuUI;
    public GameObject selectCharacterUI;
    public GameObject lulinhaSelected;
    public GameObject dilminhaSelected;
    public Button buttonCharacter;
    public GameObject LoadingUI;
    public GameObject loadingBar;
    public Text loadingText;
    public Button loadingButton;
    public AudioClip menuClick;
    public AudioClip lulinhaClick;
    public AudioClip dilminhaClick;

    // Private
    private AudioSource audioSource;
    private Scene InGameScene;
    private AsyncOperation asyncOperation;

    // Public functions
    public void ChangeToSelectCharacter()
    {
        this.audioSource.PlayOneShot(menuClick);
        this.mainMenuUI.SetActive(false);
        this.selectCharacterUI.SetActive(true);
        this.DisableButton(this.buttonCharacter);
    }

    public void ChangeToLoading()
    {
        this.audioSource.PlayOneShot(menuClick);
        this.selectCharacterUI.SetActive(false);
        this.LoadingUI.SetActive(true);
        this.DisableButton(this.loadingButton);

        StartCoroutine(this.LoadGameAsync());
    }

    public void ChangeToGame()
    {
        this.asyncOperation.allowSceneActivation = true;
    }

    public void SelectLulinha()
    {
        this.audioSource.PlayOneShot(lulinhaClick);
        ProjectScript.selectedCharacter = ProjectScript.CharacterEnum.Lulinha;
        this.lulinhaSelected.SetActive(true);
        this.dilminhaSelected.SetActive(false);
        
        if(!this.buttonCharacter.enabled)
            this.EnableButton(this.buttonCharacter);
    }
    public void SelectDilminha()
    {
        this.audioSource.PlayOneShot(dilminhaClick);
        ProjectScript.selectedCharacter = ProjectScript.CharacterEnum.Dilminha;
        this.lulinhaSelected.SetActive(false);
        this.dilminhaSelected.SetActive(true);

        if(!this.buttonCharacter.enabled)
            this.EnableButton(this.buttonCharacter);
    }

    // Private functions
    private void Start()
    {
        this.audioSource = GetComponent<AudioSource>();

        this.mainMenuUI.SetActive(true);
        this.selectCharacterUI.SetActive(false);
        this.LoadingUI.SetActive(false);

        this.lulinhaSelected.SetActive(false);
        this.dilminhaSelected.SetActive(false);
    }
    
    private IEnumerator LoadGameAsync()
    {
        this.asyncOperation = SceneManager.LoadSceneAsync("InGameScene", LoadSceneMode.Additive);
        this.asyncOperation.allowSceneActivation = false;

        while (!this.asyncOperation.isDone)
        {    
            float progress = Mathf.Clamp01(this.asyncOperation.progress/0.9f);
            this.loadingBar.GetComponent<Slider>().value = progress;
            this.loadingText.text = progress * 100f + "%";

            if (this.asyncOperation.progress >= 0.9f){
                this.loadingBar.SetActive(false);
                this.EnableButton(this.loadingButton);
            }

            yield return null;
        }
    }

    private void EnableButton(Button button)
    {
        button.enabled = true;
        var newColors = button.colors;
        newColors.normalColor = new Color(255,255,255,1);
        button.colors = newColors;
    }

    private void DisableButton(Button button)
    {
        button.enabled = false;
        var newColors = button.colors;
        newColors.normalColor = new Color(255,255,255,0.3f);
        button.colors = newColors;
    }
}
