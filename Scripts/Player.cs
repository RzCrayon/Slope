using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float cameraDistance;
    [SerializeField] private float thrust;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameManager gameManager;

    private static float ythreshold = 20.0f;

    void OnTriggerEnter(Collider collider)
    { //detects when the player is over a speedup zone
        if (collider.gameObject.CompareTag("SPEEDUPZONE")){
            GetComponent<Rigidbody>().AddForce(Vector3.forward * GetComponent<Rigidbody>().mass * 225);
            gameManager.displaySpeedUpMessage = true;
        }
        if (collider.gameObject.CompareTag("KILLERBOX")){
            Terminate();
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.CompareTag("SPEEDUPZONE")){
            gameManager.displaySpeedUpMessage = false;
        }
    }

    void OnCollisionEnter(Collision collider) {
        if (collider.gameObject.CompareTag("KILLERWALL")) {
            Terminate();
        }
    }

    // Update is called once per frame
    void Update()
    {

        Rigidbody rb = GetComponent<Rigidbody>();

        float thrustFactor = rb.velocity.magnitude % 5 + 1 * thrust;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            rb.AddForce(Vector3.left * thrustFactor);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            rb.AddForce(Vector3.right * thrustFactor);
        }
        explosion.transform.position = transform.position;

        GameObject focusSection = gameManager.activeSections[0];
        Section focusSectionRef = focusSection.GetComponent<Section>();

        float relativeYPos = focusSection.transform.position.y + focusSectionRef.centre.y - focusSectionRef.size.y / 2 - ythreshold;

        if (transform.position.y < relativeYPos){
            gameObject.SetActive(false);
        }

    }

    private void Terminate(){
        Instantiate(explosion);
        gameObject.SetActive(false);
    }
}
