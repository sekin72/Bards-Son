using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(UpgradeableModel))]
public class InteractableCustomEditor : Editor
{
    private void OnEnable()
    {
        var obj = (UpgradeableModel) target;
        obj.gameObject.layer = LayerMask.NameToLayer("RayInteractor");
    }
}
#endif