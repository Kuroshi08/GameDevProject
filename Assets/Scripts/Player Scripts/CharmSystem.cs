using System.Collections.Generic;
using UnityEngine;

public class CharmSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class Charm
{
    public string name;
    public string imagepath;
    public List<attri> effect;

}
public class attri
{
    public string att;
    public float value;
    public string mod;
}
