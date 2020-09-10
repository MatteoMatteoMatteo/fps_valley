using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class WallTransformer : MonoBehaviour
{
    public GameObject scoreObj;

    AudioSource dying;
    public GameObject dead;
    public bool alreadyDead = false;
    public bool isRestarting = false;
    public bool ready = true;
    public float goUp = 0.1f;
    public float howFast = 0.05f;
    public bool allCollected = false;

    private float score = 0;
    private float highScore = 0;
    private string highscore_path = "highscore.txt";


    // Start is called before the first frame update
    void Start()
    {
        dying = GetComponent<AudioSource>();
        if (!System.IO.File.Exists(highscore_path))
        {
            StreamWriter writer = new StreamWriter(highscore_path, true);
            writer.WriteLine("0");
            writer.Close();
        }

    }

    // Update is called once per frame
    void Update()
    {
        score = score + 1 * Time.deltaTime;


        if (ready && !isRestarting)
        {
            if (transform.position.y <= 4.74f)
            {
                transform.position = transform.position + new Vector3(0, goUp * Time.deltaTime, 0);
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
                transform.position = transform.position + new Vector3(howFast, 0, 0) * Time.deltaTime;
            }
        }
        else if (!ready && allCollected)
        {
            transform.position = new Vector3(-90.1f, 4.759992f, 1.153914f);
            allCollected = false;
        }

        if (transform.position.x >= 0)
        {
            transform.position = new Vector3(-81.1f, -4.759992f, 1.153914f);
            Dying();

        }
    }

    private void Dying()
    {

        if (!isRestarting)
        {
            dying.Play();
            dead.SetActive(true);
            highScore = float.Parse(File.ReadAllText(@highscore_path));
            if (score > highScore)
            {
                File.WriteAllText(@highscore_path, "" + score);
                scoreObj.GetComponent<TextMeshProUGUI>().text = "New highscore: " + score + " (previous score: " + highScore + ")";
            }
            else
            {
                scoreObj.GetComponent<TextMeshProUGUI>().text = "Score: " + score + " (Highscore: " + highScore + ")";
            }
            StartCoroutine(ExecuteAfterTime(3));
            isRestarting = true;
        }

    }

    public void moveCloser()
    {
        transform.position = transform.position + new Vector3(25, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && !alreadyDead)
        {
            transform.position = new Vector3(-81.1f, -4.759992f, 1.153914f);
            Dying();
            alreadyDead = true;
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        transform.position = new Vector3(-81.1f, -4.759992f, 1.153914f);
        ready = true;
        dead.SetActive(false);
        isRestarting = false;
        alreadyDead = false;
        score = 0;

    }

}
