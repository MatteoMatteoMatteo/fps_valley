using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinMaster : MonoBehaviour
{
    public int counter = 0;
    public int coinsToWin;
    public WallTransformer wallTransformer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (counter == coinsToWin)
        {
            wallTransformer.allCollected = true;
        }
    }
}
