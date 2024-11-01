using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public static float slope = 20;
    public Player player;
    public List<GameObject> activeSections = new List<GameObject>();
    public float speedUpThrust;

    public float ballStartingHeight;
    public float ballStartingHorizontal;

    public bool displaySpeedUpMessage = false;
    public int score = 0;

    [SerializeField] private new Camera camera;
    [SerializeField] private float cameraAdjustmentAngle;
    [SerializeField] public GameObject startingSection;
    [SerializeField] private GameObject[] availableSections; //the first section is always going to be the starting section
    [SerializeField] private float horizontalSpaceBetweenSections;
    [SerializeField] private float verticalSpaceBetweenSections;
    [SerializeField] private int numSectionsToGenerate;

    [SerializeField] private GameObject tower;
    [SerializeField] private int towersOnEachSide;
    [SerializeField] private float yTowerShift;
    [SerializeField] private float xTowerShift;
    [SerializeField] private float yTowerRange;
    [SerializeField] private float xTowerRange;
    
    

    private static float ballInitialHeight = 114.47f;
    private static float ballInitialHorizontal = -38.28f;


    // Start is called before the first frame update
    void Start()
    {

        startingSection.GetComponent<Section>().Initialise();
        foreach (GameObject section in availableSections){
            section.GetComponent<Section>().Initialise();
        }

        Vector3 startingSectionSize = GetObjSize(startingSection);
        player.transform.SetPositionAndRotation(new Vector3(0, ballStartingHeight + ballInitialHeight, ballInitialHorizontal + ballStartingHorizontal), Quaternion.Euler(Vector3.zero));

        GenerateTowers(Vector3.zero, startingSection.GetComponent<Section>());
        activeSections.Add(Instantiate(startingSection, Vector3.zero, startingSection.transform.rotation));

        for (int i = 1; i < numSectionsToGenerate; i++){
            GameObject section = availableSections[UnityEngine.Random.Range(0, availableSections.Length)];
            activeSections.Add(Instantiate(section, PositionNextSection(section), section.transform.rotation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.SetPositionAndRotation(player.transform.position + new Vector3(0, player.cameraDistance * Mathf.Sin((slope + cameraAdjustmentAngle) * Mathf.Deg2Rad), -player.cameraDistance * Mathf.Cos((slope + cameraAdjustmentAngle) * Mathf.Deg2Rad)), Quaternion.Euler(slope + cameraAdjustmentAngle, 0, 0));      
        RemoveLaySections();

    }

    public Vector3 GetObjSize(GameObject obj){
        return obj.GetComponent<Section>().size;
    }

    private Vector3 PositionNextSection(GameObject nextSection){

        GameObject lastSection = activeSections[^1];

        Section lastSectionRef = lastSection.GetComponent<Section>();
        Section nextSectionRef = nextSection.GetComponent<Section>();
        nextSectionRef.Initialise();

        float yOffset = lastSectionRef.size.y / 2 + nextSectionRef.size.y / 2 + (nextSectionRef.centre.y - lastSectionRef.centre.y) + verticalSpaceBetweenSections;
        float zOffset = lastSectionRef.size.z / 2 + nextSectionRef.size.z / 2 + (nextSectionRef.centre.z - lastSectionRef.centre.z) + horizontalSpaceBetweenSections;

        Vector3 pos = new Vector3(lastSectionRef.centre.x, lastSection.transform.position.y - yOffset, lastSection.transform.position.z + zOffset);

        GenerateTowers(pos, nextSectionRef);

        return pos;

    }

    private void RemoveLaySections(){

        float z2, y2, z3, y3, z4, y4;

        GameObject focusSection = activeSections[0];
        Section focusSectionRef = focusSection.GetComponent<Section>();

        y2 = focusSection.transform.position.y + focusSectionRef.centre.y - focusSectionRef.size.y / 2;
        z2 = focusSection.transform.position.z + (focusSectionRef.centre.z < 0 ? -focusSectionRef.centre.z : focusSectionRef.centre.z) + focusSectionRef.size.z / 2;

        z3 = camera.transform.position.z;
        y3 = camera.transform.position.y;

        float l = 0.01f;
        float theta = camera.transform.rotation.eulerAngles.x + camera.fieldOfView / 2;

        z4 = camera.transform.position.z + l * Mathf.Cos(theta * Mathf.Deg2Rad);
        y4 = camera.transform.position.y - l * Mathf.Sin(theta * Mathf.Deg2Rad);

        float m = (y4 - y3) / (z4 - z3);
        float f = m * (z2 - z3) + y3;  

        if (f >= y2){
            
            Destroy(activeSections[0]);
            activeSections.RemoveAt(0);

            GameObject newSection = availableSections[UnityEngine.Random.Range(0, availableSections.Length)];
            activeSections.Add(Instantiate(newSection, PositionNextSection(newSection), newSection.transform.rotation));

            score++;

        }

    }

    private void GenerateTowers(Vector3 sectionPos, Section nextSectionRef){

        for (int i = 0; i < towersOnEachSide; i++){
            
            float rxpos = xTowerShift + UnityEngine.Random.Range(0, xTowerRange);
            float rypos = sectionPos.y - yTowerShift + UnityEngine.Random.Range(-yTowerRange, yTowerRange);
            float rzpos = sectionPos.z - nextSectionRef.size.z / 2 + UnityEngine.Random.Range(0, nextSectionRef.size.z);

            Instantiate(tower, new Vector3(rxpos, rypos, rzpos), tower.transform.rotation);
            
            float lxpos = -xTowerShift - UnityEngine.Random.Range(0, xTowerRange);
            float lypos = sectionPos.y - yTowerShift + UnityEngine.Random.Range(-yTowerRange, yTowerRange);
            float lzpos = sectionPos.z - nextSectionRef.size.z / 2 + UnityEngine.Random.Range(0, nextSectionRef.size.z);

            Instantiate(tower, new Vector3(lxpos, lypos, lzpos), tower.transform.rotation);

        }

    }

}
