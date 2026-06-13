using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class MyCollider : MonoBehaviour
{
    Mathstuff MyMathStuff = new Mathstuff();
    public List<string> tags = new List<string>();
    Vector2 Pos;
    Vector2 Size;
    Vector2 offset;
    Vector2[] points = new Vector2[4];
    float rotation;
    bool active = true;
    bool useparentscale = true;
    bool manualPoints = false;
    bool manualPos = false;
    List<float> activeLayers = new List<float>();
    void Awake()
    {
        this.Pos = new Vector2(transform.position.x,transform.position.y);
        this.Size = this.transform.lossyScale;
        this.rotation = transform.rotation.eulerAngles.z;
        if (!manualPoints)
        {
            points = calcpoints();
        }
        
    }
    Vector2[] calcpoints()
    {
        Vector2[] a = new Vector2[4];
        Vector2 totaloffset = Pos + offset;
        float sinA = (float)Math.Sin(rotation);
        float cosA = (float)Math.Cos(rotation);
        for(int i = 0; i < 4; i++)
        {
            Vector2 b = Size;
            if(i % 2 == 0)
            {
                b.x =b.x * -1 /2;
            }
            else
            {
                b.x = b.x/ 2;
            }
            if (i >= 2)
            {
                b.y =b.y * -1 /2;
            }
            else
            {
                b.y = b.y /2;
            }
            b = new Vector2(b.x*cosA + b.y*sinA,b.y * cosA - b.x * sinA);
            a[i] = b + totaloffset;
        }
        return a;
    }
    public Vector2[][] GetWalls()
    {
        //up,down,left,right
        Vector2[] w1 = new Vector2[2] {points[0],points[1]};
        Vector2[] w2 = new Vector2[2] {points[2],points[3]};
        Vector2[] w3 = new Vector2[2] {points[1],points[3]};
        Vector2[] w4 = new Vector2[2] {points[0],points[2]};
        return new Vector2[][] {w1,w2,w3,w4};
    }
    public void switchState(bool state)
    {
        active = state;
    }
    public bool getState()
    {
        return active;
    }



    public List<Vector2[]> ColliderIntersect(MyCollider a)
    {   
        List<Vector2[]> sad = new List<Vector2[]>();
        foreach(Vector2[] wall1 in this.GetWalls())
        {
            foreach(Vector2[] wall2 in a.GetWalls())
                if (MyMathStuff.doIntersect(wall1, wall2))
                {
                    sad.Add(wall2);
                }
        }
        if(sad.Count == 0)
        {
            if (ColliderInSelf(a)){
                foreach(Vector2[] wall in a.GetWalls())
                {
                    sad.Add(wall);
                }
                return sad;
            }
            return null;
        }
        return sad;
    }

    public List<Vector2[]> ColliderIntersectLine(Vector2[] line)
    {
        /// returns walls that intersects line
        List<Vector2[]> sad = new List<Vector2[]>();
        foreach(Vector2[] walls in this.GetWalls())
        {
            if (MyMathStuff.doIntersect(walls, line))
            {
                sad.Add(walls);
            }
        }
        if(sad.Count == 0)
        {
            return null;
        }
        return sad;
    }
    bool ColliderInSelf(MyCollider col)
    {   
        if(col.Pos.x < Pos.x+Size.x && col.Pos.x > Pos.x-Size.x && col.Pos.y < Pos.y+Size.y && col.Pos.y > Pos.y - Size.y)
        {
            return true;
        }
        return false;
    }

    
    void Aligntoparent(Vector2 Size)
    {
        Pos = new Vector2(transform.position.x,transform.position.y);
        this.Size = Size;
        if (!manualPoints)
        {
            points = calcpoints();
        }
        
    }
    public void AddTags(string t)
    {
        foreach(string tag in tags)
        {
            if(tag == t)
            {
                return;
            }
        }
        tags.Add(t);
    }
    public List<string> GetTags()
    {
        return tags;
    }
    public void ChangePoints(Vector2[] newPoints)
    {
        this.points = newPoints;
        manualPoints = true;
    }
    public void AutoPoints()
    {
        manualPoints = false;
    }
    public void ChangeOffset(Vector2 offset )
    {
        this.offset = offset;
    }
    public void ChangeScale(Vector2 scale)
    {
        this.Size = scale;
        useparentscale = false;
    }
    public void AutoScale()
    {
        useparentscale = true;
    }
    public Vector2 getpos()
    {
        return Pos;
    }
    public Vector2 getscale()
    {
        return Size;
    }

    public List<MyCollider> getallcollisions()
    {
        if (useparentscale)
        {
            Size = transform.lossyScale;
        }

        Aligntoparent(Size);
        List<MyCollider> a = new List<MyCollider>();
        foreach(UnityEngine.Object ob in FindObjectsByType(typeof(MyCollider),FindObjectsSortMode.None))
        {
            MyCollider obcol = ob.GetComponent<MyCollider>();
            bool boolcheck = (this.ColliderIntersect(obcol)!=null);
            if (boolcheck && obcol.gameObject != gameObject && obcol.getState())
            {
                a.Add(obcol);
            }
        }
        return a;
    }
    void Update()
    {
        if (useparentscale)
        {
            Size = transform.lossyScale;
        }

        Aligntoparent(Size);
    }
    void FixedUpdate()
    {
        if (useparentscale)
        {
            Size = transform.lossyScale;
        }

        Aligntoparent(Size);
    }
}

