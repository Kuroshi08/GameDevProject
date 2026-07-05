using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class MyCollider : MonoBehaviour
{
    Mathstuff MyMathStuff = new Mathstuff();
    List<string> Tags = new List<string>();
    List<string> tags
    {
        get => Tags;
        set
        {
            foreach(string tag in value)
            {
                if (!Tags.Contains(tag))
                {
                    Tags.Add(tag);
                }
            }
            
        }
    }
    public Vector2 Pos {get; private set;}
    Vector2 Size;
    public Vector2 size
    {
        get => Size;
        set
        {
            Size = value;
            useparentscale = false;
        }
    }
    Vector2 Offset;
   public Vector2 offset
    {
        get => Offset;
        set
        {
            Offset = value;       
        }
    }
    Vector2[] points = new Vector2[4];
    float rotation;
    bool active = true;
    bool useparentscale = true;
    bool manualPoints = false;
    
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
        Vector2 totalOffset = Pos + Offset;
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
            a[i] = b + totalOffset;
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



    public bool ColliderIntersect(MyCollider a)
    {   
        Aligntoparent();
        if (ColliderInSelf(a))
        {
            return true;
        }
        if (a.ColliderInSelf(this))
        {
            return true;
        }
        foreach(Vector2[] wall1 in this.GetWalls())
        {
            foreach(Vector2[] wall2 in a.GetWalls())
                if (MyMathStuff.doIntersect(wall1, wall2))
                {
                    return true;
                }
        }
        return false;
    }

    public List<Vector2[]> ColliderIntersectLine(Vector2[] line)
    {
        /// returns walls that intersects line
        Aligntoparent();
        List<Vector2[]> sad = new List<Vector2[]>();
        foreach(Vector2[] walls in this.GetWalls())
        {
            if (MyMathStuff.GetIntersection(walls, line) != null)
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
        Aligntoparent();
        if(col.getcalpos().x< Pos.x+(Size.x/2) && col.getcalpos().x > Pos.x-(Size.x/2) && col.getcalpos().y < Pos.y+(Size.y/2) && col.getcalpos().y > Pos.y -(Size.y/2))
        {
            return true;
        }
        return false;
    }

    bool PointInSelf(Vector2 point)
    {
        Aligntoparent();
        if((point.x <= Pos.x + Offset.x + (Size.x / 2)) && (point.x >= Pos.x + Offset.x - (Size.x / 2)) && (point.y <= Pos.y + Offset.y + (Size.y / 2)) && (point.y >= Pos.y + Offset.y - (Size.y / 2)))
        {
            return true;
        }
        return false;
    }
    void Aligntoparent()
    {

        Pos = new Vector2(transform.position.x,transform.position.y);
        ChangeScale();
        if (!manualPoints)
        {
            points = calcpoints();
        }
        
    }
    void ChangeScale()
    {
        if (useparentscale)
        {
            this.Size = transform.lossyScale;
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
    public void ChangePoints(Vector2[] newPoints)
    {
        this.points = newPoints;
        manualPoints = true;
    }
    public void AutoPoints()
    {
        manualPoints = false;
    }
    public void AutoScale()
    {
        useparentscale = true;
    }
    public Vector2 getcalpos()
    {
        return Pos + Offset;
    }


    public List<MyCollider> getallcollisions()
    {
        Aligntoparent();
        List<MyCollider> a = new List<MyCollider>();
        foreach(UnityEngine.Object ob in FindObjectsByType(typeof(MyCollider),FindObjectsSortMode.None))
        {
            MyCollider obcol = ob.GetComponent<MyCollider>();
            bool boolcheck = this.ColliderIntersect(obcol);
            if (boolcheck && obcol.gameObject != gameObject && obcol.getState())
            {
                a.Add(obcol);
            }
        }
        return a;
    }
    void Update()
    {


        Aligntoparent();

    }
    void FixedUpdate()
    {

        Aligntoparent();
    }
}