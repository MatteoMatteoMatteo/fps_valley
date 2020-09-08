using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject theCoin1;

    public GameObject theCoin2;
    // Start is called before the first frame update


    public int counter = 0;

    public int coinsToWin = 1;
    public WallTransformer wallTransformer;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Instantiate at position (0, 0, 0) and zero rotation.
        Instantiate(theCoin1, new Vector3(-17.85897f, 2.9f, 6), Quaternion.identity);

        // Instantiate at position (0, 0, 0) and zero rotation.
        Instantiate(theCoin2, new Vector3(-17.85897f, 2.9f, -6), Quaternion.identity);



    }

    // Update is called once per frame
    void Update()
    {

    }
}
