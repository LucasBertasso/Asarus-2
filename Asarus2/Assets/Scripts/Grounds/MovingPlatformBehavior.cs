using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformBehavior : MonoBehaviour {

    public GameObject platform;

    public float movingSpeed;

    public Transform currentPoint;

    public Transform[] points;

    public int pointSelection;

    bool back = false;

	void Start () {
        currentPoint = points[pointSelection];
	}

    void Update()
    {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position,
                                                          currentPoint.position, movingSpeed * Time.deltaTime);
        if (platform.transform.position == currentPoint.position)
        {
            if(!back) pointSelection++;
            if (back) pointSelection--;
            if (pointSelection == points.Length -1 && !back)
            {
                back = true;
            }
            if (pointSelection == 0 && back)
            {
                back = false;
            }
            currentPoint = points[pointSelection];
        }
    }
}
