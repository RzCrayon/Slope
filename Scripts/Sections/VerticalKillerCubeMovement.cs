using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class VerticalKillerCubeMovement : MonoBehaviour
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float speed;
    
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 defaultEnd;
    
    private float timeRequired;
    private float timeAccumulated = 0.0f;
    

    void Start(){
        startPos = transform.localPosition;
        defaultEnd = new Vector3(transform.localPosition.x, 0.0f, 0.0f);
        SetEndPosToMax();
        CalculateTimeRequired();
    }

    // Update is called once per frame
    void Update()
    {

        timeAccumulated += Time.deltaTime;
        float factor =  timeAccumulated / timeRequired;

        if (factor >= 1){
            startPos = transform.localPosition;
            if (endPos != defaultEnd){
                endPos = defaultEnd;
            }
            else{
                SetEndPosToMax();
            }
            CalculateTimeRequired();
            timeAccumulated = 0.0f;
            factor = 0.0f;
        }

        transform.localPosition = Vector3.Lerp(startPos, endPos, factor);
        
    }

    private void CalculateTimeRequired(){
        timeRequired = Vector3.Distance(startPos, endPos) / speed;
    }

    private void SetEndPosToMax(){
        endPos = new Vector3(transform.localPosition.x, maxHeight * Mathf.Cos(GameManager.slope * Mathf.Deg2Rad), maxHeight * Mathf.Sin(GameManager.slope * Mathf.Deg2Rad));
    }



}
