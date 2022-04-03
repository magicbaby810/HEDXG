using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState
{
    Ready = 0,
    StandBy = 1,
    InProgress = 2,
    GameOver = 3,
    CalculateScore = 4
}

public class GameManager : MonoBehaviour
{
    public GameObject[] fruitList;
    public GameObject fruitBornPosition;
    public GameObject startBtn;
    public static GameManager gameManagerInstance;

    public GameState gameState = GameState.Ready;

    public Vector3 combineScale = new Vector3(0, 0, 0);

    public float totalScore = 0f;
   
    public Text totalScoreText;

    public AudioSource combineSource;
    public AudioSource hitSource;
    public AudioSource fruitSource;

    public GameObject replayBtn;

    public bool isReturn = false;


    private void Awake()
    {
        gameManagerInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Debug.Log("开始游戏");
        CreateFruit();
        gameState = GameState.StandBy;
        totalScoreText.text = "SCORE：0";
        startBtn.SetActive(false);
    }

    public void InvokeCreateFruit(float invokeTime)
    {
        Invoke("CreateFruit", invokeTime);
    }

    public void CreateFruit()
    {
        int index = randomForSmall();
        Debug.Log("随机生成第" + index + "个水果");
        if (fruitList[index] != null && fruitList.Length > index)
        {
            GameObject fruitObject = fruitList[index];
            var currentFruit = Instantiate(fruitObject, fruitBornPosition.transform.position, fruitObject.transform.rotation);         
            currentFruit.GetComponent<Fruit>().fruitState = FruitState.StandBy;      
            Debug.Log("随机生成水果状态：" + currentFruit.GetComponent<Fruit>().fruitState);
        }
    }

    public void CombineNewFruit(FruitType fruitType, Vector3 currentPos, Transform collisionPos)
    {
        Vector3 combineNewPos = (currentPos + collisionPos.position) / 2;
        int newFruitType = (int) fruitType + 1;
        GameObject newFruit = fruitList[newFruitType];
        var combineFruit = Instantiate(newFruit, combineNewPos, newFruit.transform.rotation);
        combineFruit.GetComponent<Fruit>().fruitState = FruitState.Collision;
        combineFruit.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        
        Debug.Log("合成水果状态：" + combineFruit.GetComponent<Fruit>().fruitState);

        float combineRadius = combineFruit.GetComponent<CircleCollider2D>().radius * 2;
        float radius = collisionPos.gameObject.GetComponent<CircleCollider2D>().radius * 2;

        combineFruit.transform.localScale = new Vector3(radius / combineRadius, radius / combineRadius, radius / combineRadius);

        combineSource.Play();
    }

    public void Replay()
    {
        Invoke("ReloadScene", 1.0f);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("HCDXG");
    }

    public int randomForSmall()
    {

        int index = Random.Range(0, 5);
       
        if (index == 4 || index == 5)
        {
            if (isReturn)
            {
                index -= 2;
                isReturn = false;
            } else
            {
                isReturn = true;
            }
        }
        return index;
    }
}
