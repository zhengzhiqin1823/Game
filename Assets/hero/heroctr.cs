using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class heroctr : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigid;

    private Animator ani;

    private float speed;

    public bool isjump;

    public int jumptimer;

    public int maxjumptimer;

    public bool isattack;

    private int attacktime;

    public int maxattacktime;

    private bool down;

    public int maxHealth = 100;//最大生命值

    public int currentHealth;//当前生命

    private float invincibleTime = 2f;//无敌时间

    private bool isinvincible;

    private float invicibleTimer;//无敌时间计时

    private int power;//力量值，直接线性影响攻击力
    public int health { get { return currentHealth; } }

    private bool isdead;

    public int deadtime;

    public int maxdeadtime;

    public Image bloodimg;

    private float ratio;

    public float dir;

    public GameObject skillshader;

    public float skillcd;

    public float skilltimer;

    public float maxskilltimer;

    private bool isskill;

    private bool skill;//是否产生了剑气
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }
    void Start()
    {
        speed = 10f;
        isjump = false;
        jumptimer = 0;
        maxjumptimer = 50;
        isattack = false;
        maxattacktime = 30;
        attacktime = 0;
        down = false;
        maxHealth = 100;
        currentHealth = maxHealth;
        invicibleTimer = invincibleTime;
        power = 0;
        isdead = false;
        deadtime = 0;
        maxdeadtime = 75;
        ratio = 1;
        dir = 0;
        skillcd = 200;
        skilltimer = skillcd;
        isskill = false;
        maxskilltimer = 20;
        skill = false;
    }

    // Update is called once per frame
    void Update()
    {
        changeHealthimg();
        if (isdead)
        {
            deadtime++;//先播放死亡动画
            if (deadtime > maxdeadtime)//躺平
            {
                ani.SetBool("isdead", true);
                if (deadtime > 2 * maxdeadtime)
                {
                    ;//加载失败动画
                }  
            }
            return;
        }
        if (isinvincible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
            {
                isinvincible = false;
            }
        }
        ani.SetBool("idle", true);
        if (isskill)
        {
           skilltimer+=0.1f;
            if (skilltimer > maxskilltimer)
            {
                isskill = false;
                ani.SetBool("skill", false);
            }
            if((int)skilltimer==(int)(0.3f*maxskilltimer)&&skill)
            {
                skill = false;
                var shader = Instantiate(skillshader);
                Vector2 skilltrans = new Vector2(this.transform.position.x + 2 * (dir - 0.5f), this.transform.position.y);
                shader.transform.position = skilltrans;
            }
        }
        if (isattack)
        {
            attacktime++;
            if(attacktime>maxattacktime)
            {
                isattack = false;
                ani.SetBool("attack", false);
                attacktime = 10;
            }
        }
        if (Input.GetKey(KeyCode.J))
        {
            ani.SetBool("attack", true);
            ani.SetBool("idle", false);
            isattack = true;
            attacktime -= attacktime / 3;
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            if(skilltimer==skillcd)
            {
                isskill = true;
                skilltimer = 0;
                ani.SetBool("skill", true);
                skill = true;
            }
        }
        skilltimer = Mathf.Clamp(skilltimer + 0.1f, 0, skillcd);
        if (isjump)
        {
            ani.SetBool("run", false);
            jumptimer++;
            if(jumptimer>maxjumptimer)
            {
                isjump = false;
                ani.SetBool("jump", false);
                ani.SetBool("idle", false);
            }
            Vector2 trans = this.transform.position;
            trans.y += 0.15f;
            if (Input.GetKey(KeyCode.A))
            {
                trans.x -= 0.1f;
                dir = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                trans.x += 0.1f;
                dir = 1;
            }
            rigid.MovePosition(trans);
            return;
        }
        if(!down)
        {
            Vector2 trans = this.transform.position;
            trans.y -= 0.1f;
            if (Input.GetKey(KeyCode.A))
            {
                trans.x -= 0.1f;
                dir = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                trans.x += 0.1f;
                dir = 1;
            }
            rigid.MovePosition(trans);
            return;
        }
        if(Input.GetKey(KeyCode.A))
        {
            ani.SetBool("run", false);
            ani.SetBool("movex", false);
            ani.SetFloat("dir", 1f);
            dir = 0;
            if (!isjump && !isattack)
            {
                ani.SetBool("idle", false);
                ani.SetBool("run", true);
            }
              if(down)
            {
                Vector2 trans = new Vector2(this.transform.position.x - 0.1f, this.transform.position.y);
                rigid.MovePosition(trans);
            }
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            ani.SetBool("run", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir = 1;
            ani.SetBool("run", false);
            ani.SetBool("movex", true);
            ani.SetFloat("dir", 0f);
            if (!isjump && !isattack)
            {
                ani.SetBool("idle", false);
                ani.SetBool("run", true);
            }
               if(down)
            {
                Vector2 trans = new Vector2(this.transform.position.x + 0.1f, this.transform.position.y);
                rigid.MovePosition(trans);
            }
            
        }
        if(Input.GetKeyDown(KeyCode.Space)&&down)
        {
            ani.SetBool("jump", true);
            ani.SetBool("idle", false);
            jumptimer = 0;
            isjump = true;
            down = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if(col.transform.tag.Equals("envirment"))
        {
            down = true;
        }
        if(isattack&&col.transform.tag.Equals("monster"))
        {
                skeletonctr ctr = col.GetComponent<skeletonctr>();
                ctr.healthChange(-20 - power);
        }
        if (isattack && col.transform.tag.Equals("hammerske"))
        {
            hammerskectr ctr = col.GetComponent<hammerskectr>();
            ctr.healthChange(-20 - power);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.transform.tag.Equals("envirment"))
        {
            down = true;
        }
        if (isattack && col.transform.tag.Equals("monster"))
        {
            if (col.name.Equals("skeleton") )
            {
                skeletonctr ctr = col.GetComponent<skeletonctr>();
                ctr.healthChange(-20 - power);
            }
        }
        if (isattack && col.transform.tag.Equals("hammerske"))
        {
            hammerskectr ctr = col.GetComponent<hammerskectr>();
            ctr.healthChange(-20-power);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject col = collision.gameObject;
        if (col.transform.tag.Equals("envirment"))
        {
            down = false;
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
        if (currentHealth==0)
        {
            isdead = true;
            ani.SetBool("dead", true);
        }
    }
    private void changeHealthimg()
    {
          bloodimg.fillAmount = Mathf.Lerp(bloodimg.fillAmount, ratio, Time.deltaTime * 5);
    }
}
