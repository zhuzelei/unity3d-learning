using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Com.Mygame;

namespace Com.Mygame
{
   
    public interface UserActions
    {
        void priestSOnB();
        void priestEOnB();
        void devilSOnB();
        void devilEOnB();
        void moveBoat();
        void offBoatL();
        void offBoatR();
        void restart();
    }
   
    public interface QueryGameStatus
    {
        bool isMoving();
        void setMoving(bool state);
        string getMessage();
        void setMessage(string message);
    }
   
    public class GameSceneController : System.Object, UserActions, QueryGameStatus
    {
        private static GameSceneController _instance;
        private BaseCode _base_code;
        private GenGameObject _gen_game_obj;
        private bool moving = false;
        private string message = "";

        public static GameSceneController GetInstance()
        {
            if (null == _instance) _instance = new GameSceneController();
            return _instance;
        }

        public BaseCode getBaseCode() { return _base_code; }
        internal void setBaseCode(BaseCode bc) { if (null == _base_code) _base_code = bc; }

        public GenGameObject getGenGameObject() { return _gen_game_obj; }
        internal void setGenGameObject(GenGameObject ggo) { if (null == _gen_game_obj) _gen_game_obj = ggo; }

        public bool isMoving() { return moving; }
        public void setMoving(bool state) { this.moving = state; }
        public string getMessage() { return message; }
        public void setMessage(string message) { this.message = message; }

        public void priestSOnB() { _gen_game_obj.priestStartOnBoat(); }
        public void priestEOnB() { _gen_game_obj.priestEndOnBoat(); }
        public void devilSOnB() { _gen_game_obj.devilStartOnBoat(); }
        public void devilEOnB() { _gen_game_obj.devilEndOnBoat(); }
        public void moveBoat() { _gen_game_obj.moveBoat(); }
        public void offBoatL() { _gen_game_obj.getOffTheBoat(0); }
        public void offBoatR() { _gen_game_obj.getOffTheBoat(1); }

        public void restart()
        {
            moving = false;
            message = "";
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    public interface ISSActionCallback
    {
        void OnActionCompleted(SSAction action);
    }
   
    public class SSActionManager : System.Object
    {
        private static SSActionManager _instance;
        public static SSActionManager GetInstance()
        {
            if (_instance == null) _instance = new SSActionManager();
            return _instance;
        }
        
        public SSAction ApplyCCMoveToAction(GameObject obj, Vector3 target, float speed, ISSActionCallback completed)
        {
            CCMoveToAction ac = obj.AddComponent<CCMoveToAction>();
            ac.RunAction(target, speed, completed);
            return ac;
        }
        public SSAction ApplyCCMoveToAction(GameObject obj, Vector3 target, float speed)
        {
            return ApplyCCMoveToAction(obj, target, speed, null);
        }

        public SSAction ApplyCCMoveToYZAction(GameObject obj, Vector3 target, float speed, ISSActionCallback completed)
        {
            CCMoveToYZAction ac = obj.AddComponent<CCMoveToYZAction>();
            ac.RunAction(obj, target, speed, completed);
            return ac;
        }
        public SSAction ApplyCCMoveToYZAction(GameObject obj, Vector3 target, float speed)
        {
            return ApplyCCMoveToYZAction(obj, target, speed, null);
        }
    }

    public class SSAction : MonoBehaviour
    {
        public void Free() { Destroy(this); }
    }

    public class CCMoveToAction : SSAction
    {
        public Vector3 target;
        public float speed;

        private ISSActionCallback monitor = null;

        public void RunAction(Vector3 target, float speed, ISSActionCallback monitor)
        {
            this.target = target;
            this.speed = speed;
            this.monitor = monitor;
            GameSceneController.GetInstance().setMoving(true);
        }

        void Update()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if (transform.position == target)
            {
                GameSceneController.GetInstance().setMoving(false);
                if (monitor != null) monitor.OnActionCompleted(this);
                Destroy(this);
            }
        }
    }

    public class CCMoveToYZAction : SSAction, ISSActionCallback
    {
        public GameObject obj;
        public Vector3 target;
        public float speed;

        private ISSActionCallback monitor = null;

        public void RunAction(GameObject obj, Vector3 target, float speed, ISSActionCallback monitor)
        {
            this.obj = obj;
            this.target = target;
            this.speed = speed;
            this.monitor = monitor;
            GameSceneController.GetInstance().setMoving(true);

            if (target.y < obj.transform.position.y)
            {
                Vector3 targetZ = new Vector3(target.x, obj.transform.position.y, target.z);
                SSActionManager.GetInstance().ApplyCCMoveToAction(obj, targetZ, speed, this);
            }
            else
            {
                Vector3 targetY = new Vector3(target.x, target.y, obj.transform.position.z);
                SSActionManager.GetInstance().ApplyCCMoveToAction(obj, targetY, speed, this);
            }
        }

        public void OnActionCompleted(SSAction action)
        {
            SSActionManager.GetInstance().ApplyCCMoveToAction(obj, target, speed, null);
        }

        void Update()
        {
            if (obj.transform.position == target)
            {
                GameSceneController.GetInstance().setMoving(false);
                if (monitor != null) monitor.OnActionCompleted(this);
                Destroy(this);
            }
        }
    }
}

public class BaseCode : MonoBehaviour
{
    

    void Start()
    {
        GameSceneController my = GameSceneController.GetInstance();
        my.setBaseCode(this);

    }
}
