using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreenScript : MonoBehaviour
{
    public Vector2 topright;
    public Vector2 bottomleft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform Bound = transform.Find("Bounds");
        List<float> xValues = new List<float>();
        List<float> yValues = new List<float>();
        foreach(Transform child in Bound)
        {
            xValues.Add(child.localPosition.x + child.localScale.x/2);
            xValues.Add(child.localPosition.x - child.localScale.x/2);
            yValues.Add(child.localPosition.y + child.localScale.y/2);
            yValues.Add(child.localPosition.y - child.localScale.y/2);
        }
        topright = new Vector2(xValues.Max(),yValues.Max());
        bottomleft = new Vector2(xValues.Min(),yValues.Min());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
