using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionAndDisplayName : MonoBehaviour
{
    
    [SerializeField]
    private GameObject model;



    public string Tile;
    public Transform PreviousSelection;
    public Transform CurrentSelection;
    // Start is called before the first frame update
    void Start()
    {
        LoadDataToModelInformation();



    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,100))
        {
            CurrentSelection = hit.transform;

            if(CurrentSelection.gameObject.layer == 7)
            {
                foreach(var i in ModelInformationList)
                {
                    if (i.Name == CurrentSelection.name)
                    {
                        Debug.Log("Nmae Of the Iteam : "+i.Name);
                    }
                }
            }




            
                Tile = hit.transform.gameObject.name;

            if (hit.transform.tag == "tiles")
            {
            }
            else
            {
                Tile = null;
            }

        }
    }






    void HighLightObject(GameObject CurrentChild)
    {

        DeHighLightObject(PreviousSelection.gameObject);

        PreviousSelection = CurrentSelection;
       // objectRenderer = transform.GetComponent<Renderer>();

    }

    void DeHighLightObject(GameObject CurrentChild)
    {


    }

    List<ModelInformation> ModelInformationList = new List<ModelInformation>();


    void LoadDataToModelInformation()
    {
        Transform[] allChildren = model.GetComponentsInChildren<Transform>();


        foreach(var I in allChildren)
        {

            if(I.GetComponent<MeshRenderer>() != null)
            {

                ModelInformationList.Add(new ModelInformation { Name = I.name, ChildModel = I.transform , ModelRenderer = I.GetComponent<MeshRenderer>() });
               
            }
        }
    }


    
}



class ModelInformation
{
    public string Name;
    public Transform ChildModel;
    public Renderer ModelRenderer;


}