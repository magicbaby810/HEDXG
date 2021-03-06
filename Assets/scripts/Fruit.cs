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
    TEN = 9,
    ELEVEN = 10
}

public class Fruit : MonoBehaviour
{
    public FruitType fruitType = FruitType.ONE;
    public bool isMove = false;
    public FruitState fruitState = FruitState.Ready;
    public float LimitX = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManagerInstance.gameState == GameState.StandBy && fruitState == FruitState.StandBy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMove = true;
            }
            if (Input.GetMouseButtonUp(0) && isMove)
            {
                isMove = false;
                this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
                fruitState = FruitState.Dropping;
                GameManager.gameManagerInstance.gameState = GameState.InProgress;
                GameManager.gameManagerInstance.InvokeCreateFruit(1.0f);
            }
            if (isMove)
            {
                Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                this.gameObject.GetComponent<Transform>().position = new Vector3(mousePosition.x, this.gameObject.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);
            }
        }

        if (this.transform.position.x > LimitX)
        {
            this.transform.position = new Vector3(LimitX, this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.x < -LimitX)
        {
            this.transform.position = new Vector3(-LimitX, this.transform.position.y, this.transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("葡萄");
        if (collision.gameObject.tag.Contains("Floor"))
        {
            GameManager.gameManagerInstance.gameState = GameState.StandBy;
            fruitState = FruitState.Collision;
        }
        if (collision.gameObject.tag.Contains("Fruit"))
        {
            GameManager.gameManagerInstance.gameState = GameState.StandBy;
            fruitState = FruitState.Collision;
        }

        if ((int) fruitState >= (int) FruitState.Dropping)
        {
            if (collision.gameObject.tag.Contains("Fruit"))
            {
                if (fruitType == collision.gameObject.GetComponent<Fruit>().fruitType) 
                {
                    float currentPosXY = this.transform.position.x + this.transform.position.y;
                    float collisionPosXY = collision.transform.position.x + collision.transform.position.y;
                    if (currentPosXY > collisionPosXY)
                    {
                        GameManager.gameManagerInstance.CombineNewFruit(fruitType, this.transform.position, collision.transform.position);
                        Destroy(this.gameObject);
                        Destroy(collision.gameObject);
                    }
                }
            }
        }
    }
}
