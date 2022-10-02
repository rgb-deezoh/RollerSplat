using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public Button menu;
    public TextMeshProUGUI title;
    public Canvas canvas;

    public static GameManager singleInsatanceManager;
    private GroundPiece[] allGroundPieces;

    public AudioSource audioSource;
    public AudioClip levelComplete;

    public ParticleSystem winEffect;
    // Start is called before the first frame update
    void Start()
    {
        SetUpNewLevels();
       // audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (singleInsatanceManager == null)
        {
            singleInsatanceManager = this;
        }else if (singleInsatanceManager != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void SetUpNewLevels()
    {
        allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevels();
    }

    public void CheckComplete()
    {
        bool isFinished = true;

        for(int i = 0; i < allGroundPieces.Length; i++)
        {
            if (allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {
            //Load next level after two seconds
            StartCoroutine(Wait());
        }
    }

    public void NextLevel()
    {
        // keeps the background music playing
        DontDestroyOnLoad(audioSource);
        DontDestroyOnLoad(canvas);
               
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SceneManager.LoadScene(1);
            menu.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            menu.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
            title.gameObject.SetActive(false);

        }
        
    }

    // Exits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator Wait()
    {
        winEffect.Play();
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(levelComplete, 2.0f);
        yield return new WaitForSeconds(2);
        NextLevel();
    }

    // This method loads the main menu
    public void LoadMainMenu()
    {
        StartCoroutine(ScreenBlend());
    }
    // This method sets and loads the main menu after a second
    IEnumerator ScreenBlend()
    {
        SceneManager.LoadScene(0);
        menu.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        playButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
    }
}
