using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalKillerCubeMovement : MonoBehaviour
{
    
    private Vector3 cubeSize;
    private Vector3 sectionSize;
    [SerializeField] private float speed;
    private bool movingRight = true;

    void Start()
    {
        GetComponentInParent<Section>().Initialise();
        sectionSize = GetComponentInParent<Section>().size;
        cubeSize = GetComponentInChildren<MeshRenderer>().bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingRight){
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else{
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        
        if (transform.position.x + cubeSize.x / 2 >= sectionSize.x / 2 || transform.position.x - cubeSize.x / 2 <= -sectionSize.x / 2){
            movingRight = !movingRight;
        }
    }
}
