using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class rotator : MonoBehaviour
{
    public GameObject a;
    WallTransformer aScript;
    public GameObject c1;
    public GameObject c2;
    public TextMeshPro coin1NumberText;
    public TextMeshPro coin2NumberText;
    public TextMeshProUGUI questionText;
    private int questionNumber1;
    private int questionNumber2;
    private int coin1Number;
    private int coin2Number;

    // Start is called before the first frame update
    void Start()
    {
        a = GameObject.Find("TrapHolder");
        aScript = a.GetComponent<WallTransformer>();
        questionText = GameObject.Find("Question").GetComponent<TextMeshProUGUI>();
        c1 = GameObject.Find("Coin1Text");
        coin1NumberText = c1.GetComponent<TextMeshPro>();
        c2 = GameObject.Find("Coin2Text");
        coin2NumberText = c2.GetComponent<TextMeshPro>();
        NewRound();
    }

    // Update is called once per frame

    void NewRound()
    {
        questionNumber1 = Random.Range(0, 40);
        questionNumber2 = Random.Range(0, 40);

        coin1Number = questionNumber1 + questionNumber2 - Random.Range(0, 10);
        coin2Number = questionNumber1 + questionNumber2;

        questionText.text = "Solve: " + questionNumber1 + " + " + questionNumber2 + "";

        if (Random.value < 0.5f)
        {
            coin1NumberText.text = "" + coin1Number + "";

            coin2NumberText.text = "" + coin2Number + "";

            c1.tag = "lose";
            c2.tag = "win";
        }
        else
        {
            coin1NumberText.text = "" + coin2Number + "";

            coin2NumberText.text = "" + coin1Number + "";

            c1.tag = "win";
            c2.tag = "lose";
        }


    }

    private void OnTriggerEnter(Collider other)
    {

        if (gameObject.tag == "win")
        {
            aScript.allCollected = true;
        }
        else if (gameObject.tag == "lose")
        {
            aScript.moveCloser();
        }

        NewRound();
    }
}
