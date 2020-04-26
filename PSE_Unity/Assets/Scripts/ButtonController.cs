using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    private SpriteRenderer renderer;

    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress)) {
            renderer.sprite = pressedImage;
        }

        if (Input.GetKeyUp(keyToPress)) {
            renderer.sprite = defaultImage;
        }
    }
}
