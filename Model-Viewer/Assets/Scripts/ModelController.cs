using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ModelController : MonoBehaviour
{
    
   // [SerializeField]
   // private GameObject dropDownUIModel;
    [Tooltip("Mouse Drag to Object Rotation Speed")]
    [SerializeField]
    [Range(0,1)]
    private float speed;
   // [SerializeField]
    public Material DefaultMaterial;
   // [SerializeField]
    public Material X_RayMaterial;
   // [SerializeField]
    public Material HighLightMaterial;
    [SerializeField]
    private RectTransform ScrollViewRect;
    [SerializeField]
    private GameObject ScrollViewRectIgnoreRayBG;
    private Vector3 positionOffset;
    private float zPosition;
    [SerializeField]
    private DropDownUI dropDownUI;
    [SerializeField]
    private ModelHighlightAndMove modelHighlightAndMove;
    Vector3 previousPose;
    Vector3 currentPose;

    void Update()
    {
        // Application.targetFrameRate = 30;
        MoveWholeModel();

        
    }

    private void MoveWholeModel()
    {
        if (Input.GetMouseButtonDown(2))
        {
            zPosition = Camera.main.WorldToScreenPoint(dropDownUI.Model.transform.position).z;
            positionOffset = dropDownUI.Model.transform.position - GetMouseAsWorldPoint();
            modelHighlightAndMove.MouseClicked = true;
            modelHighlightAndMove.nameDisplay.gameObject.SetActive(false);
        }
        if (Input.GetMouseButton(2))
        {
            dropDownUI.Model.transform.position = GetMouseAsWorldPoint() + positionOffset;
            
        }
        if(Input.GetMouseButtonDown(1))
        {
            previousPose = Input.mousePosition;

            modelHighlightAndMove.MouseClicked = true;
            modelHighlightAndMove.nameDisplay.gameObject.SetActive(false);
        }
        if (Input.GetMouseButton(1))
        {

            RoateModel();
            previousPose = Input.mousePosition;
        }

        else if(Input.GetMouseButtonUp(2)|| Input.GetMouseButtonUp(1))
        {
            modelHighlightAndMove.MouseClicked = false;
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
        currentPose = Input.mousePosition - previousPose;
        if (Vector3.Dot(dropDownUI.Model.transform.up, Vector3.up) >= 0)
        {

            dropDownUI.Model.transform.Rotate(dropDownUI.Model.transform.up, -Vector3.Dot(currentPose, Camera.main.transform.right) * speed, Space.World);
        }
        else
        {
            dropDownUI.Model.transform.Rotate(dropDownUI.Model.transform.up, Vector3.Dot(currentPose, Camera.main.transform.right) * speed, Space.World);
        }

        dropDownUI.Model.transform.Rotate(Camera.main.transform.right, Vector3.Dot(currentPose, Camera.main.transform.up) * speed, Space.World);



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
            ScrollViewRectIgnoreRayBG.SetActive(true);
            ModelText.color = Color.green;
            buttonClickDelegate?.Invoke();
            buttonClickDelegate += ModelMode;
        }
        else
        {
            ScrollViewRect.gameObject.SetActive(false);
            ScrollViewRectIgnoreRayBG.SetActive(false);
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
               // i.ModelRenderer.material
            }
        }
    }


    #endregion



}
