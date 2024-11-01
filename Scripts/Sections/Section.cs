using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Section : MonoBehaviour{
    
    public Vector3 size = Vector3.zero;
    public Vector3 centre = Vector3.zero;

    public void Initialise(){
        //iterates through every child of the group
        foreach (Transform component in GetComponentInChildren<Transform>()){
            if (component.gameObject.CompareTag("SIZEBOUNDS")){
                centre = component.localPosition;
                if (component.parent.rotation.eulerAngles.y == 0.0f){
                    centre.z = -centre.z;
                }
                size = component.GetComponent<MeshRenderer>().bounds.size;
            }
        }
    }

}