using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{
    public GameObject parent,target;
    private bool leftTarget;
    private float speed=2, scale=0.7f;
    

    void Start()
    {
        this.gameObject.transform.localScale=new Vector3(leftTarget?scale:-scale,scale,scale);     
        StartTrip();   
        this.GetComponent<SpriteRenderer>().enabled=true;
    }
    public void Update()
    {
        this.gameObject.transform.localScale=new Vector3(leftTarget?scale:-scale,scale,scale);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(leftTarget ? -speed : speed, 0);
        this.transform.position=new Vector3(this.transform.position.x, leftTarget?-1.066f:-1.724f, -0.1f);
        if ((leftTarget && target.transform.position.x > this.transform.position.x) || (!leftTarget && target.transform.position.x < this.transform.position.x))
        {
            if (target.gameObject != parent.gameObject){
                if(target.GetComponent<Building>() != null){
                    target.GetComponent<Building>().task.RemoveResources();
                }
                target = parent;
                leftTarget = !leftTarget;
            }
            else{
                parent.GetComponent<Building>().task.AddResources(this.gameObject);
                Destroy(this.gameObject);
            }
        
        }
    }

    public void StartTrip()
    {
        leftTarget = target.transform.position.x < this.transform.position.x;        
        this.transform.position=new Vector3(this.transform.position.x, leftTarget?-1.066f:-1.724f, -0.1f);
    }

}

public static class SpawnCar{
    public static void Spwan(GameObject parent, GameObject target, Transform pos, int carId){
        GameObject car=new GameObject();
        car.AddComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        car.AddComponent<Car>().target=target;
        car.GetComponent<Car>().parent=parent;        
        car.AddComponent<SpriteRenderer>();
        car.GetComponent<SpriteRenderer>().enabled=false;
        car.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Tilemaps/auto"+carId);

        car.transform.position=new Vector3(pos.position.x,pos.position.y-1.757648f,-0.1f);
    }
}