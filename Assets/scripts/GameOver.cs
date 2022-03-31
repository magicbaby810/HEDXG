using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{

    public Animator animator;
    public GameObject screenFader;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("GameOver " + RedLine.redLineInstance.gameOverCount + " " + RedLine.redLineInstance.maxCount);
        if (RedLine.redLineInstance.gameOverCount == RedLine.redLineInstance.maxCount)
        {        
            screenFader.SetActive(true);
            animator.SetTrigger("GameOver");
            
        }
    }


}
