using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using System.Linq;


/// <summary>
/// make into monobehavior
/// </summary>
public class MyPhysics : MonoBehaviour
{
    Mathstuff MyMathstuff = new Mathstuff();
    float mass;
    bool[] lockXY = new bool[2];
    bool grav;
    Vector2 vel;
    bool isGrounded;
    float Gravity = 9.8f;
    MyCollider vCollider;
    MyCollider eCollider;
    MyCollider selfcollider;


    //make this also a comp, use My colliderdirectly
    void Awake()
    {
        selfcollider = gameObject.GetComponent<MyCollider>();
        vCollider = gameObject.AddComponent<MyCollider>();
        vCollider.AddTags("MoveCol");
        vCollider.switchState(false);
        eCollider = gameObject.AddComponent<MyCollider>();
        eCollider.AddTags("MoveCol");
        eCollider.switchState(false);
    }
    public void MoveObject(Vector2 vel, string onlyAffectTag = null)
    {

        List<MyCollider> vcolliderr = new List<MyCollider>();
        ///////////////write point based( gen points based on positive x * y + vector speed, left right based on positive x)
        /// 
        /// 
        /// 
        /// if only x
        if(vel.y == 0 && vel.x != 0)
        {
            List<float> xvalues = new List<float>();
            float xpos = vel.x/Math.Abs(vel.x);
            vCollider.ChangeOffset(new Vector2(vel.x/2 + (transform.lossyScale.x/2 * xpos) ,0));
            vCollider.ChangeScale(new Vector2(Math.Abs(vel.x),transform.lossyScale.y));
            vcolliderr = vCollider.getallcollisions();
            if(vcolliderr.Count != 0)
            {
                
                foreach(MyCollider col in vcolliderr)
                {
                    xvalues.Add(col.getpos().x + col.getscale().x/2);
                    xvalues.Add(col.getpos().x - col.getscale().x/2);
                    
                }
                if(vel.x > 0)
                {

                    vel.x = xvalues.Min() - (transform.position.x + transform.lossyScale.x/2);
                    Debug.Log(transform.position.x + transform.lossyScale.x/2);
                    Debug.Log(xvalues.Min());
                    Debug.Log(vel.x);

                }
                else
                {
                   vel.x = xvalues.Max() - (transform.position.x - transform.lossyScale.x/2); 
                }
            }
            
        }
        /// 
        /// 
        /// 
        /// 
        /// 
        /// 
        
        // Vector2[] points = new Vector2[4];
        // Vector2[] tempPoints = new Vector2[2];
        // float c = vel.x / Math.Abs(vel.x) * vel.y / Math.Abs(vel.y);
        // for(int i = 1; i < 3;i ++)
        // {
        //     tempPoints[i] = new Vector2(transform.lossyScale.x/2 * (float)Math.Pow(c,i) + transform.position.x, transform.lossyScale.y/2 * (float)Math.Pow(c,i) * c + transform.position.y);
        // }
        // for(int i = 1; i < 3; i++)
        // {
        //     float p = vel.x / Math.Abs(vel.x);
        //     Vector2 a = tempPoints[i-1] + vel;
        //     points[(int)(0.5 + p/2) + i] = tempPoints[i-1];
        //     points[(int)(0.5 - p/2) + i] = a;

        // }
        // vCollider.ChangePoints(points);
        // vcolliderr = vCollider.getallcollisions();
        // if(vcolliderr.Count != 0)
        // {
        //     float minDistanceX = vel.x;
            
        //     foreach(MyCollider collider in vcolliderr)
        //     {
        //         if(collider.gameObject == gameObject)
        //         {
                    
        //             break;
        //         }
        //         float colfacex = (collider.offset.x) * -vel.x/Math.Abs(vel.x) + collider.gameObject.transform.position.x; 
        //         float reldistanceX = colfacex - o.transform.position.x; 
        //         if(Math.Abs(reldistanceX) < Math.Abs(minDistanceX))
        //         {
        //             minDistanceX = reldistanceX;
        //         }
        //     }
        //     vel.x = minDistanceX;
        // }

        transform.Translate(vel);
    }
    bool IsGround(GameObject o)
    {
        BoxCollider2D colliderground = o.AddComponent<BoxCollider2D>();
        return false;
    }
}
