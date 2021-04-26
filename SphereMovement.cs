using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    bool checkInput = true;
    public float speed = 5;
    public GameObject gameEngine;
    // Start is called before the first frame update
    void OnEnable()
    {
        checkInput = true;
        this.transform.position = new Vector3(0, 10f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkInput == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                checkInput = false;
                StartCoroutine(moveSphere(-2f, 0f));
            }else 
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                checkInput = false;
                StartCoroutine(moveSphere(2f, 0f));
            }else
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                checkInput = false;
                StartCoroutine(moveSphere(0f, 2f));
            }else
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                checkInput = false;
                StartCoroutine(moveSphere(0f, -2f));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        gameEngine.GetComponent<UIAndInitialization>().endGame(true);
    }
    IEnumerator moveSphere(float xMove, float yMove)
    {
        float targetX = Mathf.Clamp(this.transform.position.x + xMove, -8f, 8f);
        float targetY = Mathf.Clamp(this.transform.position.y + yMove, -10f, 12f);
        while (this.transform.position.x != targetX || this.transform.position.y != targetY)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(targetX, targetY, 0), speed * Time.deltaTime);
            yield return null;
        }
        checkInput = true;
    }
}
