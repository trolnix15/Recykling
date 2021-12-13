using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject max;
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
    }
    void Update(){
        Movement.move(max.transform.position.x, cam);
    }
}
static class Movement{
    private static float halfHeight, halfWidth;
    public static void move(float max, Camera cam){
        float speed=3f;
        float slowingSpeed=6f;
        float h = Input.GetAxisRaw("Horizontal");
        if(h==0 && (cam.GetComponent<Rigidbody2D>().velocity.x > 0.1 || cam.GetComponent<Rigidbody2D>().velocity.x < -0.1)){            
            float aSpeed=cam.GetComponent<Rigidbody2D>().velocity.x; 
            aSpeed += aSpeed>0? -Time.deltaTime*slowingSpeed:Time.deltaTime*slowingSpeed;
            cam.GetComponent<Rigidbody2D>().velocity = new Vector2 (aSpeed,0);
        }
        else{            
            cam.GetComponent<Rigidbody2D>().velocity = new Vector2 (h * speed,0);
        }
        checkPos(max,cam);
    }

    private static void checkPos(float max, Camera cam){
        halfHeight = cam.orthographicSize;
        halfWidth = cam.aspect * halfHeight;
        if(cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x<1){
            cam.gameObject.transform.position=new Vector3(halfWidth+1,0,-1.699f);
        }

        if(cam.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x>max+0.5f){
            cam.gameObject.transform.position=new Vector3(max-halfWidth+0.5f,0,-1.699f);
        }
    }
}

static class Click{
    public static void click(Camera cam, GameManage manager){
         if (Input.GetMouseButtonDown(0)){
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                string name = hit.collider.gameObject.transform.name;
                if(name.Contains("Player") && !name.Contains("Image"))
                    user(name, manager);
                switch (name){
                    case "PlayerImage":manager.ActiveMenu("user");break;
                    case "storage":manager.ActiveMenu("storage");break;
                    case "shop":manager.ActiveMenu("shop");break;
                    case "rcenter":manager.ActiveMenu("rcenter");break;
                    case "HideMenu":manager.ActiveMenu("hide");break;
                }
                
            }
        }
    }

    private static void user(string name, GameManage manager){
        manager.ActiveMenu("hide");
        int id=int.Parse(name.Replace("Player",""));
        manager.SetUserId(id);

    }    
}