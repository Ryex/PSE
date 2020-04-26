using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public AudioSource musicTrack;

    public bool startPlaying;

    public NoteSpawner noteSpawner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying) {
            if (Input.anyKeyDown) {
                startPlaying = true;
                noteSpawner.trackPlaying = true;

                musicTrack.Play();
            }
        } else {
            if (musicTrack.isPlaying && musicTrack.time >= 0) {
                noteSpawner.timeInSong = musicTrack.time;
            //Debug.Log("Music position: " + musicTrack.time);
            }
            
            
        }
    }
}
