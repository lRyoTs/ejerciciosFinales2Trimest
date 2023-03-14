using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public bool isGameOver = false;
    public int points = 0;
    public bool hasBeenClick = false;
    private int lives = 3;

    //COMPONENTS
    private Material _material;
    
    public AudioSource _audio;
    public AudioClip hitSound;

    public TextMeshProUGUI livesText; 


    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        SetText($"LIVES: {lives}");
        points = 0;
        hasBeenClick = false;
        StartCoroutine(GenerateNextRandomPos());
        _material = GetComponent<Renderer>().material;
        _audio = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {    
        if (!hasBeenClick) {
            _material.color = Color.green; //Chnage color to green
            points++; //Increase points
            _audio.PlayOneShot(hitSound, 1);
            hasBeenClick = true;
        }
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Function that returns a Random Position
    private Vector3 GenerateRandomPos() {
        Vector3 pos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        return pos;
    }

    //Coroutine that changes game object position every 2 seconds
    private IEnumerator GenerateNextRandomPos() {

        while (!isGameOver) {
            yield return new WaitForSeconds(2);
            
            //Check lifes
            if (!hasBeenClick) {
                //lives--;
                if (--lives <= 0)
                {
                    SetText($"LIVES: {lives}");
                    isGameOver = true;
                    break; //Cut execution
                }
                else {
                    SetText($"LIVES: {lives}");
                }
            }

            //Reset
            transform.position = GenerateRandomPos();
            _material.color = Color.blue;
            hasBeenClick = false;
        }
    }

    public void SetText(string text) {
        livesText.text = text;
    } 
}
