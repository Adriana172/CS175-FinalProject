using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Rolling : MonoBehaviour
{
    public GameObject startingPos;
    public GameObject canvasObject;
    public bool isRolling;
    public float rotateSpeed;
    public GameObject fractured;
    public AudioSource clickSound;
    GameOverController gameOverScript;


    private bool isVertical;
    private int orientation;
    private int focalQuadrant = 1; // set to default front quadrant
    private Bounds bound;
    private Vector3 left, right, up, down;
    private GameObject floorObject, playerObject, smokeObject, focalPoint;
    private string success_name = "LavaCube";
    private List<Collider> colliders = new List<Collider>();
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        // set the player start position at the Default Cube Position
        startingPos = GameObject.Find("Floor/DefaultCube");
        floorObject = GameObject.Find("Floor");
        playerObject = GameObject.Find("Player");
        smokeObject = GameObject.Find("Smoke");
        canvasObject = GameObject.Find("Canvas");
        focalPoint = GameObject.Find("Focal Point");
        gameOverScript = canvasObject.GetComponent<GameOverController>();
        Vector3 newPos = new Vector3(startingPos.transform.position.x, startingPos.transform.position.y + 1.1f, startingPos.transform.position.z);
        transform.position = newPos;
        print("START \n");
        isVertical = true;
        bound = GetComponent<BoxCollider>().bounds;
        up = new Vector3(x: -bound.size.x / 2, y: -bound.size.y / 2, z: 0);
        down = new Vector3(x: bound.size.x / 2, y: -bound.size.y / 2, z: 0);
        right = new Vector3(x: 0, y: -bound.size.y / 2, z: bound.size.z / 2);
        left = new Vector3(x: 0, y: -bound.size.y / 2, z: -bound.size.z / 2);
        Debug.Log("RIGHT: " + right.x + "," + right.y + "," + right.z);
        orientation = 0;
        clickSound = GetComponent<AudioSource>();
    }

    /*  private void OnCollisionEnter(Collision collision)
      {
          clickSound.Play();
      }*/
    void OnTriggerEnter(Collider col)
    {
        if (!colliders.Contains(col))
        {
            colliders.Add(col);
        }
        if (gameOverScript.gameOver)
        {
            return;
        }
        if (!col.gameObject.GetComponent<MeshRenderer>().enabled)
        {
            fracture fractureScript = playerObject.GetComponent<fracture>();
            floor floorScript = floorObject.GetComponent<floor>();
            playerObject.GetComponent<Rigidbody>().isKinematic = false;
            playerObject.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * floorScript.explodeSpeed * 10000;
            fractureScript.breakPlayer();


            this.GetComponent<Rigidbody>().isKinematic = false;
            floorScript.collapseFloor();

            //this.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * floorScript.explodeSpeed;
            print("GAME OVER\n");
            gameOverScript.gameOver = true;
            smokeObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        colliders.Remove(col);
    }
    /// <summary>
    /// GIven the camera quaternion, determines the actual quadrant in camera view
    /// </summary>
    void computeQuadrant()
    {
        float angleY = focalPoint.transform.rotation.eulerAngles.y;
        if (angleY >= 23.76 && angleY <= 111)
        {
            // left quadrant 
            focalQuadrant = 2;
        }
        else if (angleY > 111 && angleY <= 209)
        {
            // back quadrant 
            focalQuadrant = 3;
        }
        else if (angleY > 209 && angleY <= 287)
        {
            // right quadrant
            focalQuadrant = 4;
        }
        else
        {
            // front quadrant
            focalQuadrant = 1;
        }
    }
    /// <summary>
    /// Given a quadrant, maps the keyboard input direction
    /// to the direction in the camera frame
    /// </summary>
    /// <param name="initialdirection"></param>
    /// <param name="focalQuadrant"></param>
    int computeDirections(int initialdirection, int focalQuadrant)
    {
        int retval = initialdirection;
        if (focalQuadrant == 1)
        {
            return retval;
        }
        // left quadrant
        else if (focalQuadrant == 2)
        {
            switch (initialdirection)
            {
                case 1:
                    retval = 3;
                    break;
                case 2:
                    retval = 4;
                    break;
                case 3:
                    retval = 2;
                    break;
                case 4:
                    retval = 1;
                    break;
            }
        }
        // back quadrant 
        else if (focalQuadrant == 3)
        {
            switch (initialdirection)
            {
                case 1:
                    retval = 2;
                    break;
                case 2:
                    retval = 1;
                    break;
                case 3:
                    retval = 4;
                    break;
                case 4:
                    retval = 3;
                    break;
            }

        }
        // right quadrant
        else
        {
            switch (initialdirection)
            {
                case 1:
                    retval = 4;
                    break;
                case 2:
                    retval = 3;
                    break;
                case 3:
                    retval = 1;
                    break;
                case 4:
                    retval = 2;
                    break;
            }
        }
        return retval;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isRolling && isVertical && colliders.Count == 1 && colliders[0].gameObject.name == success_name)
        {
            gameOverScript.gameOver = true;
            colliders[0].gameObject.SetActive(false);
            smokeObject.SetActive(false);
            playerObject.GetComponent<Rigidbody>().isKinematic = false;
            gameOverScript.win = true;
            return;
        }

        if (gameOverScript.gameOver)
        {
            return;
        }

        int directionMovement = -1;
        computeQuadrant();
        if (Input.GetKey(KeyCode.LeftArrow) && !isRolling && !Input.GetKey(KeyCode.LeftControl))
        {
            directionMovement = computeDirections(1, focalQuadrant);
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !isRolling && !Input.GetKey(KeyCode.LeftControl))
        {
            directionMovement = computeDirections(2, focalQuadrant);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && !isRolling)
        {
            directionMovement = computeDirections(3, focalQuadrant);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && !isRolling)
        {
            directionMovement = computeDirections(4, focalQuadrant);
        }

        if (directionMovement == 1)
        {
            Debug.Log("DIRECTION : LEFT ");
            if (!isVertical && orientation == 2)
            {
                left.y += 0.5f;
            }
            else if (!isVertical)
            {
                left.y += 0.5f;
                left.z -= 0.5f;
                isVertical = !isVertical;
            }
            else
            {
                isVertical = !isVertical;
            }

            StartCoroutine(routine: Roll(left));
            counter++;
            // restore values
            if (isVertical && orientation != 2)
            {
                left.y -= 0.5f;
                left.z += 0.5f;
            }
            else if (orientation == 2)
            {
                left.y -= 0.5f;
            }
            if (orientation == 0)
            {
                orientation = 1;
            }
            else if (orientation == 1)
            {
                orientation = 0;
            }
        }
        else if (directionMovement == 2)
        {
            Debug.Log("DIRECTION : RIGHT ");

            if (!isVertical && orientation == 2)
            {
                right.y += 0.5f;
            }
            else if (!isVertical)
            {
                right.y += 0.5f;
                right.z += 0.5f;
                isVertical = !isVertical;
            }
            else
            {
                isVertical = !isVertical;
            }
            StartCoroutine(routine: Roll(right));
            if (isVertical && orientation != 2)
            {
                right.y -= 0.5f;
                right.z -= 0.5f;
            }
            else if (orientation == 2)
            {
                right.y -= 0.5f;
            }
            if (orientation == 0)
            {
                orientation = 1;
            }
            else if (orientation == 1)
            {
                orientation = 0;
            }
            counter++;
        }
        else if (directionMovement == 3)
        {
            Debug.Log("DIRECTION : UP");
            if (!isVertical && orientation == 1)
            {
                up.y += 0.5f;
            }
            else if (!isVertical)
            {
                up.y += 0.5f;
                up.x -= 0.5f;
                isVertical = !isVertical;
            }
            else
            {
                isVertical = !isVertical;
            }
            StartCoroutine(routine: Roll(up));
            if (isVertical && orientation != 1)
            {
                up.y -= 0.5f;
                up.x += 0.5f;
            }
            else if (orientation == 1)
            {
                up.y -= 0.5f;
            }

            if (orientation == 0)
            {
                orientation = 2;
            }
            else if (orientation == 2)
            {
                orientation = 0;
            }
            counter++;

        }
        else if (directionMovement == 4)
        {
            Debug.Log("DIRECTION : DOWN");
            if (!isVertical && orientation == 1)
            {
                down.y += 0.5f;
            }
            else if (!isVertical)
            {
                down.y += 0.5f;
                down.x += 0.5f;
                isVertical = !isVertical;
            }
            else
            {
                isVertical = !isVertical;
            }

            StartCoroutine(routine: Roll(down));
            if (isVertical && orientation != 1)
            {
                down.y -= 0.5f;
                down.x -= 0.5f;
            }
            else if (orientation == 1)
            {
                down.y -= 0.5f;
            }
            if (orientation == 0)
            {
                orientation = 2;
            }
            else if (orientation == 2)
            {
                orientation = 0;
            }
            counter++;
        }
        /*if (playsound)
        {
            clickSound.Play();
        }*/
    }

    IEnumerator Roll(Vector3 positionToRotation)
    {
        isRolling = true;
        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(lhs: Vector3.up, rhs: positionToRotation).normalized;

        while (angle < 90f)
        {
            float angleSpeed = Time.deltaTime + rotateSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }
        clickSound.Play();
        transform.RotateAround(point, axis, angle: 90 - angle);
        isRolling = false;

    }
    void OnGUI()
    {
        GUI.contentColor = Color.red;
        GUI.Label(new Rect(0, 0, 100, 50), "Moves " + counter);
    }


}
