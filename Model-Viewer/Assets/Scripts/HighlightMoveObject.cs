using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightMoveObject : MonoBehaviour
{
    [Tooltip("Name Display Object")]
    [SerializeField]
    private GameObject NameDisplay;
    Renderer objectRenderer;
    Color color;
    Text textBox;
    private void Start()
    {
        objectRenderer = transform.GetComponent<Renderer>();
        textBox = NameDisplay.GetComponentInChildren<Text>();
        NameDisplay.SetActive(false);
    }

    private void OnMouseEnter()
    {
        color = objectRenderer.material.color;
        objectRenderer.material.color = Color.blue;
    }

    private void OnMouseExit()
    {
        objectRenderer.material.color = color;
        
    }


    void OnMouseDown()
    {

        zPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        positionOffset = gameObject.transform.position - GetMouseAsWorldPoint();
       // NameDisplay.transform.SetParent(transform);
       // NameDisplay.transform.position = transform.position;
        NameDisplay.SetActive(true);
        textBox.text = transform.name;
    }

    private void OnMouseUp()
    {
            NameDisplay.SetActive(false);
        //if (NameDisplay.transform.parent == transform)
        //{
        //    NameDisplay.transform.SetParent(null);
        //}
    }

    void OnMouseDrag()
    {

        transform.position = GetMouseAsWorldPoint() + positionOffset;

    }

    private Vector3 positionOffset;



    private float zPosition;





    private Vector3 GetMouseAsWorldPoint()

    {

        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;



        // z coordinate of game object on screen

        mousePoint.z = zPosition;



        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);

    }



}
