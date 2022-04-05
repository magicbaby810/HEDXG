using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
    public GameObject BigXG;


    public Text BigXGCount;


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
        totalScoreText.text = "SCORE   0";
        startBtn.SetActive(false);
    }

    public void InvokeCreateFruit(float invokeTime)
    {
        Invoke("CreateFruit", invokeTime);
    }

    public void CreateIndexFruit(int index)
    {
        if (fruitList[index] != null && fruitList.Length > index)
        {
            GameObject fruitObject = fruitList[index];
            var currentFruit = Instantiate(fruitObject, fruitBornPosition.transform.position, fruitObject.transform.rotation);
            currentFruit.GetComponent<Fruit>().fruitState = FruitState.StandBy;
            Debug.Log("随机生成水果状态：" + currentFruit.GetComponent<Fruit>().fruitState);
        }
    }

    public void CreateFruit()
    {
        int index = randomForSmall();
        Debug.Log("随机生成第" + index + "个水果");
        CreateIndexFruit(index);
    }

    public void CombineNewFruit(FruitType fruitType, Vector3 currentPos, Transform collisionPos)
    {
        Vector3 combineNewPos = (currentPos + collisionPos.position) / 2;
        int newFruitType = (int) fruitType + 1;
        GameObject newFruit = fruitList[newFruitType];
        var combineFruit = Instantiate(newFruit, combineNewPos, newFruit.transform.rotation);
        combineFruit.GetComponent<Fruit>().fruitState = FruitState.Collision;
        combineFruit.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        

        float combineRadius = combineFruit.GetComponent<CircleCollider2D>().radius * 3;
        float radius = collisionPos.gameObject.GetComponent<CircleCollider2D>().radius * 2;

        Debug.Log("合成水果状态：" + combineFruit.GetComponent<Fruit>().fruitState + " " + radius / combineRadius);

        //combineFruit.transform.localScale = new Vector3(radius / combineRadius, radius / combineRadius, radius / combineRadius);
        combineFruit.transform.localScale = Vector3.zero;

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

        int index = UnityEngine.Random.Range(0, 5);
       
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

    public void CreateBigXGAndCalculateCount()
    {

        BigXG.SetActive(true);
        CombineBigXG.instance.startCombine();
        Debug.Log("xxxxxxxx dad0" + BigXGCount.text);

        int count = Convert.ToInt32(BigXGCount.text);
        Debug.Log("xxxxxxxx dad1 " + count);
        count++;
        Debug.Log("xxxxxxxx dad2 " + count);
        BigXGCount.text = Convert.ToString(count);
      
        

        Debug.Log("合成大西瓜 " + BigXGCount.text);
    }
}
