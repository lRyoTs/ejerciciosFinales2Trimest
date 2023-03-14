using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //VARIABLES 
    //MIN MAX COOR
    private float maxX = 7f;
    private float minX = -7f;
    
    private float maxY = 3f;
    private float minY = -3f;
    
    private float maxZ = 4f;
    private float minZ = -4f;

    //GAME
    private bool isGameOver = false;
    private int points = 0;
    private bool hasBeenClick = false;
    private int lives = 3;
    private const int MAXLIVES = 5;
    private float timer = 2f;

    //COMPONENTS
    private Material _material;
    
    private AudioSource _audio;
    public AudioClip hitSound;
    public AudioClip missSound;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;

    public GameObject gameOverPanel;


    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        SetText(livesText, $"Lives: {lives}");
        points = 0;
        SetText(scoreText, $"Score\n{points}");
        hasBeenClick = false;
        timer = 2f;

        StartCoroutine(GenerateNextRandomPos());
        _material = GetComponent<Renderer>().material;
        _audio = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {    
        if (!hasBeenClick && !isGameOver) {
            
            if (lives < MAXLIVES){
                lives++;
                SetText(livesText, $"Lives: {lives}");
            }
            _material.color = Color.green; //Change color to green
            points++; //Increase points
            SetText(scoreText, $"Score\n{points}");
            _audio.PlayOneShot(hitSound, 1f);
            hasBeenClick = true;
        }
       
    }

    //Function that returns a Random Position
    private Vector3 GenerateRandomPos() {
        Vector3 pos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        return pos;
    }

    //Coroutine that changes game object position every 2 seconds
    private IEnumerator GenerateNextRandomPos() {

        while (!isGameOver) {
            yield return new WaitForSeconds(timer);
            //Check lifes
            if (!hasBeenClick) {
                lives--;
                SetText(livesText, $"Lives: {lives}");
                _audio.PlayOneShot(missSound, 1f); //Play miss sound
                if (lives <= 0)
                {
                    GameOver();
                    break; //Cut execution
                }
            }

            //Reset
            transform.position = GenerateRandomPos();
            _material.color = Color.blue;
            hasBeenClick = false;
            timer = Random.Range(1f, 2.5f); //Get a random wait time
        }
    }

    //Function that given a text field display the text desired
    private void SetText(TextMeshProUGUI field,string text) {
        field.text = text;
    }

    //Function that manages Game Over
    public void GameOver() {
        _material.color = Color.gray; //Set game over color
        _audio.Stop();
        gameOverPanel.SetActive(true);
        isGameOver = true;
    }

    //Function that Restart game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Load same scene
    }
}
