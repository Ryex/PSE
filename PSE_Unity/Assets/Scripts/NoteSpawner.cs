using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteDir
{
    Up = 0,
    UpOp = 1,
    Right = 2,
    RightOp = 3,
    Down = 4,
    DownOp = 5,
    Left = 6,
    LeftOp = 7,
    
}

public struct Note {
    public float time;
    public NoteDir dir;
    public float leadBeats;
    public float beatsPerMinute;

    public Note(float t, NoteDir d, float lb, float bpm) {
        time = t;
        dir = d;
        leadBeats = lb;
        beatsPerMinute = bpm;
    }

    public Note(float t, int d, float lb, float bpm) {
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
    public float beatsPerMinute;
    public float leadBeatsDefault;
    public bool trackPlaying;

    public GameObject upTarget, rightTarget, downTarget, leftTarget;

    public NoteProcessor upArrow, rightArrow, downArrow, leftArrow;
    

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    public Color optionalTint;

    private double updateRate;

    public TextAsset noteFile;

    public List<Note> songNotes;

    public Note? nextNote;

    public double timeInSong;
    public int totalNotes;

    // Start is called before the first frame update
    void Start()
    {
        updateRate = beatsPerMinute / 60f;

        var noteLines  = noteFile.text.Split("\n"[0]);

        songNotes = new List<Note>();

        int i = 0;
        foreach (var noteLine in noteLines) {
            var noteData  = (noteLine.Trim()).Split(","[0]);

            float t = 0, b = 0, lb = leadBeatsDefault, bpm = beatsPerMinute;
            NoteDir d = NoteDir.Up;
            switch (noteData.Length) {
                case 4:
                    bpm = float.Parse(noteData[3]);
                    goto case 3;
                case 3:
                    lb = float.Parse(noteData[2]);
                    goto case 2;
                case 2:
                    b = float.Parse(noteData[0]);
                    d = (NoteDir)int.Parse(noteData[1]);
                    break;
                default:
                    Debug.Log($"Bad line in notes CSV [{i}]: {noteLine}");
                    continue;
            }
            float bps = bpm / 60f;
            t = bps * b;
            var note = new Note(t, d, lb, bpm);
            songNotes.Add(note);
            i++;
        }

        totalNotes = songNotes.Count;
        Debug.Log("Total Song notes: " + totalNotes);
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
            bool optional = false;
            switch (nextNote.Value.dir) {
                case NoteDir.Up:
                    prefab = upArrow;
                    target = upTarget;
                    break;
                case NoteDir.Down:
                    prefab = downArrow;
                    target = downTarget;
                    break;
                case NoteDir.Left:
                    prefab = leftArrow;
                    target = leftTarget;
                    break;
                case NoteDir.Right:
                    prefab = rightArrow;
                    target = rightTarget;
                    break;
                case NoteDir.UpOp:
                    prefab = upArrow;
                    target = upTarget;
                    optional = true;
                    break;
                case NoteDir.RightOp:
                    prefab = rightArrow;
                    target = rightTarget;
                    optional = true;
                    break;
                case NoteDir.DownOp:
                    prefab = downArrow;
                    target = downTarget;
                    optional = true;
                    break;
                case NoteDir.LeftOp:
                    prefab = leftArrow;
                    target = leftTarget;
                    optional = true;
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

                noteObj.hitEffect = hitEffect;
                noteObj.goodEffect = goodEffect;
                noteObj.perfectEffect = perfectEffect;
                noteObj.missEffect = missEffect;

                if (optional) {
                    noteObj.SetColor(optionalTint);
                    noteObj.optionalNote = true;
                }
                

                noteObj.setupStartValues();
                noteObj.gameObject.SetActive(true);

                
            }
            
            popNextNote();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if(trackPlaying) {
            while (nextNote.HasValue && timeInSong >= nextNote.Value.time) {
                Debug.Log($"Creating note: Song progress [{timeInSong}] note schedule time [{nextNote.Value.time}]");
                createNextNote();
            }                
        }

    }
}
