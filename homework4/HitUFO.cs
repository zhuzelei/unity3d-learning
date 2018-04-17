using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.HitGame;

namespace Com.HitGame
{
    public class myFactory : System.Object
    {
        //values
        public int Round = 0; // 
        public int Isbegin = 0; // 0 == not begin
        public int Score = 0;
        public int usefri = 0;
        public int GameState = 1;// 1-ready 2-playing 3-over
        public int LoseNum = 0;

        //firsbees
        public List<GameObject> fris = new List<GameObject>();


        private static myFactory _instance;
        public static myFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new myFactory();
            }
            return _instance;
        }

        public void launchAfrisbee(float g, float delaytime, Vector3 speed, Color color,
            Vector3 size, Vector3 position)
        {
            if (fris.Count == 0)
            {
                GameObject fri = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                waittolanch wt = fri.AddComponent<waittolanch>();
                wt.setting(g, fri, fris, delaytime, speed, color, size, position);
            }
            else
            {
                GameObject fri = fris[0];
                fris.RemoveAt(0);
                //这里非常重要，用了get而不是add
                waittolanch wt = fri.GetComponent<waittolanch>();
                wt.setting(g, fri, fris, delaytime, speed, color, size, position);
            }
        }

        public class waittolanch : MonoBehaviour
        {
            public List<GameObject> fris;
            public GameObject fri;
            public Camera cm;
            public float delaytime;
            public Vector3 speed;
            public float g;
            public Color color;
            public Vector3 size;
            public Vector3 position;
            public int state = 0;


            public void setting(float g, GameObject fri, List<GameObject> fris, float delaytime,
                Vector3 speed, Color color, Vector3 size, Vector3 position)
            {
                this.fri = fri;
                this.fris = fris;
                this.delaytime = delaytime;
                this.speed = speed;
                this.g = g;
                this.color = color;
                this.size = size;
                this.position = position;
                StartCoroutine(launch());
            }

            public IEnumerator launch()
            {
                //wait delaytime
                yield return new WaitForSeconds(delaytime);
                //lauch a fri
                this.transform.position = position;
                this.transform.localScale = size;
                this.GetComponent<Renderer>().material.color = color;
                state = 1;
            }

            void Update()
            {
                if (state == 1)
                {
                    speed = new Vector3(speed.x, speed.y - g, speed.z);
                    this.transform.position = this.transform.position + speed;
                    if (this.transform.position.y <= 0f)
                    {
                        
                        myFactory.GetInstance().LoseNum++;
                        this.transform.position = new Vector3(0f, 0f, -2f);
                        state = 0;
                        // when download
                        fris.Add(fri);
                        myFactory.GetInstance().usefri++;

                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mp = Input.mousePosition;
                    cm = Camera.main;
                    Ray ray = cm.ScreenPointToRay(mp);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject == this.gameObject)
                        {
                            
                            this.transform.position = new Vector3(0f, 0f, -2f);
                            state = 0;
                            // when download
                            fris.Add(fri);
                            if (myFactory.GetInstance().Round == 1)
                            {
                                myFactory.GetInstance().Score++;
                                myFactory.GetInstance().usefri++;
                            }
                            else if(myFactory.GetInstance().Round == 2)
                            {
                                myFactory.GetInstance().Score += 2;
                                myFactory.GetInstance().usefri++;
                            }
                            else if(myFactory.GetInstance().Round == 3)
                            {
                                myFactory.GetInstance().Score += 3;
                                myFactory.GetInstance().usefri++;
                            }
                                
                        }
                    }
                }
            }

        }
    }
}

