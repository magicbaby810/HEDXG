using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineBigXG : MonoBehaviour
{
    public Animator animator;


    public static CombineBigXG instance;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void startCombine()
    {
        Debug.Log("xxxxxxxx dad3");
        animator.SetTrigger("CombineBigXG");
        Debug.Log("xxxxxxxx dad2");

    }
}
