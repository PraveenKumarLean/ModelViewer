
using UnityEngine;
using TMPro;

public class SelectionAndDisplayName : MonoBehaviour
{
    
    [SerializeField]
    private GameObject model;
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


    string currentName= string.Empty;
    // Start is called before the first frame update
    void Start()
    {
        // LoadDataToModelInformation();
       

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
            else
            {
                if (!IsMaterialBacktoDefault && !isObjectClicked)
                {
                   
                    IsMaterialBacktoDefault = true;
                   
                    CurrentMode();
                }
            }
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.layer == 0)
                {
                    CurrentMode();
                    currentRendere = null;
                    nameDisplay.gameObject.SetActive(false);
                }
                else if (currentRendere != null)
                {
                    HighLightMode(currentRendere);
                    nameDisplay.gameObject.SetActive(true);
                    zPosition = Camera.main.WorldToScreenPoint(currentRendere.transform.position).z;
                    positionOffset = currentRendere.transform.position - GetMouseAsWorldPoint();
                    DragIsSelected = true;
                    isObjectClicked = true;
                    //stopMovingClickedOutSide = true;
                    nameDisplayText.text = currentRendere.name;
                }
            }

           
            // NameDisplay.transform.SetParent(transform);
            // NameDisplay.transform.position = transform.position;
            // NameDisplay.SetActive(true);
            //textBox.text = transform.name;
        }

        if (Input.GetMouseButtonUp(0))
        {
            DragIsSelected = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (currentRendere != null )
            {
               
                currentRendere.transform.position = GetMouseAsWorldPoint() + positionOffset;
                //nameDisplayCanvas.transform.SetParent(currentRendere.transform);
                Vector3 positionss = Input.mousePosition + NameDisplayOffset;

                nameDisplay.transform.position = positionss;
            }
            DragIsSelected = true;
        }
    }

    bool DragIsSelected;
    bool isObjectClicked;
    bool IsMaterialBacktoDefault;
   // bool stopMovingClickedOutSide;
   // Vector2 nameDisplayStartPosition;















    [SerializeField]
    private Color32 defaultColour = new Color32(195, 177, 177, 255);
    [SerializeField]
    private Color32 xRayColour = new Color32(255, 255, 255, 16);
    [SerializeField]
    private Color32 highlightColour = new Color32(100,209,189,255);


    public Renderer currentRendere ;


    public void HighLightMode(Renderer currentChild)
    {

        currentChild.material.color = highlightColour;

    }


    public void CurrentMode()
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
            currentRendere = null;
            currentName = string.Empty;
        }
    }

    void XrayMode(Renderer currentChild)
    {
        if (currentChild != null)
        {
            currentChild.material.color = xRayColour;
            currentRendere = null;
            currentName = string.Empty;
        }
       

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


