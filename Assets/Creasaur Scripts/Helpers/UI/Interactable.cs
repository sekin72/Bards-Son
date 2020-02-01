using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Action OpenPopUp;

#if !UNITY_EDITOR
    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("RayInteractor");
    }
#endif

    public void OnClicked()
    {
        OpenPopUp.SafeInvoke();
    }
}