using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
public class MyCollider : MonoBehaviour
{
    Vector2 Pos;
    public Vector2 Size;
    Vector2 offset;
    public Vector2[] points = new Vector2[4];
    float rotation;
    bool active;
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



    public bool ColliderIntersect(MyCollider a)
    {
        foreach(Vector2[] wall1 in this.GetWalls())
        {
            foreach(Vector2[] wall2 in a.GetWalls())
                if (doIntersect(wall1, wall2))
                {
                    return true;
                }
        }
        return false;
    }
    bool onSegment(Vector2 p, Vector2 q, Vector2 r) {
        return (q[0] <= Math.Max(p[0], r[0]) && 
                q[0] >= Math.Min(p[0], r[0]) &&
                q[1] <= Math.Max(p[1], r[1]) && 
                q[1] >= Math.Min(p[1], r[1]));
    }
    int orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q[1] - p[1]) * (r[0] - q[0]) -
                  (q[0] - p[0]) * (r[1] - q[1]);

        // collinear
        if (val == 0) return 0;

        // clock or counterclock wise
        // 1 for clockwise, 2 for counterclockwise
        return (val > 0) ? 1 : 2;
    }
    bool doIntersect(Vector2[] line1, Vector2[] line2) {
        Vector2[][] points = new Vector2[][] {line1,line2};
        // find the four orientations needed
        // for general and special cases
        int o1 = orientation(points[0][0], points[0][1], points[1][0]);
        int o2 = orientation(points[0][0], points[0][1], points[1][1]);
        int o3 = orientation(points[1][0], points[1][1], points[0][0]);
        int o4 = orientation(points[1][0], points[1][1], points[0][1]);

        // general case
        if (o1 != o2 && o3 != o4)
            return true;

        // special cases
        // p1, q1 and p2 are collinear and p2 lies on segment p1q1
        if (o1 == 0 &&
        onSegment(points[0][0], points[1][0], points[0][1])) return true;

        // p1, q1 and q2 are collinear and q2 lies on segment p1q1
        if (o2 == 0 &&
        onSegment(points[0][0], points[1][1], points[0][1])) return true;

        // p2, q2 and p1 are collinear and p1 lies on segment p2q2
        if (o3 == 0 &&
        onSegment(points[1][0], points[0][0], points[1][1])) return true;

        // p2, q2 and q1 are collinear and q1 lies on segment p2q2 
        if (o4 == 0 &&
        onSegment(points[1][0], points[0][1], points[1][1])) return true;

        return false;
    }
    
    public void Aligntoparent(Vector2 Size)
    {
        Pos = new Vector2(transform.position.x,transform.position.y);
        this.Size = Size;
        if (!manualPoints)
        {
            points = calcpoints();
        }
        
    }
    public void ChangePoints(Vector2[] newPoints)
    {
        this.points = newPoints;
        manualPoints = true;
    }
    public Vector2 getpos()
    {
        return Pos;
    }
    public Vector2 getscale()
    {
        return Size;
    }

    UnityEngine.Object[] allGOwithCol()
    {
        UnityEngine.Object[] a;
        a = FindObjectsByType(typeof(MyCollider),FindObjectsSortMode.None);
        return a;
    }
    List<UnityEngine.Object> getallcollisions(UnityEngine.Object[] ol)
    {
        List<UnityEngine.Object> a = new List<UnityEngine.Object>();
        foreach(UnityEngine.Object ob in ol)
        {
            MyCollider obcol = ob.GetComponent<MyCollider>();
            bool boolcheck = this.ColliderIntersect(obcol);
            if (boolcheck && obcol != this)
            {
                a.Add(ob);
            }
        }
        return a;
    }
    void Update()
    {
        foreach(Vector2 point in points)
        {
            Debug.Log(point);
        }
        Debug.Log(getallcollisions(allGOwithCol()).Count); 
    }
    void FixedUpdate()
    {
        if (useparentscale)
        {
            Size = transform.lossyScale;
        }

        Aligntoparent(transform.lossyScale);
    }
}

