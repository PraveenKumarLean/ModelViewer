using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDownUI : MonoBehaviour
{
    [Tooltip("Model Need to move and rotate")]
    public GameObject Model;
    [SerializeField]
    private Transform ContentContainer;
    [SerializeField]
    private GameObject m_ItemPrefab;
    [SerializeField]
    private ModelHighlightAndMove modelHighlightAndMove;
    public List<MainChildSubChild> modelChildHolder = new List<MainChildSubChild>();
    private List<string> MainChildNames = new List<string>();                       // List of main Child Names 
    private List<string> MainChildCurrentSelectionSubcribe = new List<string>();    // UI Drop down clicked to expand
    private void Start()
    {
        GetAllChildInformation();
    }

    private void GetAllChildInformation()
    {
        for (var mainChildCount = 0; mainChildCount < Model.transform.childCount; mainChildCount++)
        {
            Transform _mainChild = Model.transform.GetChild(mainChildCount);
            ButtonInstantiate(_mainChild.name, true, _mainChild.name, _mainChild);
            for (var subChildCount = 0; subChildCount < _mainChild.transform.childCount; subChildCount++)
            {
                Transform _subChild = _mainChild.transform.GetChild(subChildCount);
                ButtonInstantiate(_subChild.name, false, _mainChild.name, _subChild);
            }
        }
    }

    public void DropDownButtonPressed(Button button)  // All Instances of UIButton reference this function
    {
        if (MainChildNames.Exists(x => x == button.name))
        {
            if (MainChildCurrentSelectionSubcribe.Exists(x => x == button.name))
            {
                MainChildCurrentSelectionSubcribe.Remove(button.name);
                MainChildSubchildState(false, button);
            }
            else
            {
                MainChildCurrentSelectionSubcribe.Add(button.name);
                MainChildSubchildState(true, button);
            }
        }
        else
        {
            modelHighlightAndMove.SubchildSelectedFromUI(button.name);
        }
    }
    private void MainChildSubchildState(bool state, Button button)
    {
        foreach (var i in modelChildHolder)
        {
            if (i.MainChildUI == button.name)
            {
                i.SubChildUI.gameObject.SetActive(state);
            }
        }
    }

    private void ButtonInstantiate(string currentInstanceName, bool state, string mainChildName, Transform modelChild)
    {
        var UIButtonInstance = Instantiate(m_ItemPrefab);
        // do something with the instantiated item -- for instance
        var text = UIButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
        text.text = currentInstanceName;
        UIButtonInstance.name = currentInstanceName;
        //parent the item to the content container
        UIButtonInstance.transform.SetParent(ContentContainer);
        //reset the item's scale -- this can get munged with UI prefabs
        UIButtonInstance.transform.localScale = Vector2.one;
        UIButtonInstance.SetActive(state);
        if (mainChildName != currentInstanceName)
        {
            text.text = "  " + currentInstanceName;
            // Load data of all object in an array class
            modelChildHolder.Add(new MainChildSubChild { MainChildUI = mainChildName, SubChildUI = UIButtonInstance.transform, ChildModel = modelChild, ModelRenderer = modelChild.GetComponent<Renderer>(), SubChildTextUI = text });
            text.color = new Color32(156, 244, 96, 255);
        }
        else
        {
            text.text = currentInstanceName;
            MainChildNames.Add(currentInstanceName);
            text.color = new Color32(255, 255, 255, 255);
        }
    }
}

