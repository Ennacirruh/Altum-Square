using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAndInitialization : MonoBehaviour
{
    public GameObject orb;
    public GameObject highlightPiece;
    public GameObject UI;
    public GameObject tutorial;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject UIOverlay;
    public int highscore = 0;
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }
    public void startGame()
    {
        UI.SetActive(false);
        UIOverlay.SetActive(true);
        orb.SetActive(true);
        highlightPiece.SetActive(true);
        StartCoroutine(this.GetComponent<GenerateNinesome>().gameStart());
    }
    public void openTutorial()
    {
        UI.SetActive(false);
        tutorial.SetActive(true);
    }
    public void exitGame()
    {
        Application.Quit();
    }
    public void endGame(bool natural)
    {
        this.GetComponent<MoveTiles>().StopAllCoroutines();
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            Destroy(tile);
        }
        orb.SetActive(false);
        highlightPiece.SetActive(false);
        if(pauseMenu.activeSelf == true)
        {
            pauseMenu.SetActive(false); 
        }
        
        UIOverlay.SetActive(false);
        if(natural == false)
        {
            Time.timeScale = 1;
            UI.SetActive(true);
            tutorial.SetActive(false);
        }
        else
        {
            gameOverMenu.SetActive(true);
            gameOverMenu.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "Score: " + Mathf.Clamp( this.GetComponent<MoveTiles>().score, 0, Mathf.Infinity).ToString();
        }
    }
    public void exitGameOver()
    {
        gameOverMenu.SetActive(false);
        UI.SetActive(true);
        if (this.GetComponent<MoveTiles>().score > highscore)
        {
            highscore = this.GetComponent<MoveTiles>().score;
            UI.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Play (Highscore: " + highscore.ToString() + ")";
        }
    }
    public void endTutorial()
    {
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            Destroy(tile);
        }
        UI.SetActive(true);
        tutorial.SetActive(false);
    }
    public void pauseGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseMenu.SetActive(true);
    }
    public void resumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
    }
}
