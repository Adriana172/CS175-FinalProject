using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour
{
    public GameObject floorGameObject;
    private List<GameObject> tiles = new List<GameObject>();
    public float explodeSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < floorGameObject.transform.childCount; i++)
        {
            GameObject child = floorGameObject.transform.GetChild(i).gameObject;
            tiles.Add(child);
        }
    }
    public void collapseFloor()
    {
        foreach (var tile in tiles)
        {
            tile.GetComponent<Rigidbody>().isKinematic = false;
            tile.GetComponent<Rigidbody>().velocity = Random.onUnitSphere * explodeSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