public class HitUFO : MonoBehaviour
{
    public Vector3 camerapos;
    public Quaternion cameraqua;
    public Camera cm;
    // color
    public Color whiteColor;
    public Color blueColor;
    public Color RedColor;
    //Size
    public Vector3 whiteSize;
    public Vector3 blueSize;
    public Vector3 redSize;
    //speed
    public Vector3 whiteSpeed;
    public Vector3 blueSpeed;
    public Vector3 redSpeed;
    //a little distance use to make some shift
    public Vector3 Move;
    //a g use to change v
    public float g;
    //position
    public Vector3 leftposition;
    public Vector3 rightposition;
    // Use this for initialization
    void Start()
    {
        //make a plane and make camra in the right position
        camerapos = new Vector3(0f, 0f, 0f);
        cameraqua = Quaternion.Euler(340f, 0f, 0f);
        cm = Camera.main;
        cm.transform.position = camerapos;
        cm.transform.rotation = cameraqua;
        //初始化

        // color
        whiteColor = new Color(0.9f, 0.9f, 0.9f);
        blueColor = new Color(0f, 0f, 0.7f);
        RedColor = new Color(0.7f, 0f, 0f);
        //Size
        whiteSize = new Vector3(1.2f, 0.5f, 1.2f);
        blueSize = new Vector3(1f, 0.2f, 1f);
        redSize = new Vector3(1f, 0.2f, 1f);
        //speed
        whiteSpeed = new Vector3(0f, 0.1f, 0.25f);
        blueSpeed = new Vector3(0f, 0.1f, 0.25f);
        redSpeed = new Vector3(0f, 0.1f, 0.4f);
        //a little distance use to make some shift
        Move = new Vector3(0.05f, 0f, 0f);
        //a g use to change v
        g = 0.001f;
        //position
        leftposition = new Vector3(-1f, 0f, 0f);
        rightposition = new Vector3(1f, 0f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if(myFactory.GetInstance().LoseNum >= 3 )
        {
            myFactory.GetInstance().GameState = 3;
        }

        if (myFactory.GetInstance().Round == 1 && myFactory.GetInstance().Isbegin == 0 && myFactory.GetInstance().GameState != 3)
        {
            //Isbegin = 1；
            myFactory.GetInstance().Isbegin = 1;
            myFactory.GetInstance().GameState = 2;
            // function round1
            Invoke("Round1", 0f);
        }

        if (myFactory.GetInstance().Round == 1 && myFactory.GetInstance().usefri == 6 && myFactory.GetInstance().GameState == 2 )
        {
            myFactory.GetInstance().Round = 2;
            myFactory.GetInstance().usefri = 0;
            Invoke("Round2", 1f);
        }

        if (myFactory.GetInstance().Round == 2 && myFactory.GetInstance().usefri == 6 && myFactory.GetInstance().GameState == 2)
        {
            myFactory.GetInstance().Round = 3;
            myFactory.GetInstance().usefri = 0;
            Invoke("Round3", 1f);
        }

        
    }

    void Round1()
    {
        for (int i = 1; i <= 6; i++)
        {
            float time = i;
            float Randkey = Random.Range(-4f, 4f);
            if (i % 2 == 0)
            {
                myFactory.GetInstance().launchAfrisbee(g, time * 2, whiteSpeed + Move * Randkey, whiteColor, whiteSize, leftposition);
                
            }
            else
            {
                myFactory.GetInstance().launchAfrisbee(g, time * 2, whiteSpeed - Move * Randkey, whiteColor, whiteSize, rightposition);
                
            }
        }
    }

    void Round2()
    {
        for (int i = 1; i <= 6; i++)
        {
            float time = i;
            float Randkey = Random.Range(-4f, 4f);
            if (i % 2 == 0)
            {
                myFactory.GetInstance().launchAfrisbee(g, time * 2, blueSpeed + Move * Randkey, blueColor, blueSize, leftposition);
                
            }
            else
            {
                myFactory.GetInstance().launchAfrisbee(g, time * 2, blueSpeed - Move * Randkey, blueColor, blueSize, rightposition);
                
            }
        }
    }

    void Round3()
    {
        for (int i = 1; i <= 6; i++)
        {
            float time = i;
            float Randkey = Random.Range(-4f, 4f);
            if (i % 2 == 0)
            {
                myFactory.GetInstance().launchAfrisbee(g, time * 2, redSpeed + Move * Randkey, RedColor, redSize, leftposition);
                
            }
            else
            {
                myFactory.GetInstance().launchAfrisbee(g, time * 2, redSpeed + Move * Randkey, RedColor, redSize, rightposition);
                
            }
        }
        
    }

    

}