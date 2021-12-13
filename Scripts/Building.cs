using UnityEngine;
public class Building:MonoBehaviour
{
   [SerializeField]
   [Range(0,2)]
    private int id;

    [SerializeField]
    private GameManage manage;

    public BuildingTask task;
    public GameObject target;
    void Start()
    {
        task=new BuildingTask().Setup(id,manage,target,this.gameObject);
        manage.AddBuilding(id, task);
    }

    float time=0,truckTime=0;
    void Update()
    {
        time+=Time.deltaTime;
        truckTime+=Time.deltaTime;
        if(task.checkTime(time,0))
            time=0;
        if(task.checkTime(truckTime,1))
            truckTime=0;

    }
}

public class BuildingTask
{
    public float requiredTime=2, workers=0, cars=0,maxCars=0,sendTruck=1.5f;
    public int carPrice=100, workerPrice=70;
    public int lvl;
    public GameManage manage;
    public GameObject target,parent;
    public virtual void Start(){}
    public virtual void Start(Storage s){}
    public virtual void Start(RCenter s){}
    public BuildingTask Setup(int id, GameManage m, GameObject target, GameObject parent){
        BuildingTask b=new BuildingTask();
        switch(id){
            case 0:
                b = new Storage();
                break;
            case 1:
                b = new RCenter();
                break;
            case 2:
                b = new Shop();
                break;
        }        
        b.manage=m;
        b.target=target;
        b.parent=parent;
        b.Start();
        return b;
    }
    public virtual void getValues(float rTime){
        requiredTime=rTime;
    }
    public virtual void work(){}
    public virtual void truck(){}

    public bool checkTime(float time, int timeId){
        if(timeId==0){
            if(time > requiredTime){
                work();
                return true;
            }
            return false;
        }
        if(time > sendTruck){
            truck();
            return true;
        }
        return false;
    }
    
    public virtual void LvlUp(){}
    public virtual void AddResources(GameObject car){}
    public virtual void RemoveResources(){}   
    public void AddCar(){
        cars++;
        maxCars++;
        carPrice*=3;
    }
    public void AddWorker(){
        workers++;
        workerPrice*=2;
    }
    public virtual void LvlUpTime(){}
    public virtual void LvlUpCount(){}
}

public class Storage:BuildingTask
{    
    public int garbages=0,space=100,spaceId=0;
    public int[,] spaceLevels=new int[,]{{100,0},{150,500},{250,1500},{500,3000},{1000,10000}};//[new products count, cost]

    public override void Start()
    {
        requiredTime=1.2f;
        sendTruck=1.5f;
        workers=0;
        cars=2; 
        maxCars=2;       
    }
    
    public override void truck(){         
        if(space - garbages >0){
            for(int i=0;i<(cars <= space - garbages?cars:(space - garbages));i++){
                SpawnCar.Spwan(parent,target,parent.transform,1);
                cars--;
            }
        }
    }

    public override void AddResources(GameObject car){
        garbages++;
        cars++;
    }    
    
    public override void RemoveResources(){
        garbages--;
    }

    public void AddSpace(){
        spaceId++;
        space = spaceLevels[spaceId,0];
    }
}

public class RCenter:BuildingTask
{
    public int garbages=0, items=0,timeLevel=0,productLevel=0;
    private Storage storage;
    public int[,] timeLevels=new int[,]{{20,50},{18,150},{15,500},{12,1000},{10,2000}};//[new time, cost]
    public int[,] productLevels=new int[,]{{1,0},{2,100},{3,500},{4,1200},{5,2000}};//[new products count, cost]
    
    public override void Start()
    {
        requiredTime=timeLevels[0,0];
        sendTruck=1.5f;
        workers=3;
        cars=3;
        maxCars=3;
    }
    public override void work(){
        if(garbages>0){
            garbages-=garbages-productLevels[productLevel,0]<0?0:productLevels[productLevel,0];
            items+=garbages-productLevels[productLevel,0]<0?productLevels[productLevel,0]-garbages:productLevels[productLevel,0];
        }
    }
    public override void AddResources(GameObject car){
        garbages++;
        cars++;
    }
    public override void truck(){   
        if(cars>0 && (manage.buildings[0] as Storage).garbages - (maxCars - cars)>0)
        {
            SpawnCar.Spwan(parent,target,parent.transform,2);
            cars--;
        }
    }
    public override void RemoveResources(){
        garbages--;
    }
}

public class Shop:BuildingTask
{
    public int price=2, items=0,timeLevel=0,productLevel=0;
    private Storage storage;
    public int[,] timeLevels=new int[,]{{20,0},{18,150},{15,500},{12,1000},{10,2000}};//[new time, cost]
    public int[,] productLevels=new int[,]{{1,0},{2,200},{3,500},{4,1000},{5,1500}};//[new products count, cost]
    
    public override void Start()
    {
        requiredTime=timeLevels[0,0];
        sendTruck=1.5f;
        workers=3;
        cars=1;
        maxCars=1;
    }
    public override void work(){
        if(items>0){
            items-=items-productLevels[productLevel,0]<0?0:productLevels[productLevel,0];
            manage.AddDolars((items-productLevels[productLevel,0]<0?productLevels[productLevel,0]-items:productLevels[productLevel,0])*price);
        }
    }
    public override void AddResources(GameObject car){
        items++;
        cars++;
    }
    public override void truck(){   
        if(cars>0 && (manage.buildings[1] as RCenter).items - (maxCars - cars) >0)
        {
            SpawnCar.Spwan(parent,target,parent.transform,3);
            cars--;
        }
    }
}