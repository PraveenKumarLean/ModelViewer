using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModelHighlightAndMove : MonoBehaviour
{
    // Check Model is already selected
    public string currenHighlightName = string.Empty;
    public string currenelectionName = string.Empty;
    [SerializeField]
    private DropDownUI dropDownUI;
    [SerializeField]
    private ModelController modelController;
    private bool MouseClicked;


    [SerializeField]
    private Vector3 NameDisplayOffset;

    [SerializeField]
    private Transform nameDisplay;
    private TextMeshProUGUI nameDisplayText;


    private void Start()
    {
        nameDisplayText = nameDisplay.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseClicked = false;
            OnMouseButtonDown();
        }
        else if(MouseClicked)
        {
            MouseHoverHighLight();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseClicked = true;
            ObjectSelectedToMove = null;
        }

        if(Input.GetMouseButton(0))
        {
            if (ObjectSelectedToMove != null)
            {
                ObjectSelectedToMove.transform.position = GetMouseAsWorldPoint() + positionOffset;
                Vector3 positionss = Input.mousePosition + NameDisplayOffset;
                nameDisplay.transform.position = positionss;
            }
        }
       



    }
    bool IsStopHighLight;
    private void MouseHoverHighLight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            var CurrentSelection = hit.transform;
          
            if (currenHighlightName != CurrentSelection.name && CurrentSelection.gameObject.layer == 7)
            {
                currenHighlightName = CurrentSelection.name;
                IsStopHighLight = true;

                foreach (var i in dropDownUI.modelChildHolder)
                {
                    if (i.ChildModel.name == CurrentSelection.name)
                    {
                       
                        if ( !SelectedChild.Exists(x => x == i.ModelRenderer))
                        {
                          
                             MouseHoverHighLight(i.ModelRenderer);
                            if (currentRendere != i.ModelRenderer)
                            {
                                if (!SelectedChild.Exists(x => x == currentRendere))
                                {
                                    Debug.Log(i.ModelRenderer);
                                    RestColour(currentRendere);
                                    currentRendere = i.ModelRenderer;
                                }
                                else
                                {
                                    currentRendere = i.ModelRenderer;
                                    RestColour(currentRendere);
                                }
                            }
                        }
                    }
                }
               
            }
            else if (CurrentSelection.gameObject.layer == 0 )
            {
                if(IsStopHighLight)
                {
                    IsStopHighLight = false;
                   
                    if (!SelectedChild.Exists(x => x == currentRendere) && currentRendere)
                    {
                        Debug.Log("Call 2   : "   + currentRendere.name + "   " + hit.transform.name);
                        
                        RestColour(currentRendere);
                    }
                    

                }
            }

        }
    }

   public List<Renderer> SelectedChild = new List<Renderer>();
    private void OnMouseButtonDown()
    {
       // if ()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(ray, out _hit, 100))
            {
                var CurrentSelection = _hit.transform;
               
                if (CurrentSelection.gameObject.layer == 7)
                {
                    foreach (var i in dropDownUI.modelChildHolder)
                    {
                        if (i.ChildModel.name == CurrentSelection.name)
                        {
                            if( !SelectedChild.Exists(x => x.name == CurrentSelection.name))
                            {
                                ResetAllSelectedList();
                                SelectedChild.Clear();
                                ObjectSelectedToMove = i.ModelRenderer;
                                SelectedChild.Add(ObjectSelectedToMove);
                                SelectionHighLight(ObjectSelectedToMove);
                                
                            }
                            else
                            {
                                ObjectSelectedToMove = i.ModelRenderer;
                            }
                            DragPositionOffset(ObjectSelectedToMove);
                        }
                    }

                }
                else if(CurrentSelection.gameObject.layer == 0)
                {
                    ResetAllSelectedList();
                    SelectedChild.Clear();
                    nameDisplay.gameObject.SetActive(false);
                }
               

            }
           
        }
    }




    #region Color Change
    
    [SerializeField]
    private Color32 defaultColour = new Color32(195, 177, 177, 255);
    [SerializeField]
    private Color32 xRayColour = new Color32(255, 255, 255, 16);
    [SerializeField]
    private Color32 highlightColour = new Color32(255, 255, 120, 40);
    [SerializeField]
    private Color32 selectionColour = new Color32(100, 209, 189, 255);


    public Renderer currentRendere;

    List<Renderer> currentHiglight = new List<Renderer>();
    void MouseHoverHighLight(Renderer currentChild)
    {

        currentChild.material.color = highlightColour;

       // currentHiglight.Add(currentChild);
    }

    void SelectionHighLight(Renderer currentChild)
    {

        currentChild.material.color = selectionColour;

        currentHiglight.Add(currentChild);
    }

    void ResetAllSelectedList()
    {
        foreach (var i in SelectedChild)
        {
            RestColour(i);
            Debug.Log(i.name);
        }

    }

    void RestColour(Renderer currentRendere)
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
    void defaultmode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material.color = defaultColour;

           //currentRendere = null;
            currenHighlightName = string.Empty;
        }
    }

    void XrayMode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material.color = xRayColour;
            //currentRendere = null;
            currenHighlightName = string.Empty;
        }


    }

    #endregion




    #region SubChild Movement

    private Vector3 positionOffset;
    private float zPosition;
    private Renderer ObjectSelectedToMove;
    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;
        // z coordinate of game object on screen
        mousePoint.z = zPosition;
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }


    private void DragPositionOffset(Renderer currentRendere)
    {
        nameDisplay.gameObject.SetActive(true);
        nameDisplayText.text = currentRendere.name;
        // nameDisplay.gameObject.SetActive(true);
        zPosition = Camera.main.WorldToScreenPoint(currentRendere.transform.position).z;
        positionOffset = currentRendere.transform.position - GetMouseAsWorldPoint();
    }

    #endregion






}
