using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// make into monobehavior
/// </summary>
public class MyPhysics
{
    float mass;
    bool[] lockXY = new bool[2];
    bool grav;
    Vector2 vel;
    bool isGrounded;
    float Gravity = 9.8f;
    BoxCollider2D colliderx;
    BoxCollider2D collidery;
    GameObject o;


    //make this also a comp, use My colliderdirectly
    public MyPhysics(GameObject o)
    {
        this.o = o;
        colliderx = o.AddComponent<BoxCollider2D>();
        collidery = o.AddComponent<BoxCollider2D>();
    }
    public void MoveObject(Vector2 vel, string onlyAffectTag = null)
    {
        List<Collider2D> colliderxR = new List<Collider2D>();
        List<Collider2D> collideryR = new List<Collider2D>();
        
        colliderx.size = new Vector2(Math.Abs(vel.x),1);
        colliderx.offset = new Vector2(Math.Abs(vel.x/2) + (o.transform.localScale.x/2),0);
        
        collidery.size = new Vector2(1,Math.Abs(vel.y));
        collidery.offset = new Vector2(0,Math.Abs(vel.y/2) + (o.transform.localScale.y/2));
        colliderx.Overlap(colliderxR);
        collidery.Overlap(collideryR);

        if(colliderxR.Count != 0)
        {
            float minDistanceX = vel.x;
            
            foreach(Collider2D collider in colliderxR)
            {
                if(collider.gameObject == o)
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
        o.transform.Translate(vel);
    }
    bool IsGround(GameObject o)
    {
        BoxCollider2D colliderground = o.AddComponent<BoxCollider2D>();
        return false;
    }
}
