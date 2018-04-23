using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float Timer = 0.0f;

    public float Interval = 1.0f;

    public float Speed = 1.0f;

    public bool Flipped = false;

    public bool IsRunning = true;

    public float LaneScoreMultiplier = 1.0f;



    public GameObject PrefabTarget;

    // Use this for initialization
    void Start () {
        
    }
    
    // Update is called once per frame
    void Update () {

        if (IsRunning)
        {
            Timer += Time.deltaTime;

            if (Timer > Interval)
            {
                Spawn();
                Timer = 0.0f;
            }
        }
    }

    void Spawn()
    {
        if (PrefabTarget != null)
        {
            GameObject obj = GameObject.Instantiate(PrefabTarget);
            obj.transform.position = transform.position;
            obj.transform.parent = Main.Get().Targets.transform;
            Target tgt = obj.GetComponent<Target>();
            tgt.Speed = Speed;
            tgt.Flipped = Flipped;

            tgt.PointValue = (int)((float)tgt.PointValue * LaneScoreMultiplier);

            float rand = Random.Range(0.0f, 1.0f);

            Target.Kind kind = Target.Kind.Duck;
            if (rand <= 0.65f)
                kind = Target.Kind.Duck;
            else if (rand <= 0.92f)
                kind = Target.Kind.Badguy;
            else
                kind = Target.Kind.Nun;

            // Late hack. Killing a nun gives negative points
            if (kind == Target.Kind.Nun)
            {
                tgt.PointValue *= -10;
            }

            tgt.SetKind(kind);
        }
    }


}
