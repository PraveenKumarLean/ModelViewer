using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelHighlightAndMove : MonoBehaviour
{
    // Check Model is already selected
    private string currenHighlightName = string.Empty;
    public Camera MainCamera;

    [SerializeField]
    private DropDownUI dropDownUI;
    [SerializeField]
    private ModelController modelController;
    [HideInInspector]
    public bool MouseClicked;
    [SerializeField]
    private Vector3 NameDisplayOffset;
    public Transform nameDisplay;
    private TextMeshProUGUI nameDisplayText;
    [SerializeField]
    private float ElasticLimitToRejoin;
    private bool delayObjectOnUI; 

    private void Start()
    {
        nameDisplayText = nameDisplay.GetComponentInChildren<TextMeshProUGUI>();
        AssignMaterialColour();
        MouseClicked = false;
    }


    private void Update()
    {

        if (!IsMouseOverUI())
        {
            if(!delayObjectOnUI)
            {
                
                MouseInputs();
            }
           
        }

    }

    private void MouseInputs()
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
            DragMultipleObject(false);
            ObjectSelectedToMove = null;
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

    private void DelayMouseFunction()
    {
        delayObjectOnUI = false;
    }

    #region Mouse Hover HighLight

    private bool IsStopHighLight;


    private void MouseHoverHighLight()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            var CurrentSelection = hit.transform;

            if (currenHighlightName != CurrentSelection.name && CurrentSelection.gameObject.layer == 7)
            {
                currenHighlightName = CurrentSelection.name;
                IsStopHighLight = true;

                Renderer _highLightedCurrentRender = NameToRender(CurrentSelection.name);
                if (!SelectedChild.Exists(x => x == _highLightedCurrentRender))
                {
                    MouseHoverHighLight(_highLightedCurrentRender);

                    if (currentRender != _highLightedCurrentRender)
                    {
                        if (!SelectedChild.Exists(x => x == currentRender))
                        {
                            RestColour(currentRender);
                            currentRender = _highLightedCurrentRender;
                        }
                        else
                        {
                            currentRender = _highLightedCurrentRender;
                            RestColour(currentRender);
                        }
                    }
                }
                else if (SelectedChild.Exists(x => x == _highLightedCurrentRender))
                {
                    if (currentRender != _highLightedCurrentRender)
                        RestColour(currentRender);
                    currentRender = null;
                }
            }
            else if (CurrentSelection.gameObject.layer == 0)
            {
                if (IsStopHighLight) // Stop Constant update
                {
                    IsStopHighLight = false;

                    if (!SelectedChild.Exists(x => x == currentRender) && currentRender)
                    {
                        RestColour(currentRender);
                    }
                }
            }
        }
    }



    #endregion


    #region Mouse Selection


    public List<Renderer> SelectedChild = new List<Renderer>();
    private void OnMouseButtonDown()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        if (Physics.Raycast(ray, out _hit, 100))
        {
            var CurrentSelection = _hit.transform;

            if (CurrentSelection.gameObject.layer == 7)
            {
                Renderer _highLightedCurrentRender = NameToRender(CurrentSelection.name);

                if (!SelectedChild.Exists(x => x.name == CurrentSelection.name))
                {
                    ResetAllSelectedList();
                    SelectedChild.Clear();

                    if (ObjectSelectedToMove)
                    {
                        HightLightUIselectionChild(false, ObjectSelectedToMove.name);
                    }
                    ObjectSelectedToMove = _highLightedCurrentRender;
                    HightLightUIselectionChild(true, ObjectSelectedToMove.name);
                }
                else
                {
                    ObjectSelectedToMove = _highLightedCurrentRender;
                }
                DragPositionOffset(ObjectSelectedToMove);
            }
            else if (CurrentSelection.gameObject.layer == 0)
            {
                nameDisplay.gameObject.SetActive(false);
                ResetAllSelectedList();
                SelectedChild.Clear();
            }
        }
    }

    #endregion



    #region Color Change

    [SerializeField]
    private Color32 defaultColour = new Color32(195, 177, 177, 255);
    [SerializeField]
    private Color32 xRayColour = new Color32(255, 255, 255, 100);
    [SerializeField]
    private Color32 highlightColour = new Color32(255, 255, 120, 40);
    [SerializeField]
    private Color32 selectionColour = new Color32(100, 209, 189, 255);


    public Renderer currentRender;

    private List<Renderer> currentHiglight = new List<Renderer>();


    private void AssignMaterialColour()
    {
        modelController.HighLightMaterial.color = highlightColour;
        modelController.DefaultMaterial.color = defaultColour;
        modelController.X_RayMaterial.color = xRayColour;
    }

    private void MouseHoverHighLight(Renderer currentChild)
    {
        currentChild.material = modelController.HighLightMaterial;
        currentChild.material.color = highlightColour;
    }

    private void SelectionHighLight(Renderer currentChild)
    {
        currentChild.material = modelController.DefaultMaterial;
        currentChild.material.color = selectionColour;
        currentHiglight.Add(currentChild);
    }

   
    private void ResetAllSelectedList()
    {
        int index = SelectedChild.Count;
        for (var i = 0; i < index; i++)
        {
            RestColour(SelectedChild[0]);
            SubchildSelectedFromUI(SelectedChild[0].name);
        }

    }

    private void RestColour(Renderer currentRendere)
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
                Defaultmode(currentRendere);
                break;

        }
    }
    private void Defaultmode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material = modelController.DefaultMaterial;
            currenHighlightName = string.Empty;
        }
    }

    private void XrayMode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material = modelController.X_RayMaterial;
            currenHighlightName = string.Empty;
        }


    }

    private void TransparentMode(Renderer currentChild)
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
        return MainCamera.ScreenToWorldPoint(mousePoint);
    }


    private void DragPositionOffset(Renderer currentRendere)
    {
        nameDisplay.gameObject.SetActive(true);
        nameDisplayText.text = currentRendere.name;
        zPosition = MainCamera.WorldToScreenPoint(currentRendere.transform.position).z;
        positionOffset = currentRendere.transform.position - GetMouseAsWorldPoint();
    }




    private List<ParentChildForDrag> DragObjectCollection = new List<ParentChildForDrag>();
    private void DragMultipleObject(bool Isbuttonpressed)
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
            if (ObjectSelectedToMove)
            {
                StartCoroutine(PlaceObjectOnItsRightPosition(ObjectSelectedToMove.transform));
            }
            foreach (var i in DragObjectCollection)
            {
                i.ChildObject.SetParent(i.ParentObject);
            }
            
        }

    }




    #endregion


    #region HightLight Decision Making // Hight Name in UI Button

    private List<string> subChildNamesSubcribe = new List<string>();

    public void SubchildSelectedFromUI(string buttonName)   // Input from DropDownUI for Selection and HighLight Name
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

    private void HightLightUIselectionChild(bool state, string buttonName)
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

    #endregion


    #region Return Type Functions
    private Renderer NameToRender(string Name)
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


    private bool IsMouseOverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);
        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if (raycastResultsList[i].gameObject.CompareTag("IgnoresUI"))
            {
                raycastResultsList.RemoveAt(i);
                i--;
                if (!Input.GetMouseButton(0)) // Over UI button to stop working of button click
                {
                    delayObjectOnUI = true;
                    Invoke("DelayMouseFunction", 0.001f); // When UI button Clicked to Avoid Selection of Child Part
                }
            }
        }
        return raycastResultsList.Count > 0;
    }

    Vector3 ChildDefaultPosition(string Name)
    {
        foreach (var i in dropDownUI.modelChildHolder)
        {
            if (i.ChildModel.name == Name)
            {
                return i.ChildLocalPosition;
            }
        }
        return Vector3.zero;
    }
    #endregion



    #region Reposition the Object


    IEnumerator PlaceObjectOnItsRightPosition(Transform ChildCurrentPosition)
    {

        Vector3 childDefaultPosition = ChildDefaultPosition(ChildCurrentPosition.name);
        float Distance = Vector3.Distance(ChildCurrentPosition.localPosition, childDefaultPosition);
        if(Distance < ElasticLimitToRejoin)
        {
            float elapsedTime = 0;
            float waitTime = 0.5f;
            while (elapsedTime < waitTime)
            {
                ChildCurrentPosition.transform.localPosition = Vector3.Lerp( ChildCurrentPosition.localPosition, childDefaultPosition, (elapsedTime / waitTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }


    }

    #endregion



}
