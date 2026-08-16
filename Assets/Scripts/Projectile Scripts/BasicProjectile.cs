using System;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public float starttime;
    public string spellname;
    public Vector2 direction;
    public GameObject parent;
    public projectile data;
    public IColliders col;
    public Vector2 Pos;
    public Vector2 Vel;
    public float damage;
    SpriteRenderer SR;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(data.Vel * Time.deltaTime);

        List<MyCollider> colret = col.getallcollisions(new List<string>() {"Hurtbox","Physics"}, true);
        if(colret.Count != 0)
        {
            bool hitother = false;
            foreach (MyCollider c in colret)
            {
                IHealth hpscript = c.gameObject.GetComponent<IHealth>();
                if(hpscript != null)
                {
                    hpscript.Damage(data.damage,data.immuneFrame);
                }
                if(c.gameObject != parent)
                {
                    Debug.Log(c.gameObject.name);
                    hitother = true;
                }
            }
            if(data.AttackType == "ranged" && hitother)
            {
                Destroy(gameObject);
            }
            
            
        }
        if(Time.time - starttime >= data.lifespan)
        {
            Destroy(gameObject);
        }
    }


    void Init()
    {
        starttime = Time.time;
        this.transform.position = Pos;
        this.transform.rotation = Quaternion.Euler(0,0,Vector2.Angle(direction, Vector2.right));


        Texture2D image = Resources.Load(data.ImagePath) as Texture2D;
        Rect rec = new Rect(0, 0, image.width, image.height);
        Sprite sprite = Sprite.Create(image,rec,new Vector2(0.5f,0.5f),image.width);
        if(direction.x < 0)
        {
            this.transform.Rotate(180,0,0);
        }
        SR.sprite = sprite;
        if(data.coltype == "circle")
        {
            col = gameObject.AddComponent<CircleCollider>();
        }
        if(data.coltype == "square")
        {
            col = gameObject.AddComponent<MyCollider>();
        }
        col.size = data.colsize;
        if(col == null)
        {
            Destroy(this.gameObject);
        }
    }
}
