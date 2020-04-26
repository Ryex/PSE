using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteDir
{
    Up = 0,
    UpRight = 1,
    Right = 2,
    DownRight = 3,
    Down = 4,
    DownLeft = 5,
    Left = 6,
    UpLeft = 7,
    
}

public struct Note {
    public double time;
    public NoteDir dir;
    public double leadBeats;
    public double beatsPerMinute;

    public Note(double t, NoteDir d, double lb, double bpm) {
        time = t;
        dir = d;
        leadBeats = lb;
        beatsPerMinute = bpm;
    }

    public Note(double t, int d, double lb, double bpm) {
        time = t;
        dir = (NoteDir)d;
        leadBeats = lb;
        beatsPerMinute = bpm;
    }

    public override string ToString(){
        return "{" + $"Time: {time}, Dir: {dir}, lead: {leadBeats}, bpm: {beatsPerMinute}" + "}";
    }
}


public class NoteSpawner : MonoBehaviour
{
    public double beatsPerMinute;
    public double leadBeatsDefault;
    public bool trackPlaying;

    public NoteProcessor upLeftArrow;
    public GameObject upLeftTarget;
    public NoteProcessor upRightArrow;
    public GameObject upRightTarget;
    public NoteProcessor downLeftArrow;
    public GameObject downLeftTarget;
    public NoteProcessor downRightArrow;
    public GameObject downRightTarget;

    private double updateRate;

    public TextAsset noteFile;

    public List<Note> songNotes;

    public Note? nextNote;

    public double timeInSong;

    // Start is called before the first frame update
    void Start()
    {
        updateRate = beatsPerMinute / 60f;

        var noteLines  = noteFile.text.Split("\n"[0]);

        songNotes = new List<Note>();

        int i = 0;
        foreach (var noteLine in noteLines) {
            var noteData  = (noteLine.Trim()).Split(","[0]);

            double t = 0, lb = leadBeatsDefault, bpm = beatsPerMinute;
            NoteDir d = NoteDir.UpRight;
            switch (noteData.Length) {
                case 4:
                    bpm = double.Parse(noteData[3]);
                    goto case 3;
                case 3:
                    lb = double.Parse(noteData[2]);
                    goto case 2;
                case 2:
                    t = double.Parse(noteData[0]);
                    d = (NoteDir)int.Parse(noteData[1]);
                    break;
                default:
                    Debug.Log($"Bad line in notes CSV [{i}]: {noteLine}");
                    continue;
            }
            var note = new Note(t, d, lb, bpm);
            songNotes.Add(note);
            i++;
        }

        Debug.Log("Song notes (presort): " + string.Join("\n ", songNotes));

        songNotes.Sort((n1, n2) => n1.time.CompareTo(n2.time));
        timeInSong = 0;

        Debug.Log("Song notes: " + string.Join("\n ", songNotes));


        popNextNote();
        Debug.Log("Next note: " + nextNote);
    }


    private void popNextNote() {
        if (songNotes.Count > 0 ) {
            nextNote = songNotes[0];
            songNotes.RemoveAt(0);
        } else {
            nextNote = null;
        }
        
    }

    private void createNextNote() {

        if (nextNote.HasValue) {
            NoteProcessor prefab = null;
            GameObject target = null;
            switch (nextNote.Value.dir) {
                case NoteDir.Up:
                    //Not impl
                    break;
                case NoteDir.Down:
                    //Not impl
                    break;
                case NoteDir.Left:
                    //Not impl
                    break;
                case NoteDir.Right:
                    //Not impl
                    break;
                case NoteDir.UpRight:
                    prefab = upRightArrow;
                    target = upRightTarget;
                    break;
                case NoteDir.UpLeft:
                    prefab = upLeftArrow;
                    target = upLeftTarget;
                    break;
                case NoteDir.DownRight:
                    prefab = downRightArrow;
                    target = downRightTarget;
                    break;
                case NoteDir.DownLeft:
                    prefab = downLeftArrow;
                    target = downLeftTarget;
                    break;
                default:
                    // Unknown error?
                    break;
            }

            if (prefab != null) {
                var noteObj = Instantiate(prefab, transform.position, prefab.transform.rotation);
                noteObj.Target = target;
                noteObj.beatsPerMin = nextNote.Value.beatsPerMinute;
                noteObj.leadTimeBeats = nextNote.Value.leadBeats;
                noteObj.setupStartValues();
                noteObj.gameObject.SetActive(true);
            }
            
            popNextNote();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if(!trackPlaying) {

            // if(Input.anyKeyDown) {
            //     trackPlaying = true;
            //     // TODO PLAY TRACK
            // }
        } else {
            if (nextNote.HasValue && timeInSong >= nextNote.Value.time) {
                Debug.Log("Creating note: Song progress [" + timeInSong + "] note schedule time [" + nextNote.Value.time + "]");
                createNextNote();
                
            }
        }

    }
}
