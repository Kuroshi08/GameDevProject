using System;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject rhytmsys;
    RhythmSystem RS;
    AudioSource AS;
    public float offset;
    bool start;
    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RS = rhytmsys.GetComponent<RhythmSystem>();
        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(math.frac(RS.GetCurrentBeatP() + 0.5)) < 0.1f && !start)
        {
            AS.time = (math.frac(RS.GetCurrentBeatP()) ) /2 + offset;
            startBeat();
        }
    }
    void startBeat()
    {
        start = true;
        time = Time.time;
        AS.Play();
    }
}
