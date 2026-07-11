using System;
using System.Security.Cryptography;
using Mono.Cecil.Cil;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject CameraO;
    public GameObject CurrentScreen;
    Vector2 MovementBuffer = new Vector2(1,1);
    float CameraSpeed = 5;
    BasicMovement movementScript;
    Camera Camera;
    Vector2 CameraView;
    Vector2 CurrentLevelSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementScript = GetComponent<BasicMovement>();
        Camera = CameraO.GetComponent<Camera>();
        CameraView.y = Camera.orthographicSize * 2;
        CameraView.x = CameraView.y * (Camera.pixelWidth/Camera.pixelHeight);
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }
    void FollowPlayer()
    {
        float distancex = transform.position.x - CameraO.transform.position.x;
        if(Math.Abs(distancex) > MovementBuffer.x)
        {
            float translateD = distancex - (MovementBuffer.x * (distancex/Math.Abs(distancex)));
            MoveCameraInBounds(new Vector2(translateD,0));
        }
        float distancey = transform.position.y - CameraO.transform.position.y;
        if(Math.Abs(distancey) > MovementBuffer.y)
        {
            float translateD = distancey - (MovementBuffer.y * (distancey/Math.Abs(distancey)));
            MoveCameraInBounds(new Vector2(0,translateD));
        }
        if(movementScript.MoveDir.x == 0)
        {
            if(Math.Abs(distancex) > CameraSpeed * Time.deltaTime)
            {
                MoveCameraInBounds(new Vector2(CameraSpeed * Time.deltaTime * (distancex/Math.Abs(distancex)),0));
            }
            else
            {
                MoveCameraInBounds(new Vector2(distancex,0));
            }
        }
    }
    void MoveCameraInBounds(Vector2 V)
    {
        if(CurrentScreen != null)
        {

            ScreenScript sc = CurrentScreen.GetComponent<ScreenScript>();
            Vector2 boundstr = sc.topright - CameraView/2;
            Vector2 boundsbl = sc.bottomleft + CameraView/2;
            
            if(boundstr.x <= boundsbl.x)
            {
                V.x = CameraO.transform.position.x - ((sc.topright.x - sc.bottomleft.x)/2);
            }
            if(boundstr.y <= boundsbl.y)
            {
                V.y = CameraO.transform.position.y - ((sc.topright.y - sc.bottomleft.y)/2);
            }

            Vector2 newPos = (Vector2)CameraO.transform.position + V;
            if(newPos.x > boundstr.x)
            {
                V.x = boundstr.x - CameraO.transform.position.x;
            }
            if(newPos.x < boundsbl.x)
            {
                V.x = boundsbl.x - CameraO.transform.position.x;
            }
            if(newPos.y > boundstr.y)
            {
                V.y = boundstr.y - CameraO.transform.position.y;
            }
            if(newPos.y < boundsbl.y)
            {
                V.y = boundsbl.y - CameraO.transform.position.y;
            }
        }
        Camera.transform.Translate(V);
    }
}
