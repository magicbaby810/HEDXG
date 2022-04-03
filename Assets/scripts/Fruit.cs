using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitState
{
    Ready = 0,
    StandBy = 1,
    Dropping = 2,
    Collision = 3
}


public enum FruitType
{
    ONE = 0,
    TWO = 1,
    THREE = 2,
    FOUR = 3, 
    FIVE = 4,
    SIX = 5,
    SEVEN = 6,
    EIGHT = 7,
    NINE = 8,
    TEN = 9
}

public class Fruit : MonoBehaviour
{
    public FruitType fruitType = FruitType.ONE;
    public bool isMove = false;
    public FruitState fruitState = FruitState.Ready;
    public float LimitX = 2.0f;

    public Vector3 originScale = Vector3.zero;
    public float scaleSpeed = 0.1f;
    public float fruitScore = 1.0f;

    public float LimitRedHeight = 1.0f;

    public bool isCombining = false;


    private void Awake()
    {
        originScale = new Vector3(0.8f, 0.8f, 0.8f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("GameState：" + GameManager.gameManagerInstance.gameState + " FruitState：" + fruitState + " move:" + isMove + " down:" + Input.GetMouseButtonDown(0));
        if (GameManager.gameManagerInstance.gameState == GameState.StandBy && fruitState == FruitState.StandBy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("按下");
                isMove = true;
            }
            if (Input.GetMouseButtonUp(0) && isMove)
            {
                Debug.Log("松开");
                isMove = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                fruitState = FruitState.Dropping;
                GameManager.gameManagerInstance.gameState = GameState.InProgress;
                GameManager.gameManagerInstance.InvokeCreateFruit(1.5f);
            }
            if (isMove)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.gameObject.GetComponent<Transform>().position = new Vector3(mousePosition.x, this.gameObject.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);
            }
        }
        //Debug.Log("LimitLeftX is " + LimitX);
        if (this.transform.position.x > LimitX)
        {
            this.transform.position = new Vector3(LimitX, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.x < -LimitX)
        {
            this.transform.position = new Vector3(-LimitX, this.transform.position.y, this.transform.position.z);
        }

        if (this.transform.localScale.x < originScale.x)
        {
            Debug.Log("scale " + this.transform.localScale.x + " " + originScale.x);
            this.transform.localScale += (new Vector3(0.5f, 0.5f, 0.5f) * scaleSpeed);
        }
        else
        {
            this.transform.localScale = originScale;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (fruitState == FruitState.Dropping)
        {
            Debug.Log("444");
            if (collision.gameObject.tag.Contains("Floor"))
            {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                fruitState = FruitState.Collision;
                Debug.Log("5555");

                GameManager.gameManagerInstance.hitSource.Play();
            }

            if (collision.gameObject.tag.Contains("Fruit"))
            {
                GameManager.gameManagerInstance.gameState = GameState.StandBy;
                fruitState = FruitState.Collision;

                //RedLine.redLineInstance.OnTriggerEnter2D(collision.collider);
                Debug.Log(this.transform.position.y + "  LimitRedHeight is " + LimitRedHeight);
                if (this.transform.position.y > LimitRedHeight)
                {     
                    RedLine.redLineInstance.OnTriggerEnter2D(collision.collider);
                }

                Debug.Log("6666 " + this.transform.position.y + " " + RedLine.redLineInstance.transform.position.y);
                GameManager.gameManagerInstance.fruitSource.Play();
            }
        }

        checkCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

       checkCollision(collision);

    }

    public void checkCollision(Collision2D collision)
    {
        //Debug.Log("xxxxxxxx");
        if ((int)fruitState >= (int)FruitState.Dropping)
        {
            if (collision.gameObject.tag.Contains("Fruit"))
            {
                //Debug.Log("xxxxxxxx " + fruitType + " " + collision.gameObject.GetComponent<Fruit>().fruitType);
                if (fruitType == collision.gameObject.GetComponent<Fruit>().fruitType)
                {
                    if (fruitType != FruitType.TEN)
                    {
                        if (this.transform.localScale == originScale)
                        {
                            Debug.Log("xxxxxxxx 是否正在合成 = " + isCombining);
                            if (!isCombining)
                            {
                                float currentPosXY = this.transform.position.x + this.transform.position.y;
                                float collisionPosXY = collision.transform.position.x + collision.transform.position.y;
                                if (currentPosXY > collisionPosXY)
                                {
                                    isCombining = true;
                                    Debug.Log("xxxxxxxx 合成开始");

                                    GameManager.gameManagerInstance.CombineNewFruit(fruitType, this.transform.position, collision.transform);
                                    fruitScore = ((int)fruitType + 1) * 2;
                                    GameManager.gameManagerInstance.totalScore += fruitScore;
                                    GameManager.gameManagerInstance.totalScoreText.text = "SCORE：" + GameManager.gameManagerInstance.totalScore.ToString();
                                    Destroy(this.gameObject);
                                    Destroy(collision.gameObject);

                                    isCombining = false;
                                    Debug.Log("xxxxxxxx 合成结束");
                                }
                            }  
                        }
                    }
                    else
                    {
                        Destroy(this.gameObject);
                        Destroy(collision.gameObject);
                    }
                }
            }
        }
    }
}
