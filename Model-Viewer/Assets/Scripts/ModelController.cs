using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelController : MonoBehaviour
{
    [Tooltip("Model Need to move and rotate")]
    [SerializeField]
    private GameObject model;
    [Tooltip("Mouse Drag to Object Rotation Speed")]
    [SerializeField]
    private float speed;
    private Vector3 positionOffset;
    private float zPosition;

   
    void Update()
    {
        if(Input.GetMouseButtonDown(2))  
        {
            zPosition = Camera.main.WorldToScreenPoint(model.transform.position).z;
            positionOffset = model.transform.position - GetMouseAsWorldPoint();
        }
        if(Input.GetMouseButton(2))
        {
            model.transform.position = GetMouseAsWorldPoint() + positionOffset;
           
        }

        RoateModel();
    }


   

    private Vector3 GetMouseAsWorldPoint()
    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = zPosition;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }

    Vector3 previousPose;
    Vector3 currentPose;

    void RoateModel()
    {

        //float axisX = Input.GetAxis("Mouse X") * speed * Mathf.Deg2Rad;
        //float axisY = Input.GetAxis("Mouse Y") * speed * Mathf.Deg2Rad;

        //model.transform.Rotate(Vector3.up, -axisX);
        //model.transform.Rotate(Vector3.right, -axisY);

        if (Input.GetMouseButton(1))
        {

            currentPose = Input.mousePosition - previousPose;
            if (Vector3.Dot(model.transform.up, Vector3.up) >= 0)
            {

                model.transform.Rotate(model.transform.up , -Vector3.Dot(currentPose, Camera.main.transform.right) * speed, Space.World) ;
            }
            else
            {
                model.transform.Rotate(model.transform.up, Vector3.Dot(currentPose, Camera.main.transform.right) * speed, Space.World);
            }

            model.transform.Rotate(Camera.main.transform.right, Vector3.Dot(currentPose, Camera.main.transform.up) * speed, Space.World);
        }


        previousPose = Input.mousePosition;
    }

    
}
