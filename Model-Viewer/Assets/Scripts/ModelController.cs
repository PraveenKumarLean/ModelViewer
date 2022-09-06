using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ModelController : MonoBehaviour
{
    [Tooltip("Model Need to move and rotate")]
    [SerializeField]
    private GameObject model;
    [Tooltip("Mouse Drag to Object Rotation Speed")]
    [SerializeField]
    [Range(0,1)]
    private float speed;
    [SerializeField]
    private Material DefaultMaterial;
    [SerializeField]
    private Material X_RayMaterial;
    [SerializeField]
    private RectTransform ScrollViewRect;
    private Vector3 positionOffset;
    private float zPosition;
    [SerializeField]
    private DropDownUI dropDownUI;

    Vector3 previousPose;
    Vector3 currentPose;

    void Update()
    {
        // Application.targetFrameRate = 30;
        MoveWholeModel();

        RoateModel();
    }

    private void MoveWholeModel()
    {
        if (Input.GetMouseButtonDown(2))
        {
            zPosition = Camera.main.WorldToScreenPoint(model.transform.position).z;
            positionOffset = model.transform.position - GetMouseAsWorldPoint();
        }
        if (Input.GetMouseButton(2))
        {
            model.transform.position = GetMouseAsWorldPoint() + positionOffset;

        }
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

    #region UIButton 

    [SerializeField]
    private TextMeshProUGUI XRayText;
    [SerializeField]
    private TextMeshProUGUI TransText;
    [SerializeField]
    private TextMeshProUGUI ModelText;
    public delegate void OnButtonClickDelegate();
    private event OnButtonClickDelegate buttonClickDelegate;
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

          
            CurrentMode = "XRay";
        }
        else
        {
            XRayText.color = Color.black;
            ChangeMaterial(DefaultMaterial);
            CurrentMode = "Transparent";
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
            ScrollViewRect.gameObject.SetActive(true);
            ModelText.color = Color.green;
            buttonClickDelegate?.Invoke();
            buttonClickDelegate += ModelMode;
        }
        else
        {
            ScrollViewRect.gameObject.SetActive(false);
            ModelText.color = Color.black;
            buttonClickDelegate -= ModelMode;
            buttonClickDelegate?.Invoke();
        }
    }


    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }

    public string CurrentMode = string.Empty;

    void ChangeMaterial(Material materialToChange)
    {

        foreach (var i in dropDownUI.modelChildHolder)
        {
            if (i.ModelRenderer)
            {
                 i.ModelRenderer.material = materialToChange;

            }
        }
    }


    #endregion



}
