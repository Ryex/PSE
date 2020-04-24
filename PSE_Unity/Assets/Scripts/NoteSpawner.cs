using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public float beatTempo;
    public bool trackPlaying;

    private float updateRate;

    // Start is called before the first frame update
    void Start()
    {
        updateRate = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!trackPlaying) {

            if(Input.anyKeyDown) {
                trackPlaying = true;
                // TODO PLAY TRACK
            }
        }

    }
}
