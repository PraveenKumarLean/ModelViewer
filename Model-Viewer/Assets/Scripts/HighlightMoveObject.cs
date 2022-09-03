using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightMoveObject : MonoBehaviour
{
    Renderer renderer;
    Color color;
    private void Start()
    {
        renderer = transform.GetComponent<Renderer>();
        color = renderer.material.color;
    }

    private void OnMouseEnter()
    {
        renderer.material.color = Color.blue;
    }

    private void OnMouseExit()
    {
        renderer.material.color = color;
    }

    private Vector3 positionOffset;



    private float zPosition;



    void OnMouseDown()
    {

            zPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            positionOffset = gameObject.transform.position - GetMouseAsWorldPoint();

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



    void OnMouseDrag()
    {

        transform.position = GetMouseAsWorldPoint() + positionOffset;

    }
}
