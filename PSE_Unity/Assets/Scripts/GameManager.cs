using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public AudioSource musicTrack;

    public bool startPlaying;

    public NoteSpawner noteSpawner;


    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int missPenalty = 20;

    public int hits;
    public int misses;

    public Text scoreText;
    public Text hitsText;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        setText();
    }

    // Update is called once per frame
    void Update()
    {
        setText();
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

    public void setText() {
        scoreText.text = $"Score: {currentScore}";
        hitsText.text = $"Hits: {hits} | Misses: {misses}";
    }

    public void NoteHit(float dist) {
        Debug.Log("Note HIT");
        currentScore += scorePerNote;
        hits += 1;
    }

    public void NoteMiss() {
        Debug.Log("Note MISS");
        currentScore -= missPenalty;
        misses += 1;
    }
}
