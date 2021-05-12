using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameOverText;
    public GameObject successText;
    public GameObject fireworksObject;
    static GameObject playerObject;
    
    public bool gameOver = false;
    public bool win = false;
    
    /*private string lossText = "Game Over";
    private string winText = "Success!";*/
    List<Text> texts = new List<Text>();


    void Start()
    {
        // Store all text objects in a list
        playerObject = GameObject.Find("Player");
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Text child = this.transform.GetChild(i).gameObject.GetComponent<Text>();
            texts.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If game ends, we'll fade in the game over screen
        if (gameOver)
        {
            if (win)
            {
                fireworksObject.SetActive(true);
            }
            // Reset scene if user presses 'R'
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
                gameOver = false;
                win = false;
                fireworksObject.SetActive(false);
                return;
            }
            StartCoroutine(FadeImage(win));
        }
    }
    IEnumerator FadeImage(bool iswin)
    {
        gameOverText.SetActive(!iswin);
        successText.SetActive(iswin);
        // loop fades in opacity
        for (float i = 0; i <= 0.8f; i += Time.deltaTime)
        {
            Image image = this.GetComponent<Image>();
            image.color = new Color(0, 0, 0, i);
            // make a similar fade for text children
            foreach (var text in texts)
            {
                text.color = new Color(1, 1, 1, i + 0.2f);
            }


            yield return null;
        }
    }
}
