using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DropDownUI : MonoBehaviour
{
    [SerializeField]
    private GameObject Model;
    
    //public RectTransform ScrollViewRect;
    [SerializeField]
    private Transform ContentContainer;
    [SerializeField]
    private GameObject m_ItemPrefab;
    [SerializeField]
    private SelectionAndDisplayName selectionAndDisplay;

   public List<MainChildSubChild> modelChildHolder = new List<MainChildSubChild>();
    private void Start()
    {

        GetAllChildInformation();

        // To get Default Rect 
       

    }
    
    
    void GetAllChildInformation()
    {
        for (var mainChildCount = 0; mainChildCount < Model.transform.childCount; mainChildCount++)
        {
            Transform _mainChild = Model.transform.GetChild(mainChildCount);
            ButtonInstantiate(_mainChild.name, true, _mainChild.name, _mainChild);
            for (var subChildCount = 0; subChildCount < _mainChild.transform.childCount; subChildCount++)
            {
                Transform _subChild = _mainChild.transform.GetChild(subChildCount);
                ButtonInstantiate(_subChild.name, false, _mainChild.name ,_subChild);

            }
        }
    }


    public List<string> MainChildNames = new List<string>();     // List of main Child Names 
    public List<string> MainChildNamesSubcribe = new List<string>();

    public void DropDownButtonPressed(Button button)
    {
        Debug.Log("Function call unnessary");
        //var _buttonName = button.name;
        if (MainChildNames.Exists(x => x == button.name))
        {

            if (MainChildNamesSubcribe.Exists(x => x == button.name))
            {
                MainChildNamesSubcribe.Remove(button.name);
                MainChildSubchildState(false, button);
            }
            else
            {
                MainChildNamesSubcribe.Add(button.name);
                MainChildSubchildState(true, button);
            }
           
        }
        else
        {
            selectionAndDisplay.SubchildSelectedFromUI(button.name);


            //foreach (var i in modelChildHolder)
            //{
            //    if(i.ModelRenderer.name == button.name)
            //    {
            //       // selectionAndDisplay.currentRendere = i.ModelRenderer;
            //    }
            //}
        }



    }

   

    void MainChildSubchildState(bool state, Button button)
    {
        foreach (var i in modelChildHolder)
        {
            if (i.MainChildUI == button.name)
            {
                i.SubChildUI.gameObject.SetActive(state);
            }
        }
    }

    void ButtonInstantiate(string Name, bool State, string mainChildName, Transform ModelChild)
    {
        var item_go = Instantiate(m_ItemPrefab);
        // do something with the instantiated item -- for instance
        var _text =   item_go.GetComponentInChildren<TextMeshProUGUI>();
        _text.text = Name;
        item_go.name = Name;
        //parent the item to the content container
        item_go.transform.SetParent(ContentContainer);
        //reset the item's scale -- this can get munged with UI prefabs
        item_go.transform.localScale = Vector2.one;
        item_go.SetActive(State);
        if (mainChildName != Name)
        {
            _text.text = "  "+Name;
            modelChildHolder.Add(new MainChildSubChild { MainChildUI = mainChildName, SubChildUI = item_go.transform , ChildModel = ModelChild , ModelRenderer = ModelChild.GetComponent<Renderer>() , SubChildTextUI  = _text });
            _text.color = new Color32(156,244,96,255);
        }
        else
        {
            _text.text = Name;
            MainChildNames.Add(Name);
            _text.color = new Color32(255, 255, 255, 255);
        }

    }


    //Vector2 rectHeightWidth;
    //Vector3 rectPosition;
    //void IncreaseScrollViewRect()
    //{
    //    if(MainChildNamesSubcribe.Count>0)
    //    {
    //        ScrollViewRect.sizeDelta = new Vector2( rectHeightWidth.x, 960);
    //        ScrollViewRect.localPosition = new Vector3(rectPosition.x, -49, rectPosition.z);
    //    }
    //    else
    //    {
    //        ScrollViewRect.sizeDelta = rectHeightWidth;
    //        ScrollViewRect.localPosition = rectPosition;
    //    }
    //}

}

public class MainChildSubChild
{
    public  string MainChildUI;
    public  Transform SubChildUI;
    public TextMeshProUGUI SubChildTextUI;
    public Transform ChildModel;
    public Renderer ModelRenderer;
}

