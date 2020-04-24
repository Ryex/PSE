using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProcesser : MonoBehaviour
{

    public GameObject Target;

    public float distance_;
    public float StartDistance;
    public float leadTimeBeats;
    public float beatsPerMin;
    public float leadTimeSec;

    public float travelPerSec;

    // Start is called before the first frame update
    void Start()
    {
        StartDistance = Vector3.Distance(transform.position, Target.transform.position);
        leadTimeSec = leadTimeBeats * (beatsPerMin / 60f);
        travelPerSec = StartDistance / leadTimeSec;

    }

    // Update is called once per frame
    void Update()
    {
       
        var heading = Target.transform.position - transform.position;
        distance_ = heading.magnitude;
        var direction = heading / distance_; // normalized direction

        transform.position += direction * (travelPerSec * Time.deltaTime);

    }
}
