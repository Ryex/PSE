using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProcessor : MonoBehaviour
{

    public GameObject Target;

    public float distance_;
    public Vector3 direction_;
    public float StartDistance;
    public double leadTimeBeats;
    public double beatsPerMin;
    public double leadTimeSec;

    public float travelPerSec;

    public bool canBePressed;

    public KeyCode keyToPress;

    

    // Start is called before the first frame update
    void Start()
    {
        setupStartValues();
    }


    public void setupStartValues() {

        var heading = Target.transform.position - transform.position;

        StartDistance = distance_ = heading.magnitude;
        leadTimeSec = leadTimeBeats * (beatsPerMin / 60f);
        travelPerSec = StartDistance / (float)leadTimeSec;

        direction_ = heading / distance_; // normalized direction
    }
    // Update is called once per frame
    void Update()
    {
       
        distance_ = Vector3.Distance(transform.position, Target.transform.position);


        transform.position += direction_ * (travelPerSec * Time.deltaTime);

        if (Input.GetKeyDown(keyToPress)) {
            if(canBePressed) {
                gameObject.SetActive(false);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Activator") {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "Activator") {
            canBePressed = false;
        }
    }
}
