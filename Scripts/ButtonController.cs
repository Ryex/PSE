using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{


    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress)) {
            GetComponent<SpriteRenderer>().sprite = pressedImage;
        }

        if (Input.GetKeyUp(keyToPress)) {
            GetComponent<SpriteRenderer>().sprite = defaultImage;
        }
    }
}
