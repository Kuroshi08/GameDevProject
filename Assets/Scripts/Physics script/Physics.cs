using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
/// <summary>
/// make into monobehavior
/// </summary>
public class MyPhysics : MonoBehaviour
{
    float mass;
    bool[] lockXY = new bool[2];
    bool grav;
    Vector2 vel;
    bool isGrounded;
    float Gravity = 9.8f;
    MyCollider vCollider;


    //make this also a comp, use My colliderdirectly
    void Awake()
    {
        vCollider = gameObject.AddComponent<MyCollider>();
    }
    public void MoveObject(Vector2 vel, string onlyAffectTag = null)
    {
        List<UnityEngine.Object> vcolliderr = new List<UnityEngine.Object>();
        ///////////////write point based( gen points based on positive x * y + vector speed, left right based on positive x)
        Vector2[] points = new Vector2[4];
        Vector2[] tempPoints = new Vector2[2];
        float c = vel.x / Math.Abs(vel.x) * vel.y / Math.Abs(vel.y);
        for(int i = 0; i < 2;i ++)
        {
            tempPoints[i] = new Vector2(transform.lossyScale.x/2 * (float)Math.Pow(-1,2) + transform.position.x, transform.lossyScale.y/2 * (float)Math.Pow(-1,2) * c + transform.position.y);
        }
        for(int i = 1; i < 3; i++)
        {
            float p = vel.x / Math.Abs(vel.x);
            Vector2 a = tempPoints[i-1] + vel;
            points[(int)(0.5 + p/2)] = tempPoints[i-1];
            points[(int)(0.5 - p/2)] = a;

        }
        vCollider.ChangePoints(points);
        vcolliderr = vCollider.getallcollisions();
        if(vcolliderr.Count != 0)
        {
            float minDistanceX = vel.x;
            
            foreach(MyCollider collider in vcolliderr)
            {
                if(collider.gameObject == gameObject)
                {
                    
                    break;
                }
                float colfacex = (collider.offset.x) * -vel.x/Math.Abs(vel.x) + collider.gameObject.transform.position.x; 
                float reldistanceX = colfacex - o.transform.position.x; 
                if(Math.Abs(reldistanceX) < Math.Abs(minDistanceX))
                {
                    minDistanceX = reldistanceX;
                }
            }
            vel.x = minDistanceX;
        }
        transform.Translate(vel);
    }
    bool IsGround(GameObject o)
    {
        BoxCollider2D colliderground = o.AddComponent<BoxCollider2D>();
        return false;
    }
}
