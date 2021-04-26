using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNinesome : MonoBehaviour
{
    public GameObject fourWayPiece;
    public GameObject cornerPiece;
    public GameObject deadEndPiece;
    public GameObject junctionPiece;
    public GameObject solidPiece;
    public GameObject straightPiece;
    public bool texturePieces = false;
    public Material texture;
    // Start is called before the first frame update

    private void Start()
    {

    }
    public IEnumerator gameStart()
    {
        for (int i = 9; i > 0; i--)
        {
            GenerateLayer(i * -2);
            if (i == 1)
            {
                StartCoroutine(this.GetComponent<MoveTiles>().gameStart());
                this.GetComponent<MoveTiles>().rotationPoint.GetComponent<SelectionAndRotation>().doneGenerating = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void GenerateLayer(int xPos)
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject tile = Instantiate(selectRandomTile());
            tile.transform.position = new Vector3(-8 + (2f * i), xPos,0f);
            tile.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,randomDirection()));
            tile.AddComponent<PositionTracker>();
            if (texturePieces == true)
            {
                tile.GetComponent<Renderer>().material = texture;
            }
        }
    }

    public GameObject selectRandomTile()
    {
        GameObject selectedObject = solidPiece;
        float num = Mathf.RoundToInt(Random.Range(0,100));
        if (num >= 0 && num < 10)
        {
            selectedObject = solidPiece;
        }
        else if (num >= 10 && num < 30)
        {
            selectedObject = junctionPiece;
        }
        else if (num >= 30 && num < 50)
        {
            selectedObject = fourWayPiece;
        }
        else if (num >= 50 && num < 75)
        {
            selectedObject = cornerPiece;
        }
        else if (num >= 75 && num <= 100)
        {
            selectedObject = straightPiece;
        }
        return selectedObject;
    }
    public float randomDirection()
    {
        float finalAngle;
        float num = Mathf.RoundToInt(Random.Range(0, 3));
        switch (num)
        {
            case 0:
                finalAngle = 0f;
                break;
            case 1:
                finalAngle = 90f;
                break;
            case 2:
                finalAngle = 180f;
                break;
            case 3:
                finalAngle = 270f;
                break;
            default:
                finalAngle = 0f;
                break;
        }
        return finalAngle;
    }


}
