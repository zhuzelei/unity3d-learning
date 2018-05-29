using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCircle : MonoBehaviour {

    private ParticleSystem particleSystem;  
    private ParticleSystem.Particle[] particleArr;  
    private CirclePosition[] circle; 

    public float minRadius = 5.0f;  
    public float maxRadius = 12.0f;    
    public float speed = 2f;         
    public float distance = 0.02f;  
    public int count = 10000;       
    public float size = 0.03f;
    private int tier = 10;

    // Use this for initialization
    void Start () { 
        particleArr = new ParticleSystem.Particle[count];
        circle = new CirclePosition[count];

        particleSystem = this.GetComponent<ParticleSystem>();
        particleSystem.startSpeed = 0;             
        particleSystem.startSize = size;         
        particleSystem.loop = false;
        particleSystem.maxParticles = count;      
        particleSystem.Emit(count);               
        particleSystem.GetParticles(particleArr);
        RandomlySpread();  
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < count; i++)
        { 
            circle[i].angle -= (i % tier + 1) * (speed / circle[i].radius / tier); 
            circle[i].angle = (360.0f + circle[i].angle) % 360.0f;
            float theta = circle[i].angle / 180 * Mathf.PI;
            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
        }

        particleSystem.SetParticles(particleArr, particleArr.Length);
    }

    void RandomlySpread()
    {
        for (int i = 0; i < count; ++i)
        {     
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);
  
            float angle = Random.Range(0.0f, 360.0f);
            float theta = angle / 180 * Mathf.PI;
 
            float time = Random.Range(0.0f, 360.0f);
            circle[i] = new CirclePosition(radius, angle, time);
            particleArr[i].position = new Vector3(circle[i].radius * Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
        }
        particleSystem.SetParticles(particleArr, particleArr.Length);
    }

}

public class CirclePosition
{
    public float radius = 0f, angle = 0f, time = 0f;
    public CirclePosition(float radius, float angle, float time)
    {
        this.radius = radius;   
        this.angle = angle;     
        this.time = time;        
    }
}