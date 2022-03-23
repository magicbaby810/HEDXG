using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedLine : MonoBehaviour
{
    public bool isMove = false;
    public float speed = 0.8f;
    public float limit_y = 0.5f;
    public static RedLine redLineInstance;
    public int count = 0;
    public int gameOverCount = 0;
    public bool isStay = false;
    public int maxCount = 5;

    private void Awake()
    {
        redLineInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            count++;
            if (count < 20)
            {
                redLineInstance.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
            }
            else if (count > 20)
            {
                redLineInstance.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (count == 40)
            {
                gameOverCount++;
                count = 0;
            }

            if (gameOverCount < maxCount && !isStay)
            {
                isMove = false;
                gameOverCount = 0;             
                count = 0;
                redLineInstance.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
           
            if (gameOverCount == maxCount)
            {
                Debug.Log("RedLine 10");
                Invoke("GameOverAndCalculateScoreState", 0.5f);
             
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("0000");
        if (collision.gameObject.tag.Contains("Fruit"))
        {
            Debug.Log("1111");
            if ((int) GameManager.gameManagerInstance.gameState < (int) GameState.GameOver)
            {
                Debug.Log("222 " + collision.gameObject.GetComponent<Fruit>().fruitState);
                if (collision.gameObject.GetComponent<Fruit>().fruitState == FruitState.Collision)
                {
                    //GameManager.gameManagerInstance.gameState = GameState.GameOver;
                    //Invoke("MoveAndCalculateScoreState", 0.5f);
                    isMove = true;
                    isStay = true;
                }
            }

            if (GameManager.gameManagerInstance.gameState == GameState.CalculateScore)
            {
                float currentScore = collision.GetComponent<Fruit>().fruitScore;
                GameManager.gameManagerInstance.totalScore += currentScore;
                GameManager.gameManagerInstance.totalScoreText.text = GameManager.gameManagerInstance.totalScore + "分";
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exitexit");
        isStay = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("zzzzzzz");
        isStay = true;
    }


    public void GameOverAndCalculateScoreState()
    {
        Debug.Log("333");
        isMove = false;
        GameManager.gameManagerInstance.gameState = GameState.GameOver;
        count = 0;
        gameOverCount = 0;
        Invoke("ReloadScene", 1.0f);
        //GameManager.gameManagerInstance.gameState = GameState.CalculateScore;
    }

    public void ReloadScene()
    {
        float highestScore = PlayerPrefs.GetFloat("highestScore");
        if (highestScore < GameManager.gameManagerInstance.totalScore)
        {
            PlayerPrefs.SetFloat("highestScore", GameManager.gameManagerInstance.totalScore);
        }

        SceneManager.LoadScene("HCDXG");
    }

}
