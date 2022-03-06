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
            if (this.transform.position.y > limit_y)
            {
                this.transform.Translate(Vector3.right * speed);
            } else
            {
                isMove = false;
                Invoke("ReloadScene", 1.0f);
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
                    GameManager.gameManagerInstance.gameState = GameState.GameOver;
                    Invoke("MoveAndCalculateScoreState", 0.5f);
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

    public void MoveAndCalculateScoreState()
    {
        Debug.Log("333");
        isMove = true;
        GameManager.gameManagerInstance.gameState = GameState.CalculateScore;
    }

    public void ReloadScene()
    {
        float highestScore = PlayerPrefs.GetFloat("highestScore");
        if (highestScore < GameManager.gameManagerInstance.totalScore)
        {
            PlayerPrefs.SetFloat("highestScore", GameManager.gameManagerInstance.totalScore);
        }

        SceneManager.LoadScene("HCDXG2022");
    }
}
