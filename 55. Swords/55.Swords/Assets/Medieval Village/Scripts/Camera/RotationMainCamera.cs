using UnityEngine;
using System;
using System.Collections;

/*Вращение и приближение камеры*/
public class RotationMainCamera : MonoBehaviour 
{
    public Transform target;
    public int SpeedX = 250;
    public int SpeedY = 250;
    public float yMinLimit = 0;
    public int yMaxLimit = 65;
    public float StartDistance = 25;
    public float ZoomMin = 3;
    public float ZoomMax = 35;
    public float ZoomScrollSpeed = 10.0f;

    private float Distance;
    private int xSpeed = 250;
    private int ySpeed = 120;
    private float x = 0.0f;
    private float y = 0.0f;

    private float MinAngle;
    private bool InCollision = false;

    void Start () 
    {
        MinAngle = yMinLimit;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        Distance = StartDistance;
    }

    void LateUpdate () 
    {
        Ray RayCenter = new Ray(new Vector3(transform.position.x, transform.position.y + 30, transform.position.z), -Vector3.up); //Пускаем луч для определения высоты земли в данной точке
        RaycastHit hit;
        if (Physics.Raycast(RayCenter, out hit, 31f))
        {
            InCollision = true;
        }
        else
        {
            InCollision = false;
        }
        /*
        Ray rayLeft = new Ray(new Vector3(transform.position.x, 50, transform.position.z), -transform.up); //Пускаем луч для определения высоты земли в данной точке
        if (Physics.Raycast(rayLeft, out hit, 0.5f))
        {
            InCollision = true;
        }
        Ray rayRight = new Ray(new Vector3(transform.position.x, 50, transform.position.z), -transform.up); //Пускаем луч для определения высоты земли в данной точке
        if (Physics.Raycast(rayRight, out hit, 0.5f))
        {
            InCollision = true;
        }
        */
        if (InCollision == true)
        {
            float ArcTangens = Mathf.Atan(Mathf.Abs(target.position.y - hit.point.y - 0.5f)/Mathf.Sqrt(Mathf.Pow(target.position.x - hit.point.x, 2) + Mathf.Pow(target.position.z - hit.point.z, 2)));
            yMinLimit = Mathf.MoveTowards(yMinLimit, Convert.ToInt16(Math.Ceiling(ArcTangens * 180 / Mathf.PI)), 2f);
        }
        if (InCollision == false || yMinLimit < MinAngle)
        {
            yMinLimit = Mathf.MoveTowards(yMinLimit, MinAngle, 2);
        }

        if (target) 
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
 		    y = ClampAngle(y, yMinLimit, yMaxLimit);
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -Distance) + target.position;
            transform.rotation = rotation;
            transform.position = position;
        }
        if (Input.GetKey(KeyCode.Mouse2))
        {
            xSpeed = SpeedX;
            ySpeed = SpeedY;
        }
        else
        {
            xSpeed = 0;
            ySpeed = 0;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            Distance = Vector3.Distance(transform.position, target.position);
            Distance = ZoomLimit(Distance - Input.GetAxis("Mouse ScrollWheel") * ZoomScrollSpeed, ZoomMin, ZoomMax);
            transform.position = -(transform.forward * Distance) + target.position;
        }
    }

    float ClampAngle (float angle, float min, float max) 
    {
	    if (angle < -360)
        {
		    angle += 360;
        }
	    if (angle > 360)
        {
		    angle -= 360;
        }
	    return Mathf.Clamp (angle, min, max);
    }

    float ZoomLimit(float dist, float min, float max)
    {

        if (dist < min)
        {
            dist = min;
        }
        if (dist > max)
        {
            dist = max;
        }
        return dist;

    }
}
