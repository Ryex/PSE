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

    public bool wasHit;


    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    

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
            if(canBePressed && !wasHit) {
                noteHit();
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Activator") {
            canBePressed = true;
        }
    }

    private void noteHit() {
        wasHit = true;
        NoteQuality quality;
        GameObject effectPrefab;
        if (distance_ > NoteHit.NormalThresh){
            noteMiss();
            return;
        } else if (distance_ > NoteHit.GoodThresh){
            quality = NoteQuality.Normal;
            effectPrefab = hitEffect;
        } else if (distance_ > NoteHit.PerfectThresh){
            quality = NoteQuality.Good;
            effectPrefab = goodEffect;
        } else {
            quality = NoteQuality.Perfect;
            effectPrefab = perfectEffect;
        }
        
        Instantiate(effectPrefab, transform.position, effectPrefab.transform.rotation);
        GameManager.instance.NoteHit(quality);
        wasHit = true;
        StartCoroutine(FadeToAndKill(0.0f, 0.2f));
    }

    private void noteMiss() {
        wasHit = true;
        GameManager.instance.NoteMiss();
        Instantiate(missEffect, transform.position, missEffect.transform.rotation);
        StartCoroutine(FadeToAndKill(0.0f, 1.0f));
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!wasHit) {
            if(other.tag == "Activator") {
                canBePressed = false;
                noteMiss();
            }
        } 
    }


    IEnumerator FadeToAndKill(float aValue, float aTime)
     {
         float alpha = transform.GetComponent<SpriteRenderer>().material.color.a;
         for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
         {
             Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, aValue, t));
             transform.GetComponent<SpriteRenderer>().material.color = newColor;
             yield return null;
         }
         Destroy(gameObject);
     }
}
