using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModelController : MonoBehaviour
{
    [Tooltip("Mouse Drag to Object Rotation Speed")]
    [SerializeField]
    [Range(0, 1)]
    private float speed;
    public Material DefaultMaterial;
    public Material X_RayMaterial;
    public Material HighLightMaterial;
    public Material TransparentMaterial;
    [SerializeField]
    private RectTransform ScrollViewRect;
    private Vector3 positionOffset;
    private float zPosition;
    [SerializeField]
    private DropDownUI dropDownUI;
    [SerializeField]
    private ModelHighlightAndMove modelHighlightAndMove;
    Vector3 previousPose;
    Vector3 currentPose;

    private void Update()
    {
        MoveWholeModel();
    }

    #region Movement
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
        if (Input.GetMouseButtonDown(1))
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

        else if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1))
        {
            modelHighlightAndMove.MouseClicked = false;
        }
    }


    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zPosition;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    #endregion

    #region Rotation

    private void RoateModel()
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
    #endregion

    #region UIButton 

    [SerializeField]
    private TextMeshProUGUI XRayText;
    [SerializeField]
    private TextMeshProUGUI TransText;
    [SerializeField]
    private TextMeshProUGUI ModelText;
    public delegate void OnButtonClickDelegate();
    private event OnButtonClickDelegate buttonClickDelegate;
    public void OnButtonPressed(string ButtonName)
    {
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
    private bool X_RayBool;
    private void X_RayMode()
    {
        X_RayBool = !X_RayBool;

        if (X_RayBool)
        {
            XRayText.color = Color.green;
            buttonClickDelegate?.Invoke();
            buttonClickDelegate += X_RayMode;
            ChangeMaterial(X_RayMaterial);

            CurrentMode = "XRay";
        }
        else
        {
            XRayText.color = Color.black;
            ChangeMaterial(DefaultMaterial);
            //  CurrentMode = "Default";
            buttonClickDelegate -= X_RayMode;
            buttonClickDelegate?.Invoke();
        }

    }

    private bool TransparentBool;

    private void TransparentMode()
    {
        TransparentBool = !TransparentBool;
        if (TransparentBool)
        {
            TransText.color = Color.green;
            buttonClickDelegate?.Invoke();
            buttonClickDelegate += TransparentMode;
            ChangeMaterial(TransparentMaterial);
            CurrentMode = "Transparent";
        }
        else
        {
            TransText.color = Color.black;
            ChangeMaterial(DefaultMaterial);
            // CurrentMode = "Default";
            buttonClickDelegate -= TransparentMode;
            buttonClickDelegate?.Invoke();
        }
    }

    private bool ModelBool;

    private void ModelMode()
    {
        ModelBool = !ModelBool;
        if (ModelBool)
        {
            ScrollViewRect.gameObject.SetActive(true);
            ModelText.color = Color.green;
            //  buttonClickDelegate?.Invoke();
            //  buttonClickDelegate += ModelMode;
        }
        else
        {
            ScrollViewRect.gameObject.SetActive(false);
            ModelText.color = Color.black;
            //  buttonClickDelegate -= ModelMode;
            //  buttonClickDelegate?.Invoke();
        }
    }


    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }

    [HideInInspector]
    public string CurrentMode = string.Empty;

    private void ChangeMaterial(Material materialToChange)
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
