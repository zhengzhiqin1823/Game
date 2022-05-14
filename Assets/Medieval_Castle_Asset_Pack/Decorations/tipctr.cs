using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tipctr : MonoBehaviour
{
    // Start is called before the first frame update

    public Canvas cans;
    void Start()
    {
        cans.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        cans.GetComponent<Canvas>().enabled = true;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        cans.GetComponent<Canvas> ().enabled = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        cans.GetComponent<Canvas>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cans.GetComponent<Canvas>().enabled = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        cans.GetComponent<Canvas>().enabled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        cans.GetComponent<Canvas>().enabled = false;
    }
}
