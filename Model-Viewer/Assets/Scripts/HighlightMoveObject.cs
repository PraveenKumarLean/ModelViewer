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
}
