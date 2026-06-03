using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// MERGE WITH MYCOLLIDER to make into a monobehavior
/// </summary>

public class MyColComp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MyCollider col;
    public List<string> colactivelayers = new List<string>{"a","a"};
    public Vector2 Size = new Vector2();


    public bool useparentscale = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public MyColComp(bool useparentscale = true)
    {
        this.useparentscale = useparentscale;
    }
    void Awake()
    {
        col = new MyCollider(this.gameObject,this.transform.lossyScale,new Vector2(0,0));
    }
    void Start()
    {
        
    }
    UnityEngine.Object[] allGOwithCol()
    {
        UnityEngine.Object[] a;
        a = FindObjectsByType(typeof(MyColComp),FindObjectsSortMode.None);
        return a;
    }
    List<UnityEngine.Object> getallcollisions(UnityEngine.Object[] ol)
    {
        List<UnityEngine.Object> a = new List<UnityEngine.Object>();
        foreach(UnityEngine.Object ob in ol)
        {
            MyColComp obcolcomp = ob.GetComponent<MyColComp>();
            MyCollider obcol = obcolcomp.col;
            bool boolcheck = this.col.ColliderIntersect(obcol);
            if (boolcheck && obcol != this.col)
            {
                a.Add(ob);
            }
        }
        return a;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(getallcollisions(allGOwithCol()).Count); 
    }
    void getobjectsize()
    {
        Size = transform.lossyScale;
    }
    void FixedUpdate()
    {
        if (useparentscale)
        {
            getobjectsize();
        }

        col.Aligntoparent(transform.lossyScale);
    }
}
