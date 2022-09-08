using TMPro;
using UnityEngine;

public class MainChildSubChild
{
    public string MainChildUI;
    public Transform SubChildUI;
    public TextMeshProUGUI SubChildTextUI;
    public Transform ChildModel;
    public Renderer ModelRenderer;
    public Vector3 ChildLocalPosition;
    public Quaternion ChildLocalRotation;

}

public class ParentChildForDrag
{
    public Transform ParentObject;
    public Transform ChildObject;
}