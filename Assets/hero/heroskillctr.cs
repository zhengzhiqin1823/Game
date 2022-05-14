using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroskillctr : MonoBehaviour
{
    // Start is called before the first frame update
    public heroctr ctr;

    public int magic;

    private Animator ani;

    private float dir;//技能方向

    public GameObject hero;

    private float skilltimer;

    public float skillshowtimer;

    private float maxskilltimer;
    private void Awake()
    {
        hero = GameObject.Find("hero");
        ctr = hero.GetComponent<heroctr>();
        dir = ctr.dir;
        ani = GetComponent<Animator>();
        ani.SetFloat("Blend", dir);
        skilltimer = 0;
        maxskilltimer = 300;
        skillshowtimer = 75;
    }

    // Update is called once per frame
    void Update()
    {
        skilltimer += 0.2f;
        if(skillshowtimer<skilltimer)
        {
            ani.SetBool("skill", true);
        }
        Vector2 trans = new Vector2(this.transform.position.x + (dir - 0.5f) * 0.1f, this.transform.position.y);
        this.transform.position = trans;
        if(skilltimer>maxskilltimer)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.transform.tag.Equals("monster"))
        {
            skeletonctr ctr = col.GetComponent<skeletonctr>();
            ctr.healthChange(-20);
        }
        if (col.transform.tag.Equals("hammerske"))
        {
            hammerskectr ctr = col.GetComponent<hammerskectr>();
            ctr.healthChange(-20);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.transform.tag.Equals("monster"))
        {
            skeletonctr ctr = col.GetComponent<skeletonctr>();
            ctr.healthChange(-20);
        }
        if (col.transform.tag.Equals("hammerske"))
        {
            hammerskectr ctr = col.GetComponent<hammerskectr>();
            ctr.healthChange(-20);
        }
    }
}
