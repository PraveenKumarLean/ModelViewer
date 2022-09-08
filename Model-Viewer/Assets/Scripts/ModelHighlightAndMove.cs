using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public bool MouseClicked;


    [SerializeField]
    private Vector3 NameDisplayOffset;

    [SerializeField]
    public Transform nameDisplay;
    private TextMeshProUGUI nameDisplayText;


    private void Start()
    {
        nameDisplayText = nameDisplay.GetComponentInChildren<TextMeshProUGUI>();
        AssignMaterialColour();
        MouseClicked = false;
     //   Debug.Log("No Idea "+ SystemInfo.operatingSystem);
    }


    void Update()
    {

        if (!IsMouseOverUI())
        {
           

            if (Input.GetMouseButtonDown(0))
            {
                MouseClicked = true;
                OnMouseButtonDown();
                DragMultipleObject(true);
            }
            else if (!MouseClicked)
            {
                MouseHoverHighLight();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseClicked = false;
                ObjectSelectedToMove = null;
                DragMultipleObject(false);
            }

            if (Input.GetMouseButton(0))
            {
                if (ObjectSelectedToMove != null)
                {

                    ObjectSelectedToMove.transform.position = GetMouseAsWorldPoint() + positionOffset;
                    Vector3 positionss = Input.mousePosition + NameDisplayOffset;
                    nameDisplay.transform.position = positionss;
                }
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
                                  //  Debug.Log(i.ModelRenderer);
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
                        else if(SelectedChild.Exists(x => x == i.ModelRenderer))
                        {
                            if(currentRendere!= i.ModelRenderer)
                            RestColour(currentRendere);
                            currentRendere = null;
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
                       // Debug.Log("Call 2   : "   + currentRendere.name + "   " + hit.transform.name);
                        
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
                                if (ObjectSelectedToMove)
                                {
                                    HightLightUIselectionChild(false, ObjectSelectedToMove.name);
                                }
                                ObjectSelectedToMove = i.ModelRenderer;
                                HightLightUIselectionChild(true, ObjectSelectedToMove.name);



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

    bool IsMouseOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
        for(int i =0; i<raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.CompareTag("IgnoresUI"))
            {
                raycastResultsList.RemoveAt(i);
                i--;
                
                if(currentRendere) 
                {
                    RestColour(currentRendere);
                    currentRendere = null;
                }
            }
        }
        return raycastResultsList.Count > 0;
    }



    #region Color Change
    
    [SerializeField]
    private Color32 defaultColour = new Color32(195, 177, 177, 255);
    [SerializeField]
    private Color32 xRayColour = new Color32(255, 255, 255, 100);
    [SerializeField]
    private Color32 highlightColour = new Color32(255, 255, 120, 40);
    [SerializeField]
    private Color32 selectionColour = new Color32(100, 209, 189, 255);


    public Renderer currentRendere;

    List<Renderer> currentHiglight = new List<Renderer>();


    private void AssignMaterialColour()
    {
        modelController.HighLightMaterial.color = highlightColour;
        modelController.DefaultMaterial.color = defaultColour;
        modelController.X_RayMaterial.color = xRayColour;
    }

    void MouseHoverHighLight(Renderer currentChild)
    {
        currentChild.material = modelController.HighLightMaterial;
        currentChild.material.color = highlightColour;
    }

    void SelectionHighLight(Renderer currentChild)
    {
        currentChild.material = modelController.DefaultMaterial;
        currentChild.material.color = selectionColour;

        currentHiglight.Add(currentChild);
    }

    void ResetAllSelectedList()
    {
        int index = SelectedChild.Count;
        for (var i = 0; i < index; i++)
        {
            RestColour(SelectedChild[0]);
            SubchildSelectedFromUI(SelectedChild[0].name);
        }
       
    }

    void RestColour(Renderer currentRendere)
    {
        switch (modelController.CurrentMode)
        {
            case "XRay":
                XrayMode(currentRendere);
                break;
            case "Transparent":
                TransparentMode(currentRendere);
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
            currentChild.material = modelController.DefaultMaterial;
            currenHighlightName = string.Empty;
        }
    }

    void XrayMode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material = modelController.X_RayMaterial;
            currenHighlightName = string.Empty;
        }


    }
               
    void TransparentMode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material = modelController.TransparentMaterial;
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




    List<ParentChildForDrag> DragObjectCollection = new List<ParentChildForDrag>();
    void DragMultipleObject(bool Isbuttonpressed)
    {
        if (Isbuttonpressed)
        {
            foreach (var i in subChildNamesSubcribe)
            {
                if (ObjectSelectedToMove)
                {
                    if (i != ObjectSelectedToMove.name)
                    {
                        var currentTransfor = NameToRender(i).transform;
                        DragObjectCollection.Add(new ParentChildForDrag { ChildObject = currentTransfor, ParentObject = currentTransfor.parent.transform });

                        currentTransfor.SetParent(ObjectSelectedToMove.transform);
                    }
                }
            }
        }
        else
        {
            foreach (var i in DragObjectCollection)
            {
                i.ChildObject.SetParent(i.ParentObject);
            }
        }

    }




    #endregion
    Renderer NameToRender(string Name)
    {
        foreach (var i in dropDownUI.modelChildHolder)
        {
            if (i.ChildModel.name == Name)
            {
                return i.ModelRenderer;
            }
        }
        return null;
    }

    public List<string> subChildNamesSubcribe = new List<string>();

    public void SubchildSelectedFromUI(string buttonName)
    {
        Renderer SelectedRender = NameToRender(buttonName);

        if (subChildNamesSubcribe.Exists(x => x == buttonName))
        {
            HightLightUIselectionChild(false, buttonName);
            if (SelectedChild.Exists(x => x == SelectedRender))
            {
                SelectedChild.Remove(SelectedRender);
            }
        }
        else
        {
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
                    if (!subChildNamesSubcribe.Exists(x => x == buttonName))
                    {
                        subChildNamesSubcribe.Add(buttonName);
                        nameDisplay.gameObject.SetActive(false);
                    }

                    SelectedChild.Add(i.ModelRenderer);
                    SelectionHighLight(i.ModelRenderer);
                    i.SubChildTextUI.color = new Color32(156, 244, 255, 255);
                }
                else
                {
                    if (subChildNamesSubcribe.Exists(x => x == buttonName))
                    {
                        subChildNamesSubcribe.Remove(buttonName);
                        nameDisplay.gameObject.SetActive(false);
                    }
                    RestColour(i.ModelRenderer);
                    i.SubChildTextUI.color = new Color32(156, 244, 96, 255);

                }
            }
        }
    }








}
