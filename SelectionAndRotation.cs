using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectionAndRotation : MonoBehaviour
{
    public GameObject gameEngine;
    public GameObject highlightPiece;
    public float speed = 5;
    public float sizeingSpeed = 5;
    public float rotationSpeed = 25;
    public bool checkInput = true;
    public bool scrollHighlight = false;
    public bool doneGenerating;
    bool twoByTwo = false;
    public float minHeight = -7f;
    public float maxHeight = 11f;
    public float rightmostMax = 10f;

    Vector3 rayShift;
    public Coroutine movement;

    GameObject selectedTile;
    List<GameObject> selectedTiles = new List<GameObject>();

    private void Start()
    {
        rayShift = new Vector3(0.1f, -0.1f, 0f);
        highlightPiece.GetComponent<Renderer>().material.color = Color.red;
    }
    public void OnEnable()
    {
        twoByTwo = false;
        doneGenerating = false;
        checkInput = true;
        highlightPiece.transform.localScale =new Vector3(1f, 1f, 1f);
    }
    private void Update()
    {
        if (checkInput == true && Input.anyKey && highlightPiece.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                checkInput = false;
                movement = StartCoroutine(moveSelection(-2f, 0f, speed));
            }
            else
            if (Input.GetKeyDown(KeyCode.D))
            {
                checkInput = false;
                movement = StartCoroutine(moveSelection(2f, 0f, speed));
            }
            else
            if (Input.GetKeyDown(KeyCode.W))
            {
                checkInput = false;
                movement = StartCoroutine(moveSelection(0f, 2f, speed));
            }
            else
            if (Input.GetKeyDown(KeyCode.S))
            {
                checkInput = false;
                movement = StartCoroutine(moveSelection(0f, -2f, speed));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                checkInput = false;
                rotate(true);

            }else
            if (Input.GetKeyDown(KeyCode.E))
            {
                checkInput = false;
                rotate(false);
            }else
            if (Input.GetKeyDown(KeyCode.Space))
            {
                checkInput = false;
                movement = StartCoroutine(aujustSelectionSize());
            }
        }

    }
    public IEnumerator moveSelection(float xMove, float yMove, float speed)
    {
        float targetX = Mathf.Clamp(highlightPiece.transform.position.x + xMove, -9, rightmostMax);
        float targetY = Mathf.Clamp(highlightPiece.transform.position.y + yMove, minHeight, maxHeight);
        while (highlightPiece.transform.position.x != targetX || highlightPiece.transform.position.y != targetY) 
        {
            highlightPiece.transform.position = Vector3.MoveTowards(highlightPiece.transform.position,new Vector3( targetX, targetY, -0.35f), speed * Time.deltaTime);
            yield return null;
            if (scrollHighlight == true)
            {
                
                float xAujeust = targetX - highlightPiece.transform.position.x;
                float yAujust = targetY - highlightPiece.transform.position.y;
                StopCoroutine(movement);
                scrollHighlight = false;
                movement = StartCoroutine(moveSelection(xAujeust, yAujust + 2f, gameEngine.GetComponent<MoveTiles>().speed));
            }
        }
        if (highlightPiece.transform.position.x == targetX && highlightPiece.transform.position.y == targetY)
        {
            checkInput = true;

        }
    }
    IEnumerator aujustSelectionSize()
    {
        float size;
        if (twoByTwo == true)
        {
            size = 1;
            twoByTwo = false;
            rightmostMax = 7;
            minHeight = -7;
        }
        else
        {
            rightmostMax = 5;
            minHeight = -5;
            size = 2;
            twoByTwo = true;
        }
        StartCoroutine(moveSelection(0f, 0f, speed));
        Vector3 target = new Vector3(size, size, size);
        while (highlightPiece.transform.localScale != target) {

            highlightPiece.transform.localScale = Vector3.MoveTowards(highlightPiece.transform.localScale, target, sizeingSpeed * Time.deltaTime);
            yield return null;
        }
        checkInput = true;
    }
    void rotate(bool clockwise)
    {
        if (doneGenerating == true && scrollHighlight == false)
        {
            pullSelectedTiles();

            if (twoByTwo == false)
            {
                if (gameEngine.GetComponent<MoveTiles>().scrolling == false)
                {
                    rotationAnimation(selectedTile, clockwise);
                }
            }
            else
            {
                Vector3 offset = new Vector3(1f, -1f, 0f);
                if (selectedTiles[0] != null) {
                    this.transform.position = selectedTiles[0].transform.position + offset;
                    foreach (GameObject t in selectedTiles)
                    {
                        t.transform.SetParent(this.transform);
                    }
                    if (gameEngine.GetComponent<MoveTiles>().scrolling == false)
                    {
                        rotationAnimation(this.gameObject, clockwise);
                    }
                    this.transform.DetachChildren();
                }
            }
        }
        else
        {
            checkInput = true;
        }


    }
    void pullSelectedTiles()
    {

        if (twoByTwo == false)
        {
            selectedTiles.Clear();
            selectedTile = pullTile(0);
        }
        else
        {
            selectedTiles.Clear();
            selectedTile = null;
            for (int i = 0; i < 4; i++)
            {
                selectedTiles.Add(pullTile(i));
            }
        }
    }
    public void aujustMaxHeight()
    {
        maxHeight = Mathf.Clamp(maxHeight + 2f, 0, 11f);
    }
    public GameObject pullTile(int corner)
    {
        GameObject tileToSelect = null;
        Vector3 origin = highlightPiece.transform.position + rayShift;
        switch (corner)
        {
            case 1:
                origin += new Vector3(2f,0f,0f);
                break;
            case 2:
                origin += new Vector3(0f, -2f, 0f);
                break;
            case 3:
                origin += new Vector3(2f, -2f, 0f);
                break;
            default:
                break;
        }
        RaycastHit hit;
        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.forward), out hit, 1f))
        {
            tileToSelect = hit.collider.gameObject.transform.parent.gameObject;
        }
        return tileToSelect;
    }
    public void rotationAnimation(GameObject objectToRotate, bool clockwise)
    {
        if (clockwise == true)
        {
            objectToRotate.transform.Rotate(new Vector3(0f, 0f, 90f));
        }
        else
        {
            objectToRotate.transform.Rotate(new Vector3(0f, 0f, -90f));
        }
        checkInput = true;
    }
}
