﻿using System.Collections;
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
    public float LimitLeftX = 2.0f;

    public Vector3 originScale = Vector3.zero;
    public float scaleSpeed = 0.01f;
    public float fruitScore = 1.0f;

    private void Awake()
    {
        originScale = new Vector3(0.5f, 0.5f, 0.5f);
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

        if (this.transform.position.x > (-LimitLeftX - 3))
        {
            this.transform.position = new Vector3((-LimitLeftX - 3), this.transform.position.y, this.transform.position.z);
        }
        if (this.transform.position.x < (LimitLeftX + 3))
        {
            this.transform.position = new Vector3((LimitLeftX + 3), this.transform.position.y, this.transform.position.z);
        }

        if (this.transform.localScale.x < originScale.x)
        {
            Debug.Log("scale " + this.transform.localScale.x + " " + originScale.x);
            this.transform.localScale += (new Vector3(1, 1, 1) * scaleSpeed);
        } else
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

                Debug.Log("6666 " + this.transform.position.y + " " + RedLine.redLineInstance.transform.position.y);
                GameManager.gameManagerInstance.fruitSource.Play();
            }
        }

        if (collision.gameObject.tag.Contains("RedLine"))
        {
            Debug.Log("7777");

        }


        if ((int) fruitState >= (int) FruitState.Dropping)
        {
            if (collision.gameObject.tag.Contains("Fruit"))
            {
                if (fruitType == collision.gameObject.GetComponent<Fruit>().fruitType && fruitType != FruitType.ELEVEN) 
                {
                    if (this.transform.localScale == originScale)
                    {
                        float currentPosXY = this.transform.position.x + this.transform.position.y;
                        float collisionPosXY = collision.transform.position.x + collision.transform.position.y;
                        if (currentPosXY > collisionPosXY)
                        {
                            GameManager.gameManagerInstance.CombineNewFruit(fruitType, this.transform.position, collision.transform.position);
                            GameManager.gameManagerInstance.totalScore += fruitScore;
                            GameManager.gameManagerInstance.totalScoreText.text = GameManager.gameManagerInstance.totalScore.ToString() + "分";
                            Destroy(this.gameObject);
                            Destroy(collision.gameObject);
                        }
                    } 
                }
            }
        }
    }
}
