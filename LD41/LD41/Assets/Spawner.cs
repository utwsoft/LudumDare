using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float Timer = 0.0f;

    public float Interval = 1.0f;

    public float Speed = 1.0f;

    public bool Flipped = false;

    public bool IsRunning = true;



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

            float rand = Random.Range(0.0f, 1.0f);

            tgt.SetKind((rand <= 0.75f) ? Target.Kind.Duck : Target.Kind.Badguy);
        }
    }


}
