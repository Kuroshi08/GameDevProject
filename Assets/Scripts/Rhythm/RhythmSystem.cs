using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RhythmSystem : MonoBehaviour
{
    public float BPM = 120;
    public double StartTime;
    public float Lpercent = 10;
    float currentBeat;
    float ExactBeat;
    public GameObject Heart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartRhythm();
    }
    void StartRhythm()
    {
        StartTime = Time.timeAsDouble;
    }
    // Update is called once per frame
    void Update()
    {
        if (ifnewbeat())
        {
            StartCoroutine(PulseHeart());
        }
    }
    IEnumerator PulseHeart()
    {
        
        yield return new WaitForSeconds(0.01f);
        Image i = Heart.GetComponent<Image>();
        
        for(int a = 0; a < 10; a++)
        {
            if(a < 5)
            {
                i.color = i.color + new Color(0.1f,0.1f,0.1f);
            }
            else
            {
                i.color = i.color - new Color(0.1f,0.1f,0.1f);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    bool ifnewbeat()
    {
        double SPB = 1/(BPM/60);
        double timeSinceStart = Time.timeAsDouble - StartTime;
        if(Math.Floor(timeSinceStart/SPB) != ExactBeat)
        {
            ExactBeat = (float)Math.Floor(timeSinceStart/SPB);
            return true;

        }
        return false;
    }
    float GetCurrentBeatP()
    {
        double SPB = 1/(BPM/60);
        double timeSinceStart = Time.timeAsDouble - StartTime;
        return((float)(timeSinceStart/SPB));
    }
    
}
