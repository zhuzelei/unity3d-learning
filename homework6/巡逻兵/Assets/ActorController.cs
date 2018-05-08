using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class ActorController : MonoBehaviour {

    private Animator ani;
    private AnimatorStateInfo currentBaseState;
    private Rigidbody rig;

    private Vector3 velocity;
    private float rotateSpeed = 15f;
    private float runSpeed = 7f;//7f似乎游戏简单了很多
    

    // Use this for initialization
    void Start () {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!ani.GetBool("isLive")) return;
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        ani.SetFloat("Speed", Mathf.Max(Mathf.Abs(x), Mathf.Abs(z)));
        // 设置速度
        ani.speed = 1 + ani.GetFloat("Speed") / 3;
        // 调整跑步的时候的动画速度

        velocity = new Vector3(x, 0, z);

        
        if (x != 0 || z != 0)
        {
            Quaternion rotation = Quaternion.LookRotation(velocity);
            if (transform.rotation != rotation) transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotateSpeed);
        }

        this.transform.position += velocity * Time.fixedDeltaTime * runSpeed;
        // 主角移动
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Area"))
        {
            Debug.Log("into"+other.gameObject.name);//检测进入区域是否检测成功
            Publish publish = Publisher.getInstance();
            int patrolType = other.gameObject.name[other.gameObject.name.Length - 1] - '0';
            publish.notify(ActorState.ENTER_AREA, patrolType, this.gameObject);
            // 进入区域后，发布消息
        }
    }

    //触碰后死亡
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Patrol") && ani.GetBool("isLive"))
        {
            ani.SetBool("isLive", false);
            ani.SetTrigger("toDie");
            // 执行死亡动作
            Publish publish = Publisher.getInstance();
            publish.notify(ActorState.DEATH, 0, null);
            // 碰撞后，发布死亡信息
        }
    }
}
