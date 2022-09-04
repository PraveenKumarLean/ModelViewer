using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModelController : MonoBehaviour
{
    [Tooltip("Model Need to move and rotate")]
    [SerializeField]
    private GameObject model;
    [Tooltip("Mouse Drag to Object Rotation Speed")]
    [SerializeField]
    [Range(0,1)]
    private float speed;
    [Tooltip("Model child Parts")]
    [SerializeField]
    private Renderer[] childObjects;
    [SerializeField]
    private Material DefaultMaterial;
    [SerializeField]
    private Material X_RayMaterial;
    [SerializeField]
    private DropDownUI dropDownUI;
    private Vector3 positionOffset;
    private float zPosition;

    private void Start()
    {
        
    }

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

    [SerializeField]
    private TextMeshProUGUI XRayText;
    [SerializeField]
    private TextMeshProUGUI TransText;
    [SerializeField]
    private TextMeshProUGUI ModelText;

    public void  OnButtonPressed(string ButtonName)
    {
       // X_RayMode(false);
       // TransparentMode(false);
        switch (ButtonName)
        {
            case "XRay":
               
                X_RayMode();

                    break;
            case "Transparent":
                
                TransparentMode();
                break;
            case "Model":
                ModelMode();

                break;


        }

       
    }
    bool X_RayBool;
    void X_RayMode()
    {
        X_RayBool = !X_RayBool;

        if (X_RayBool)
        {
            XRayText.color = Color.green;
            ChangeMaterial(X_RayMaterial);

            buttonClickDelegate?.Invoke();
            buttonClickDelegate += X_RayMode;
            CurrentMode = string.Empty;
            CurrentMode = "XRay";
        }
        else
        {
            XRayText.color = Color.black;
            ChangeMaterial(DefaultMaterial);
            buttonClickDelegate -= X_RayMode;
            buttonClickDelegate?.Invoke();
            CurrentMode = string.Empty;
        }

    }

    bool TransparentBool;

    void TransparentMode()
    {
        TransparentBool = !TransparentBool;
        if (TransparentBool)
        {
            TransText.color = Color.green;
            buttonClickDelegate?.Invoke();
            buttonClickDelegate += TransparentMode;
        }
        else
        {
            TransText.color = Color.black;
            buttonClickDelegate -= TransparentMode;
            buttonClickDelegate?.Invoke();
        }
    }

    bool ModelBool;

    void ModelMode()
    {
        ModelBool = !ModelBool;
        if (ModelBool)
        {
            dropDownUI.ScrollViewRect.gameObject.SetActive(true);
            ModelText.color = Color.green;
            buttonClickDelegate?.Invoke();
            buttonClickDelegate += ModelMode;
        }
        else
        {
            dropDownUI.ScrollViewRect.gameObject.SetActive(false);
            ModelText.color = Color.black;
            buttonClickDelegate -= ModelMode;
            buttonClickDelegate?.Invoke();
        }
    }

    public string CurrentMode = string.Empty;

    void ChangeMaterial(Material materialToChange)
    {
        foreach (var i in childObjects)
        {
            i.material = materialToChange;
        }
    }




    public delegate void OnButtonClickDelegate();
    private event OnButtonClickDelegate buttonClickDelegate;

    

}
