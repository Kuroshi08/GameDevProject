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
        /// if only y
        if(vel.y != 0 && vel.x == 0)
        {
            List<float> yvalues = new List<float>();
            float ypos = vel.y/Math.Abs(vel.y);
            vCollider.ChangeOffset(new Vector2(0 ,vel.y/2 + (transform.lossyScale.y/2 * ypos)));
            vCollider.ChangeScale(new Vector2(transform.lossyScale.x,Math.Abs(vel.y)));
            vcolliderr = vCollider.getallcollisions();
            if(vcolliderr.Count != 0)
            {
                
                foreach(MyCollider col in vcolliderr)
                {
                    yvalues.Add(col.getpos().y + col.getscale().y/2);
                    yvalues.Add(col.getpos().y - col.getscale().y/2);
                    
                }
                if(vel.y > 0)
                {

                    vel.y = yvalues.Min() - (transform.position.y + transform.lossyScale.y/2);

                }
                else
                {
                   vel.y = yvalues.Max() - (transform.position.y - transform.lossyScale.y/2); 
                }
            }
            
        } 
        /// 
        /// 
        /// 
        /// both xy
        
        if(vel.y != 0 && vel.x != 0)
        {
            eCollider.ChangeOffset(vel);
            Vector2[] spoints = new Vector2[2];
            Vector2[] rpoints = new Vector2[4];
            if((vel.x/Math.Abs(vel.x)) == (vel.y / Math.Abs(vel.y))){
                spoints[0] = new Vector2(selfcollider.getpos().x - selfcollider.getscale().x,selfcollider.getpos().y + selfcollider.getscale().y); //top left
                spoints[1] = new Vector2(selfcollider.getpos().x + selfcollider.getscale().x,selfcollider.getpos().y - selfcollider.getscale().y); //bottom right
                
                
            }
            else
            {
                spoints[0] = selfcollider.getpos() + selfcollider.getscale(); //top right
                spoints[1] = selfcollider.getpos() - selfcollider.getscale(); //bottom left

            }
            if(vel.x > 0)
            {
                
                rpoints[0] = spoints[0] + vel;
                rpoints[1] = spoints[0];
                rpoints[2] = spoints[1] + vel;
                rpoints[3] = spoints[1];
            }
            else
            {
                rpoints[1] = spoints[0] + vel;
                rpoints[0] = spoints[0];
                rpoints[3] = spoints[1] + vel;
                rpoints[2] = spoints[1];
            }
            vCollider.ChangePoints(rpoints);



            vcolliderr = vCollider.getallcollisions();
            List<MyCollider> ecolliderr = eCollider.getallcollisions(); 
            foreach(MyCollider col in ecolliderr)
            {
                if(!vcolliderr.Contains(col))
                {
                   vcolliderr.Add(col); 
                }
            }

            /// now detect if object should take x or y val () use intesect point, if d1 < d2 get d1 x or y to list, get min of list
        }
        transform.Translate(vel);
    }
    bool IsGround(GameObject o)
    {
        BoxCollider2D colliderground = o.AddComponent<BoxCollider2D>();
        return false;
    }
}
