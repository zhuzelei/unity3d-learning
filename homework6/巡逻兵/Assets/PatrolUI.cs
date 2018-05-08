using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tem.Action;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class PatrolUI : SSActionManager, ISSActionCallback, Observer {

    public enum ActionState : int { IDLE, WALKLEFT, WALKFORWARD, WALKRIGHT, WALKBACK }
    private Animator ani;
    private SSAction currentAction;
    private ActionState currentState;
    // 保证当前只有一个动作
    private const float walkSpeed = 1f;
    private const float runSpeed = 3f;
    // 跑步和走路的速度
	// Use this for initialization
	new void Start () {
        ani = this.gameObject.GetComponent<Animator>();
        Publish publisher = Publisher.getInstance();
        publisher.add(this);
        // 添加事件
        currentState = ActionState.IDLE;
        idle();
        // 开始时，静止状态
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    public void SSEventAction(SSAction source, SSActionEventType events = SSActionEventType.COMPLETED, int intParam = 0, string strParam = null, Object objParam = null)
    {
        currentState = currentState > ActionState.WALKBACK ? ActionState.IDLE : (ActionState)((int)currentState + 1);
        // 改变当前状态
        switch (currentState)
        {
            case ActionState.WALKLEFT:
                walkLeft();
                break;
            case ActionState.WALKRIGHT:
                walkRight();
                break;
            case ActionState.WALKFORWARD:
                walkForward();
                break;
            case ActionState.WALKBACK:
                walkBack();
                break;
            default:
                idle();
                break;
        }
    }

    public void idle()
    {
        currentAction = IdleAction.GetIdleAction(Random.Range(1, 1.5f), ani);
        this.runAction(this.gameObject, currentAction, this);
    }

    public void walkLeft()
    {
        Vector3 target = Vector3.left * Random.Range(3, 5) + this.transform.position;
        currentAction = WalkAction.GetWalkAction(target, walkSpeed, ani);
        this.runAction(this.gameObject, currentAction, this);
    }
    public void walkRight()
    {
        Vector3 target = Vector3.right * Random.Range(3, 5) + this.transform.position;
        currentAction = WalkAction.GetWalkAction(target, walkSpeed, ani);
        this.runAction(this.gameObject, currentAction, this);
    }

    public void walkForward()
    {
        Vector3 target = Vector3.forward * Random.Range(3, 5) + this.transform.position;
        currentAction = WalkAction.GetWalkAction(target, walkSpeed, ani);
        this.runAction(this.gameObject, currentAction, this);
    }
    
    public void walkBack()
    {
        Vector3 target = Vector3.back * Random.Range(3, 5) + this.transform.position;
        currentAction = WalkAction.GetWalkAction(target, walkSpeed, ani);
        this.runAction(this.gameObject, currentAction, this);
    }
    
    public void turnNextDirection()
    {
        currentAction.destory = true;
        // 销毁当前动作
        switch (currentState)
        {
            case ActionState.WALKLEFT:
                currentState = ActionState.WALKRIGHT;
                walkRight();
                break;
            case ActionState.WALKRIGHT:
                currentState = ActionState.WALKLEFT;
                walkLeft();
                break;
            case ActionState.WALKFORWARD:
                currentState = ActionState.WALKBACK;
                walkBack();
                break;
            case ActionState.WALKBACK:
                currentState = ActionState.WALKFORWARD;
                walkForward();
                break;
        }
    }

    //一旦发现目标就立刻追踪
    public void getGoal(GameObject gameobject)
    {
        currentAction.destory = true;
        currentAction = RunAction.GetRunAction(gameobject.transform, runSpeed, ani);
        this.runAction(this.gameObject, currentAction, this);
    }
    //失去目标
    public void loseGoal()
    {
        currentAction.destory = true;
        idle();
    }

    public void stop()
    {
        currentAction.destory = true;
        currentAction = IdleAction.GetIdleAction(-1f, ani);
        this.runAction(this.gameObject, currentAction, this);
        // 永久站立
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject. name);
        Transform parent = collision.gameObject.transform.parent;
        if (parent != null && parent.CompareTag("Wall")) turnNextDirection();//撞墙
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Door")) turnNextDirection();//撞门
    }

    
    public void notified(ActorState state, int pos, GameObject actor)
    {
        if (state == ActorState.ENTER_AREA)
        {
            Debug.Log("CheckThis:" + (this.gameObject.name[this.gameObject.name.Length - 1] - '0'));
            if (pos == this.gameObject.name[this.gameObject.name.Length - 1] - '0')
                getGoal(actor);
            else loseGoal();
        }
        else stop();//死亡之后不再走动
    }
}
