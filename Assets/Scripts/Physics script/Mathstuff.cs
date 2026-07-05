using UnityEngine;
using Unity.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class Mathstuff
{
    bool onSegment(Vector2 p, Vector2 q, Vector2 r) {
        return (q[0] <= Math.Max(p[0], r[0]) && 
                q[0] >= Math.Min(p[0], r[0]) &&
                q[1] <= Math.Max(p[1], r[1]) && 
                q[1] >= Math.Min(p[1], r[1]));
    }
    int orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q[1] - p[1]) * (r[0] - q[0]) -
                  (q[0] - p[0]) * (r[1] - q[1]);

        // collinear
        if (val == 0) return 0;

        // clock or counterclock wise
        // 1 for clockwise, 2 for counterclockwise
        return (val > 0) ? 1 : 2;
    }
    public bool doIntersect(Vector2[] line1, Vector2[] line2) {
        Vector2[][] points = new Vector2[][] {line1,line2};
        // find the four orientations needed
        // for general and special cases
        int o1 = orientation(points[0][0], points[0][1], points[1][0]);
        int o2 = orientation(points[0][0], points[0][1], points[1][1]);
        int o3 = orientation(points[1][0], points[1][1], points[0][0]);
        int o4 = orientation(points[1][0], points[1][1], points[0][1]);

        // general case
        if (o1 != o2 && o3 != o4)
            return true;

        // special cases
        // p1, q1 and p2 are collinear and p2 lies on segment p1q1
        if (o1 == 0 &&
        onSegment(points[0][0], points[1][0], points[0][1])) return true;

        // p1, q1 and q2 are collinear and q2 lies on segment p1q1
        if (o2 == 0 &&
        onSegment(points[0][0], points[1][1], points[0][1])) return true;

        // p2, q2 and p1 are collinear and p1 lies on segment p2q2
        if (o3 == 0 &&
        onSegment(points[1][0], points[0][0], points[1][1])) return true;

        // p2, q2 and q1 are collinear and q1 lies on segment p2q2 
        if (o4 == 0 &&
        onSegment(points[1][0], points[0][1], points[1][1])) return true;

        return false;
    }
    public Vector2? GetIntersection(Vector2[] line1, Vector2[] line2)
    {
        Vector2 ret = new Vector2();
        Vector2 v1 = line1[0] - line1[1];
        Vector2 v2 = line2[0] - line2[1];
        float a1 = v1.y/v1.x;
        float a2 = v2.y/v2.x;
        float c1 = line1[0].y - (a1*line1[0].x);
        float c2 = line2[0].y - (a2*line2[0].x);
        float d = a1 - a2;
        if(Math.Abs(a1) == float.PositiveInfinity && Math.Abs(a2) == float.PositiveInfinity)
        {
            return null;
        }
        if(v1[0] != 0 && v2[0] != 0)
        {
            if(d == 0)
            {
                return null;
            }
            ret.x = (c2-c1) / d;
            ret.y = a1 * ret.x  + c1;
        }
        else
        {
            if(Math.Abs(a1 )== float.PositiveInfinity)
            {
                ret.x = line1[0].x;
                ret.y = (ret.x * a2) + c2;
            }
            if(Math.Abs(a2 )== float.PositiveInfinity)
            {
                ret.x = line2[0].x;
                ret.y = (ret.x * a1) + c1;
            }
        }
        bool p = Vector2.Dot((ret - line1[0]).normalized,(line1[1] - ret).normalized) < 0.99 & (ret != line1[0] || ret != line1[1]) ;

        //Debug.Log($"{p},{ret},{line2[0]},{line2[1]}");
        if(Vector2.Dot((ret - line1[0]).normalized,(line1[1] - ret).normalized) < 0.99 & (ret != line1[0] || ret != line1[1]))
        {
            return null;
        }
        return ret;

        

    }
}
