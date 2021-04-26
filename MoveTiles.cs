using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTiles : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3;
    public float restingTime = 5;
    public float speedUpRate = 0.05f;
    public int spawnHeight;
    public int deletionHeight;
    public bool scrolling = false;
    public GameObject UIOverlay;
    public GameObject rotationPoint;
    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {

    }
    public IEnumerator gameStart()
    {
        score = 0;
        yield return new WaitForSeconds(1f);
        StartCoroutine(callNextMove());
    }
    // Update is called once per frame

    IEnumerator callNextMove()
    {
        rotationPoint.GetComponent<SelectionAndRotation>().aujustMaxHeight();
        
        if (rotationPoint.GetComponent<SelectionAndRotation>().checkInput == true)
        {
            rotationPoint.GetComponent<SelectionAndRotation>().checkInput = false;
            rotationPoint.GetComponent<SelectionAndRotation>().movement = StartCoroutine(rotationPoint.GetComponent<SelectionAndRotation>().moveSelection(0f, 2f, speed));
        }
        else
        {
            rotationPoint.GetComponent<SelectionAndRotation>().scrollHighlight = true;
        }
        StartCoroutine(moveTile());
        yield return new WaitForSeconds(restingTime);
        StartCoroutine(callNextMove());
        this.GetComponent<GenerateNinesome>().GenerateLayer(spawnHeight);
        score++;
        if (score > 0)
        {
            UIOverlay.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Score: " + score.ToString();
        }
    }
    IEnumerator moveTile()
    {
        scrolling = true;
        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("Tile"))
        {
            StartCoroutine(actualMovement(tile));
        }
        restingTime = Mathf.Clamp(restingTime - speedUpRate, 1f, Mathf.Infinity);

        yield return null;
    }

    IEnumerator actualMovement(GameObject tile)
    {
        float targetY;
        targetY = tile.transform.position.y + 2f;
        while (tile.transform.position.y != targetY)
        {
            tile.transform.position = Vector3.MoveTowards(tile.transform.position, new Vector3(tile.transform.position.x, targetY, 0), speed * Time.deltaTime);
            yield return null;
        }
        if (tile.transform.position.y == targetY)
        {
            tile.GetComponent<PositionTracker>().x = tile.transform.position.x;
            tile.GetComponent<PositionTracker>().y = tile.transform.position.y;
            scrolling = false;

            if (tile.transform.position.y >= deletionHeight)
            {
                Destroy(tile);
            }
        }
    }
}
