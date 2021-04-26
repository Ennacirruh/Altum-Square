using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMovement : MonoBehaviour
{
    public float speed = 3;
    public float restingTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(this.transform.position.x, -6, 0);
        StartCoroutine(callNextMove());
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(0, 12f, 0), speed * Time.deltaTime);
    }

    IEnumerator callNextMove()
    {
        StartCoroutine(moveTile());

        yield return new WaitForSeconds(restingTime);
        StartCoroutine(callNextMove());
    }
    IEnumerator moveTile()
    {
        float targetY;
        targetY = this.transform.position.y + 2f;

        while (this.transform.position.y != targetY)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, targetY, 0), speed * Time.deltaTime);
            yield return null;
        }
        restingTime = Mathf.Clamp(restingTime - 0.05f, 1f, Mathf.Infinity);
    }
}
