using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct NoteHit {
    public const float NormalThresh = 0.5f;
    public const float GoodThresh = 0.25f;
    public const float PerfectThresh = 0.05f;
}

public enum NoteQuality {
    Normal,
    Good,
    Perfect
}

public class GameManager : MonoBehaviour
{

    public AudioSource musicTrack;

    public bool startPlaying;

    public NoteSpawner noteSpawner;


    public static GameManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerGoodNote = 150;
    public int scorePerPerfectNote = 250;
    public int missPenalty = 75;

    public int totalHits;
    public int goodHits;
    public int normalHits;
    public int perfectHits;
    public int totalMisses;

    public Text scoreText;
    public Text hitsText;

    public TextFadeAndBlink pressToStartText;



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
                pressToStartText.FadeOut();
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
        hitsText.text = $"Hits: {totalHits} | Misses: {totalMisses}";
    }

    public void NoteHit(NoteQuality quality) {
        Debug.Log("Note HIT");

        switch (quality) {
            case NoteQuality.Normal:
                currentScore += scorePerNote;
                normalHits += 1;
                break;
            case NoteQuality.Good:
                currentScore += scorePerGoodNote;
                goodHits += 1;
                break;
            case NoteQuality.Perfect:
                currentScore += scorePerPerfectNote;
                perfectHits += 1;
                break;
        }

        totalHits += 1;


    }

    public void NoteMiss() {
        Debug.Log("Note MISS");
        currentScore -= missPenalty;
        totalMisses += 1;
    }
}
