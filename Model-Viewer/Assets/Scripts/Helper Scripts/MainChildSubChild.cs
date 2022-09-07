using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainChildSubChild 
{
    public string MainChildUI;
    public Transform SubChildUI;
    public TextMeshProUGUI SubChildTextUI;
    public Transform ChildModel;
    public Renderer ModelRenderer;
}

public class ParentChildForDrag
{
    public Transform ParentObject;
    public Transform ChildObject;
}