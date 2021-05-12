using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fracture : MonoBehaviour
{
    public GameObject fractured;
    // Update is called once per frame
    Rigidbody m_Rigidbody;
    private float m_Thrust = 20000f;

    public void breakPlayer()
    {
        Instantiate(fractured, transform.position, transform.rotation);
        GetComponent<Rigidbody>().AddExplosionForce(m_Thrust, transform.position, 5);
        Destroy(gameObject);

    }

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Instantiate(fractured, transform.position, transform.rotation);
        //     GetComponent<Rigidbody>().AddExplosionForce(500000, transform.position, 5);
        //     m_Rigidbody.AddForce(0, 0, m_Thrust, ForceMode.Impulse);
        //     Destroy(gameObject);
        // }
    }
}
