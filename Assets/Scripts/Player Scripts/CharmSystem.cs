using System;
using System.Collections.Generic;
using UnityEngine;

public class CharmSystem : MonoBehaviour
{
    BasicMovement BM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BM = GetComponent<BasicMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyChanges(Charm c)
    {
        foreach(attri a in c.effect)
        {
            
            if(a.att == "jump count")
            {
                if(changeValue(BM.Maxjump,a.mod,a.value) != null)
                {
                    BM.Maxjump = (int)changeValue(BM.Maxjump,a.mod,a.value);
                }
            }
        }
    }
    float? changeValue(float v, string m, float n)
    {
        if(m == "add")
        {
            return v + n;
        }
        return null;
    }
}
[Serializable]
public class CharmsGroup
{
    public List<Charm> Charms;
}
[Serializable]
public class Charm
{
    public string name;
    public string imagepath;
    public List<attri> effect;

}
[Serializable]
public class attri
{
    public string att;
    public float value;
    public string mod;
}
