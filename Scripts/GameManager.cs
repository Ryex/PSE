using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct NoteHit {
    public const float NormalThresh = 0.28f;
    public const float GoodThresh = 0.15f;
    public const float PerfectThresh = 0.07f;
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

    public KeyCode debugFastForwardKey;

    public float trackTargetVolume = 0.6f;
    public float trackFadeInTime = 2.4f;
    public float trackStartTime;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        setText();
        currentScore = 0;
        musicTrack.volume = 0;
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
                musicTrack.time = trackStartTime;
                musicTrack.Play();
                StartCoroutine(FadeTrack(trackTargetVolume, trackFadeInTime));
            }
        } else {
            if (Debug.isDebugBuild && Input.GetKeyDown(debugFastForwardKey)) {
                float fastFastforward = musicTrack.time + (musicTrack.clip.length / 8f);
                if (fastFastforward > musicTrack.clip.length - 0.01f) {
                    fastFastforward = musicTrack.clip.length - 0.01f;
                }
                musicTrack.time = fastFastforward;
            }
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

        missedText.text = totalMisses.ToString();

        int totalNotes = noteSpawner.totalNotes;

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

    public void NoteHit(NoteQuality quality, bool optional) {
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

    public void NoteMiss(bool optional) {
        Debug.Log("Note MISS");
        if (!optional) {
            currentScore -= missPenalty;
        }
        totalMisses += 1;
    }

    IEnumerator FadeTrack(float vValue, float fTime)
    {
        float startVol = musicTrack.volume;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fTime)
        {
            musicTrack.volume = Mathf.Lerp(startVol, vValue, t);
            yield return null;
        }
    }
}
