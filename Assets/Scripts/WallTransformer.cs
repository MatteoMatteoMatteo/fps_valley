using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTransformer : MonoBehaviour
{

    public bool ready = true;
    public float goUp = 0.1f;
    public float howFast = 0.05f;
    public bool allCollected = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (ready)
        {
            if (transform.position.y <= 4.74f)
            {
                transform.position = transform.position + new Vector3(0, goUp, 0);
            }
            else
            {
                ready = !ready;
            }
        }

        if (!ready && !allCollected)
        {
            if (transform.position.x <= 20)
            {
                transform.position = transform.position + new Vector3(howFast, 0, 0);
            }
        }
        else if (!ready && allCollected)
        {
            transform.position = new Vector3(-81.1f, 4.759992f, 1.153914f);
            allCollected = false;
        }



    }

    public void moveCloser()
    {
        transform.position = transform.position + new Vector3(30, 0, 0);
    }
}
