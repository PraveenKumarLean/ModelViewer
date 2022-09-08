
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SelectionAndDisplayName : MonoBehaviour
{
    
    //[SerializeField]
   // private GameObject model;
    [SerializeField]
    private DropDownUI dropDownUI;
    [SerializeField]
    private ModelController modelController;
    [SerializeField]
    private Vector3 NameDisplayOffset;

    [SerializeField]
    private Transform nameDisplay;
    private TextMeshProUGUI nameDisplayText;
    public float val;
    public string currentName= string.Empty;
    void Start()
    {
        nameDisplayText = nameDisplay.GetComponentInChildren<TextMeshProUGUI>();
    }
   
    // Update is called once per frame
    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!DragIsSelected)
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                var CurrentSelection = hit.transform;
                CurrentSelectionOnmouse(CurrentSelection);

            }
            else
            {
                if (!IsMaterialBacktoDefault && !isObjectClicked)
                {

                    IsMaterialBacktoDefault = true;

                    CurrentMode();
                }
            }
        }

        OnMouseButtonDown(ray);

        if (Input.GetMouseButtonUp(0))
        {
            DragIsSelected = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (currentRendere != null)
            {

                currentRendere.transform.position = GetMouseAsWorldPoint() + positionOffset;
                Vector3 positionss = Input.mousePosition + NameDisplayOffset;

                nameDisplay.transform.position = positionss;
            }
            DragIsSelected = true;
        }

    }

    private void OnMouseButtonDown(Ray ray)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit _hit;
            if (Physics.Raycast(ray, out _hit, 100))
            {
                if (_hit.transform.gameObject.layer == 0)
                {
                    CurrentMode();
                    currentRendere = null;
                    nameDisplay.gameObject.SetActive(false);
                    ResetsubChildNamesSubcribe();
                    //
                }
                else if (currentRendere != null)
                {
                    //  HighLightMode(currentRendere);
                    HightLightUIselectionChild(true, currentRendere.name);
                    nameDisplay.gameObject.SetActive(true);
                    zPosition = Camera.main.WorldToScreenPoint(currentRendere.transform.position).z;
                    positionOffset = currentRendere.transform.position - GetMouseAsWorldPoint();
                    DragIsSelected = true;
                    isObjectClicked = true;
                    //stopMovingClickedOutSide = true;
                    ResetsubChildNamesSubcribe();
                    nameDisplayText.text = currentRendere.name;
                    
                }
                if(_hit.transform.gameObject.layer == 5)
                {
                    nameDisplay.gameObject.SetActive(false);
                    currentRendere = null;
                }
            }

        }
    }



    public void CurrentSelectionOnmouse(Transform CurrentSelection)
    {
        if (currentName != CurrentSelection.name)
        {
            currentName = CurrentSelection.name;

            if (CurrentSelection.gameObject.layer == 7)
            {
                foreach (var i in dropDownUI.modelChildHolder)
                {
                    if (i.ChildModel.name == CurrentSelection.name)
                    {
                        //  Debug.Log("Nmae Of the Iteam : " + i.ChildModel.name);
                          HighLightMode(i.ModelRenderer);
                        
                        if (currentRendere != i.ModelRenderer)
                        {
                            CurrentMode();
                            currentRendere = i.ModelRenderer;
                            isObjectClicked = IsMaterialBacktoDefault = false;
                            nameDisplay.gameObject.SetActive(false);
                        }

                    }
                }
            }
            else
            {
                if (!isObjectClicked)
                    CurrentMode();
            }

        }
    }



    bool DragIsSelected;
    bool isObjectClicked;
    bool IsMaterialBacktoDefault;

    #region Color Change

    [SerializeField]
    private Color32 defaultColour = new Color32(195, 177, 177, 255);
    [SerializeField]
    private Color32 xRayColour = new Color32(255, 255, 255, 16);
    [SerializeField]
    private Color32 highlightColour = new Color32(100,209,189,255);


   public  Renderer currentRendere ;

    List<Renderer> currentHiglight = new List<Renderer>();


    void CurrentMode()
    {
        
        switch (modelController.CurrentMode)
        {
            case "XRay":
                XrayMode(currentRendere);
                break;

            default:
                defaultmode(currentRendere);
                break;
               
        }
    }
    void HighLightMode(Renderer currentChild)
    {

        currentChild.material.color = highlightColour;

        currentHiglight.Add(currentChild);
    }

    void defaultmode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material.color = defaultColour;

            Debug.Log("Functioncalling Count   +   defaultmode");
           HightLightUIselectionChild(false, currentChild.name);

            currentRendere = null;
            currentName = string.Empty;
        }
    }

    void XrayMode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material.color = xRayColour;
            Debug.Log("Functioncalling Count   +   XrayMode");
            //  HightLightUIselectionChild(false, currentChild.name);
            currentRendere = null;
            currentName = string.Empty;
        }
       

    }

    #endregion

    #region SubChild Movement

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

    #endregion

    void ResetsubChildNamesSubcribe()
    {
        int index = subChildNamesSubcribe.Count;
        for (var i =0; i< index; i++)
        {
            SubchildSelectedFromUI(subChildNamesSubcribe[0]);
            if(i > index-1)
            {
                subChildNamesSubcribe.Clear();
            }
        }
       
    }



    public  List<string> subChildNamesSubcribe = new List<string>();

    public void SubchildSelectedFromUI(string buttonName)
    {
        if (subChildNamesSubcribe.Exists(x => x == buttonName))
        {
           // CurrentMode();
            HightLightUIselectionChild(false, buttonName);
            subChildNamesSubcribe.Remove(buttonName);
        }
        else
        {
            subChildNamesSubcribe.Add(buttonName);
            HightLightUIselectionChild(true, buttonName);
        }
    }

    void HightLightUIselectionChild(bool state, string buttonName)
    {
        foreach (var i in dropDownUI.modelChildHolder)
        {
            if (i.ChildModel.name == buttonName)
            {
                if (state)
                {
                    HighLightMode(i.ModelRenderer);
                    i.SubChildTextUI.color = new Color32(156, 244, 255, 255);
                }
                else if(!state)
                {
                    if(modelController.CurrentMode == "XRay")
                    {
                        i.ModelRenderer.material.color = xRayColour;
                    }
                    else
                    {
                        i.ModelRenderer.material.color = defaultColour;
                    }
                   
                    i.SubChildTextUI.color = new Color32(156, 244, 96, 255);

                }
            }
        }
    }


}


