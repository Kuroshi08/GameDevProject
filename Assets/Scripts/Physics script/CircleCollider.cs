using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

// this is kinematic so no physics sim
public class CircleCollider : MonoBehaviour , IColliders
{
    public Vector2 size {get;set;}
    float r;
    public Vector2 pos;
    public Vector2 offset;
    public bool active = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        r = size.x;
    }

    // Update is called once per frame
    void Update()
    {
        pos = transform.position;
    }
    
    bool ColliderIntersect(MyCollider a)
    {
        Vector2 relativeCenter = this.pos - a.Pos;

        Vector2 offsetFromCorner = new Vector2(Math.Abs(relativeCenter.x),Math.Abs(relativeCenter.y)) - a.size/2;

        float v = Math.Min(Math.Max(offsetFromCorner.x, offsetFromCorner.y), 0)
            + (Math.Max(offsetFromCorner.magnitude,0))
            - r;
        if(v > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    } 
public List<MyCollider> getallcollisions(List<string> exc, bool Only)
    {
        List<UnityEngine.Object> a = new List<UnityEngine.Object>();
        List<MyCollider> b = new List<MyCollider>();
        bool exists = false;
        MyColliderGroup g = MyColliderGroups.HasGroup(exc,Only);
        if(g != null)
        {
            a = g.colliders;
            exists = true;
        }
        if (!exists)
        {
            foreach(UnityEngine.Object ob in FindObjectsByType(typeof(MyCollider),FindObjectsSortMode.None))
            {
                bool exclud = Only;
                MyCollider obcol = ob.GetComponent<MyCollider>();
                foreach(string s in exc)
                {
                    if (obcol.tags.Contains(s))
                    {
                        exclud = !Only;
                    }
                }
                if (exclud)
                {
                    continue;
                }
                a.Add(obcol);
            }
            MyColliderGroups.groups.Add(new MyColliderGroup(exc,Only,a));
        }

        foreach(UnityEngine.Object ob in a)
        {
            if(ob == null)
            {
                a.Remove(ob);
                g.colliders = a;
                MyColliderGroups.UpdateGroup(g.tags,g.Only,g.colliders);
                continue;
            }
            MyCollider obcol = ob.GetComponent<MyCollider>();
            bool boolcheck = this.ColliderIntersect(obcol);
            if (boolcheck && obcol.gameObject != gameObject && obcol.getState())
            {
                b.Add(obcol);
            }
        }
        return b;
    }
}
