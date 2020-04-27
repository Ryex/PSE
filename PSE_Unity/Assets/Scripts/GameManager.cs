using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct NoteHit {
    public const float NormalThresh = 0.5f;
    public const float GoodThresh = 0.25f;
    public const float PerfectThresh = 0.05f;
}

public enum Rank {
    F = 0,
    D = 60,
    C = 70,
    B = 82,
    A = 91,
    S = 96,
    SS = 98,
    SSS = 100
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

    public int totalHits, goodHits, normalHits, perfectHits, totalMisses;

    public Text scoreText, hitsText;

    public TextFadeAndBlink pressToStartText;

    public GameObject resultsScreen;
    public Text percentHitText, normalHitsText, goodHitsText, perfectHitsText, missedText, rankText, finalScoreText;


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
            if (!musicTrack.isPlaying && !resultsScreen.activeInHierarchy) {
                showResults();
            }
            if (resultsScreen.activeInHierarchy) {
                if (Input.anyKeyDown) {
                    Application.Quit();
                }
            }
            
        }
    }

    private void showResults() {
        resultsScreen.SetActive(true);

        normalHitsText.text = normalHits.ToString();
        goodHitsText.text = goodHits.ToString();
        perfectHitsText.text = perfectHits.ToString();

        int totalNotes = totalHits + totalMisses;

        float percentHit = ((float)totalHits / (float)totalNotes) * 100f;

        percentHitText.text = percentHit.ToString("F2") + "%";

        string rankValue = "F";
        if (percentHit > (float)Rank.D) {
            rankValue = "D";
            if (percentHit > (float)Rank.C) {
                rankValue = "C";
                if (percentHit > (float)Rank.B) {
                    rankValue = "B";
                    if (percentHit > (float)Rank.A) {
                        rankValue = "A";
                        if (percentHit > (float)Rank.S) {
                            rankValue = "S";
                            if (percentHit > (float)Rank.SS) {
                                rankValue = "SS";
                                if (percentHit < (float)Rank.SSS) {
                                    rankValue = "SSS";
                                }
                            }
                        }
                    }
                }
            }
        }
        rankText.text = rankValue;

        finalScoreText.text = currentScore.ToString();


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
