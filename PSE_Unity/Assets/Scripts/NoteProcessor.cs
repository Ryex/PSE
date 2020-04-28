using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteProcessor : MonoBehaviour
{

    public GameObject Target;

    public float distance_;
    public Vector3 direction_;
    public float StartDistance;
    public float leadTimeBeats;
    public float beatsPerMin;
    public float leadTimeSec;

    public float travelPerSec;

    public bool canBePressed;

    public KeyCode keyToPress;

    public bool wasHit;

    public Color tintColor;
    public bool optionalNote;


    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    

    // Start is called before the first frame update
    void Start()
    {
        tintColor = transform.GetComponent<SpriteRenderer>().material.color;
        optionalNote = false;
    }


    public void setupStartValues() {

        var heading = Target.transform.position - transform.position;

        StartDistance = distance_ = heading.magnitude;
        leadTimeSec = leadTimeBeats / (beatsPerMin / 60f) ;
        travelPerSec = StartDistance / (float)leadTimeSec;

        direction_ = heading / distance_; // normalized direction

        FadeIn(leadTimeSec / 2f);
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

    public void SetColor(Color c) {
        tintColor = c;
        transform.GetComponent<SpriteRenderer>().material.color = c;
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
        GameManager.instance.NoteHit(quality, optionalNote);
        wasHit = true;
        FadeToAndKill(0.0f, 0.2f);
    }

    private void noteMiss() {
        wasHit = true;
        GameManager.instance.NoteMiss(optionalNote);
        Instantiate(missEffect, transform.position, missEffect.transform.rotation);
        FadeToAndKill(0.0f, 0.5f);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!wasHit) {
            if(other.tag == "Activator") {
                canBePressed = false;
                noteMiss();
            }
        } 
    }


    private void FadeToAndKill(float aValue, float aTime)
    {
        StartCoroutine(FadeTo(aValue,aTime));
        Destroy(gameObject, aTime);
    }

    private void FadeIn(float time) {
        Color c = tintColor;
        transform.GetComponent<SpriteRenderer>().material.color = new Color(c.r, c.g, c.b, 0);
        StartCoroutine(FadeTo(1.0f, time));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        Color c = transform.GetComponent<SpriteRenderer>().material.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(c.r, c.g, c.b, Mathf.Lerp(c.a, aValue, t));
            transform.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }
    }
}
