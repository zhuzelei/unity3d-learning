using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.MyGame
{

    public class SSAction : ScriptableObject
    {

        public bool enable = true;
        public bool destroy = false;

        public GameObject gameobject;
        public Transform transform;
        public ISSActionCallback callback;

        protected SSAction() { }

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }
    }

    public class CCMoveToAction : SSAction                        //移动
    {
        public Vector3 target;
        public float speed;

        private CCMoveToAction() { }
        public static CCMoveToAction GetSSAction(Vector3 target, float speed)
        {
            CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
            action.target = target;
            action.speed = speed;
            return action;
        }

        public override void Update()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                this.destroy = true;
                this.callback.SSActionEvent(this);
            }
        }

        public override void Start()
        {

        }
    }

    public class CCSequenceAction : SSAction, ISSActionCallback
    {
        public List<SSAction> sequence;
        public int repeat = -1;
        public int start = 0;

        public static CCSequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence)
        {
            CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
            action.repeat = repeat;
            action.sequence = sequence;
            action.start = start;
            return action;
        }

        public override void Update()
        {
            if (sequence.Count == 0) return;
            if (start < sequence.Count)
            {
                sequence[start].Update();
            }
        }

        public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0, string strParam = null, Object objectParam = null)
        {
            source.destroy = false;          //先保留这个动作，如果是无限循环动作组合之后还需要使用
            this.start++;
            if (this.start >= sequence.Count)
            {
                this.start = 0;
                if (repeat > 0) repeat--;
                if (repeat == 0)
                {
                    this.destroy = true;
                    this.callback.SSActionEvent(this);
                }
            }
        }

        public override void Start()
        {
            foreach (SSAction action in sequence)
            {
                action.gameobject = this.gameobject;
                action.transform = this.transform;
                action.callback = this;
                action.Start();
            }
        }

        void OnDestroy()
        {

        }
    }

    public enum SSActionEventType : int { Started, Competeted }

    public interface ISSActionCallback
    {
        void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0, string strParam = null, Object objectParam = null);
    }

    public class SSActionManager : MonoBehaviour, ISSActionCallback                      //action管理器
    {

        private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();    //将执行的动作的字典集合,int为key，SSAction为value
        private List<SSAction> waitingAdd = new List<SSAction>();                       //等待去执行的动作列表
        private List<int> waitingDelete = new List<int>();                              //等待删除的动作的key                

        protected void Update()
        {
            foreach (SSAction ac in waitingAdd)
            {
                actions[ac.GetInstanceID()] = ac;                                      //获取动作实例的ID作为key
            }
            waitingAdd.Clear();

            foreach (KeyValuePair<int, SSAction> kv in actions)
            {
                SSAction ac = kv.Value;
                if (ac.destroy)
                {
                    waitingDelete.Add(ac.GetInstanceID());
                }
                else if (ac.enable)
                {
                    ac.Update();
                }
            }

            foreach (int key in waitingDelete)
            {
                SSAction ac = actions[key];
                actions.Remove(key);
                DestroyObject(ac);
            }
            waitingDelete.Clear();
        }

        public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
        {
            action.gameobject = gameobject;
            action.transform = gameobject.transform;
            action.callback = manager;
            waitingAdd.Add(action);
            action.Start();
        }

        public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0, string strParam = null, Object objectParam = null)
        {

        }
    }
}