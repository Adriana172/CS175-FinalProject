using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;
    //public Vector3 endPos;
    public GameObject startingPos;

    // Start is called before the first frame update
    void Start()
    {
        // set the player start position at the Default Cube Position
        startingPos = GameObject.Find("Floor/DefaultCube");
        Vector3 newPos = new Vector3(startingPos.transform.position.x, startingPos.transform.position.y + 1.1f, startingPos.transform.position.z);
        transform.position = newPos;
        playerRb = GetComponent<Rigidbody>();
        /*endPos = new Vector3(newPos.x, newPos.y - 0.5f,  newPos.z - 1.5f);*/
    }

    public IEnumerator Interpolate(Vector3 endPos, Quaternion endROTATION)
    {
        float totalTime =0.18f; // or whatever

        float startTime = Time.time;
        float endTime = startTime + totalTime;

        var startPos = transform.position;
        var startROTATION = transform.rotation;

        while (Time.time < endTime)
        {
            float timeSoFar = Time.time - startTime;
            float fractionTime = timeSoFar / totalTime;

            transform.position =
              Vector3.Lerp(startPos, endPos, fractionTime);

            transform.rotation =
              Quaternion.Slerp(startROTATION, endROTATION, fractionTime);
            yield return null;
        }
        transform.position = endPos;

        transform.rotation = endROTATION;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            print("Left Arrow \n");
            var endROTATION = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
            var startPos = transform.position;
            var endPos = new Vector3(startPos.x, startPos.y - 0.5f, startPos.z - 1.5f);
            StartCoroutine(Interpolate(endPos, endROTATION));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            print("Right Arrow \n");
            var endROTATION = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            var startPos = transform.position;
            var endPos = new Vector3(startPos.x, startPos.y - 0.5f, startPos.z + 1.5f);
            StartCoroutine(Interpolate(endPos, endROTATION));

        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            print("Down Arrow \n");
            var endROTATION = Quaternion.Euler(0.0f, 0.0f, -90.0f);
            var startPos = transform.position;
            var endPos = new Vector3(startPos.x + 1.5f, startPos.y - 0.5f, startPos.z);
            StartCoroutine(Interpolate(endPos, endROTATION));
        }

        else if (Input.GetKey(KeyCode.UpArrow))
        {
            print("Up Arrow \n");
            var endROTATION = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            var startPos = transform.position;
            var endPos = new Vector3(startPos.x - 1.5f, startPos.y - 0.5f, startPos.z);
            StartCoroutine(Interpolate(endPos, endROTATION));
        }
        
    }

    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
   /* void FixedUpdate()
    {
        if (_isLerping)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                _isLerping = false;
            }
        }
    }*/
}
