using System.Collections.Generic;
using UnityEngine;

public class MyCol : MonoBehaviour
{
    public MyCollider col;
    List<string> colactivelayers = new List<string>{"a","a"};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = new MyCollider(this.gameObject,this.transform.localScale,new Vector2(0,0),colactivelayers);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Debug.Log(col.getpos()); 
    }
    void FixedUpdate()
    {
        col.Aligntoparent();
    }
}
