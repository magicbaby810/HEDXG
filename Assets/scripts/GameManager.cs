using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        startBtn.SetActive(false);
    }

    public void InvokeCreateFruit(float invokeTime)
    {
        Invoke("CreateFruit", invokeTime);
    }

    public void CreateFruit()
    {
        int index = Random.Range(0, 5);
        Debug.Log("随机生成第" + index + "个水果");
        if (fruitList[index] != null && fruitList.Length > index)
        {
            GameObject fruitObject = fruitList[index];
            var currentFruit = Instantiate(fruitObject, fruitBornPosition.transform.position, fruitObject.transform.rotation);         
            currentFruit.GetComponent<Fruit>().fruitState = FruitState.StandBy;
            Debug.Log("随机生成水果状态：" + currentFruit.GetComponent<Fruit>().fruitState);
        }
    }

    public void CombineNewFruit(FruitType fruitType, Vector3 currentPos, Vector3 collisionPos)
    {
        Vector3 combineNewPos = (collisionPos + collisionPos) / 2;
        int newFruitType = (int) fruitType + 1;
        GameObject newFruit = fruitList[newFruitType];
        var combineFruit = Instantiate(newFruit, combineNewPos, newFruit.transform.rotation);
        combineFruit.GetComponent<Fruit>().fruitState = FruitState.Collision;
        Debug.Log("合成水果状态：" + combineFruit.GetComponent<Fruit>().fruitState);
    }
}
