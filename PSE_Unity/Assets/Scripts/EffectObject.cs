using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{


    public float lifetime = 0.90f;

    public bool dieing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dieing) {
            dieing = true;
            StartCoroutine(FadeToAndKill(0.0f, lifetime));
        }
    }

    IEnumerator FadeToAndKill(float aValue, float aTime)
     {
        Color c = transform.GetComponent<SpriteRenderer>().material.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(c.r, c.g, c.b, Mathf.Lerp(c.a, aValue, t));
            transform.GetComponent<SpriteRenderer>().material.color = newColor;
            yield return null;
        }
         Destroy(gameObject);
     }
}
