using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class skeletonctr : MonoBehaviour
{
    private Rigidbody2D rigid;

    private Animator ani;

    private float speed;

    public bool isattack;

    private int attacktime;

    public int maxattacktime;

    public int maxHealth = 100;//最大生命值

    public int currentHealth;//当前生命

    private int power;//力量值，直接线性影响攻击力

    public int health { get { return currentHealth; } }

    private bool isfinded;//发现主角

    private GameObject hero;

    public int walkTimer;

    public int maxwalkTimer;

    public bool walkdir;//true向右走，false向左走

    public bool wall;

    private bool isdead;

    public int deadtime;

    public int maxdeadtime;

    private float invincibleTime = 0.5f;//无敌时间

    private bool isinvincible;

    private float invicibleTimer;//无敌时间计时

    public Image bloodimg;

    private float ratio;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        speed = 10f;
        isattack = false;
        maxattacktime = 100;
        attacktime = 0;
        currentHealth = maxHealth;
        isfinded = false;
        walkTimer = 0;
        maxwalkTimer = 200;
        walkdir = true;
        isdead = false;
        deadtime = 0;
        maxdeadtime = 75;
        invicibleTimer = invincibleTime;
        ratio = 1;

    }

    // Update is called once per frame
    void Update()
    {
        changeHealthimg();
        hero = GameObject.Find("hero");
        if (isinvincible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
            {
                isinvincible = false;
            }
        }
        if (isdead)
        {
            deadtime++;
            if(deadtime>maxdeadtime)
            {
                ani.SetBool("isdead", true);
                if(deadtime>2*maxdeadtime)
                GameObject.Destroy(this.gameObject);//摧毁这个游戏对象
            }
        }    
        ani.SetBool("movex", walkdir);
        if(Mathf.Abs(hero.transform.position.x-this.transform.position.x+hero.transform.position.y-this.transform.position.y)<3)//满足条件，发现hero
        {
            isfinded = true;
        }
        else
        {
            isfinded = false;
        }
        if (currentHealth == 0)
        {
            return;
        }
        if (isattack)
        {
            attacktime++;

            if (attacktime > maxattacktime)
            {
                isattack = false;
                ani.SetBool("attack", false);
                attacktime = 0;
            }
            return;
        }
        //else中巡逻
        else
        {
            Vector2 trans = new Vector2(this.transform.position.x, this.transform.position.y);
            if(!isfinded)//没有发现主角时
            {
                if(walkdir&&!wall)
                {
                    trans.x += 0.1f;
                    walkTimer++;
                    if(walkTimer>maxwalkTimer)
                    {
                        walkTimer = 0;
                        walkdir = false;
                    }
                }
                else if(!walkdir&&!wall)
                {
                    trans.x -= 0.1f;
                    walkTimer++;
                    if (walkTimer > maxwalkTimer)
                    {
                        walkTimer = 0;
                        walkdir = true;
                    }
                }
                
            }
            else//发现主角后
            {
                if(Mathf.Abs(hero.transform.position.y-this.transform.position.y)<0.2)
                {
                    if (hero.transform.position.x < this.transform.position.x)//小于说明在左边
                    {
                        walkdir = false;
                        if(!wall)
                        trans.x -= 0.07f;
                    }
                    else
                    {
                        walkdir = true;
                        if(!wall)
                        trans.x += 0.07f;
                    }
                }
            }
            ani.SetBool("walk", true);
            ani.SetBool("movex", walkdir);
            rigid.MovePosition(trans);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.transform.tag.Equals("hero"))
        {
            isattack = true;
            if (hero.transform.position.x < this.transform.position.x)//小于说明在左边
            {
                walkdir = false;
            }
            else
            {
                walkdir = true;
            }
            ani.SetBool("movex", walkdir);
            ani.SetBool("attack", true);
            ani.SetBool("walk", false);
        }
        if (col.transform.tag.Equals("envirment"))
        {
            wall = true;
            walkdir = !walkdir;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.transform.tag.Equals("hero"))
        {
            isattack = true;
            if (hero.transform.position.x < this.transform.position.x)//小于说明在左边
            {
                walkdir = false;
            }
            else
            {
                walkdir = true;
            }
            ani.SetBool("movex", walkdir);
            ani.SetBool("attack", true);
            ani.SetBool("walk", false);
            heroctr ctr = col.GetComponent<heroctr>();
            ctr.healthChange(-10);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.transform.tag.Equals("envirment"))
        {
            wall = false;
            walkdir = !walkdir;
        }
    }
    public void healthChange(int amount)
    {
        if (currentHealth == 0)
        {
            return;
        }
        if (amount < 0)
        {
            if (isinvincible) return;
            isinvincible = true;
            invicibleTimer = invincibleTime;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);//限制生命值范围
        ratio = (float)currentHealth / ((float)maxHealth);
        if (currentHealth == 0)
        {
            ani.SetBool("dead", true);
            ani.SetBool("walk", false);
            ani.SetBool("attack", false);
            ani.SetBool("idle", false);
            isdead = true;
        }
    }
    private void changeHealthimg()
    {
        bloodimg.fillAmount = Mathf.Lerp(bloodimg.fillAmount, ratio, Time.deltaTime * 5);
    }
}
