using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManage : MonoBehaviour
{
    [SerializeField]
    private int dolars=500, user_id=-1;
    [SerializeField]
    GameObject userImage, userMenu, storageMenu, shopMenu, RCenterMenu;
    [SerializeField]
    Sprite[] userImages;
    [SerializeField]
    Text[] resoursesText;
    [SerializeField]
    Text[] StorageTextInfo, ShopTextInfo, RCenterTextInfo;
    public BuildingTask[] buildings = new BuildingTask[3];
    void Start()
    {
        GetGameData();
    }
    public void AddBuilding(int id, BuildingTask task)
    {
        buildings[id] = task;
    }
    private void GetGameData()
    {
        dolars = PlayerPrefs.GetInt("dolars");
        if (user_id != -1)
            SetUser(user_id);
        else
            ActiveMenu("user");

    }

    void Update()
    {
        UpadteData();
        Click.click(Camera.main, this.GetComponent<GameManage>());
    }

    private void UpadteData()
    {
        resoursesText[0].text = dolars.ToString();
    }
    public void SetUserId(int id)
    {
        user_id = id - 1;
        SetUser(id - 1);
    }

    private void SetUser(int id)
    {
        userImage.GetComponent<Image>().sprite = userImages[id];
        ActiveMenu("hide");
    }

    public void ActiveMenu(string name)
    {
        switch (name)
        {
            case "user": userMenu.SetActive(true); break;
            case "storage":
                Storage storage = buildings[0] as Storage;
                StorageTextInfo[0].text = (storage.space - storage.garbages).ToString();
                StorageTextInfo[1].text = storage.space.ToString();
                StorageTextInfo[2].text = storage.maxCars.ToString();
                StorageTextInfo[3].text = storage.spaceId < 5 ? storage.spaceLevels[storage.spaceId + 1, 1].ToString() : "MAX";
                StorageTextInfo[4].text = storage.maxCars < 31 ? storage.carPrice.ToString() : "MAX";
                storageMenu.SetActive(true);
                break;
            case "shop":
                Shop shop = buildings[2] as Shop;

                ShopTextInfo[0].text = shop.workers.ToString();
                ShopTextInfo[1].text = shop.maxCars.ToString();
                ShopTextInfo[2].text = shop.timeLevels[shop.timeLevel, 0].ToString();
                ShopTextInfo[3].text = shop.productLevels[shop.productLevel, 0].ToString();

                ShopTextInfo[4].text = shop.workerPrice.ToString();
                ShopTextInfo[5].text = shop.maxCars < 31 ? shop.carPrice.ToString() : "MAX";
                ShopTextInfo[6].text = shop.timeLevel < 5 ? shop.timeLevels[shop.timeLevel + 1, 1].ToString() : "MAX";
                ShopTextInfo[7].text = shop.productLevel < 5 ? shop.productLevels[shop.productLevel + 1, 1].ToString() : "MAX";

                shopMenu.SetActive(true);
                break;

            case "rcenter":

                RCenter rCenter = buildings[1] as RCenter;

                RCenterTextInfo[0].text = rCenter.workers.ToString();
                RCenterTextInfo[1].text = rCenter.maxCars.ToString();
                RCenterTextInfo[2].text = rCenter.timeLevels[rCenter.timeLevel, 0].ToString();
                RCenterTextInfo[3].text = rCenter.productLevels[rCenter.productLevel, 0].ToString();

                RCenterTextInfo[4].text = rCenter.workerPrice.ToString();
                RCenterTextInfo[5].text = rCenter.maxCars < 31 ? rCenter.carPrice.ToString() : "MAX";
                RCenterTextInfo[6].text = rCenter.timeLevel < 5 ? rCenter.timeLevels[rCenter.timeLevel + 1, 1].ToString() : "MAX";
                RCenterTextInfo[7].text = rCenter.productLevel < 5 ? rCenter.productLevels[rCenter.productLevel + 1, 1].ToString() : "MAX";
                RCenterMenu.SetActive(true); break;

            case "hide":
                storageMenu.SetActive(false);
                userMenu.SetActive(false);
                shopMenu.SetActive(false);
                RCenterMenu.SetActive(false);
                if (user_id == -1)
                {
                    ActiveMenu("user");
                }
                break;
        }
    }

    public void LvlUpStorageSpace()
    {
        Storage s = buildings[0] as Storage;
        if (s.spaceId < 4)
        {
            if (dolars >= s.spaceLevels[s.spaceId + 1, 1])
            {
                s.AddSpace();
                dolars -= s.spaceLevels[s.spaceId, 1];
                ActiveMenu("storage");
            }
        }
    }

    public void AddCar(int id)
    {
        if (dolars >= buildings[id].carPrice)
        {
            dolars -= buildings[id].carPrice;
            buildings[id].AddCar();
            ActiveMenu(buildings[id].parent.name);
        }
    }

    public void AddWorker(int id)
    {
        if (dolars >= buildings[id].workerPrice)
        {
            dolars -= buildings[id].workerPrice;
            buildings[id].AddWorker();
            ActiveMenu(buildings[id].parent.name);
        }
    }
    public void AddDolars(int quantity)
    {
        dolars += quantity;
    }

    public void LvlUpWork(int id){
        if(id == 1){
            RCenter s = buildings[1] as RCenter;
            if (s.timeLevel < 4)
            {
                if (dolars >= s.productLevels[s.productLevel + 1, 1])
                {
                    Debug.Log(s.productLevels[s.productLevel+1, 1]);
                    dolars -= s.productLevels[s.productLevel+1, 1];
                    s.productLevel++;
                    s.requiredTime=s.productLevels[s.productLevel,0];
                    ActiveMenu("rcenter");
                }
            }
        }
        else{
            Shop s = buildings[2] as Shop;
            if (s.productLevel < 4)
            {
                if (dolars >= s.productLevels[s.productLevel + 1, 1])
                {
                    dolars -= s.productLevels[s.productLevel+1, 1];
                    s.productLevel++;
                    s.requiredTime=s.productLevels[s.productLevel,0];
                    ActiveMenu("shop");
                }
            }
        }
    }

    public void LvlUpTime(int id){
        if(id == 1){
            RCenter s = buildings[1] as RCenter;
            if (s.timeLevel < 4)
            {
                if (dolars >= s.timeLevels[s.timeLevel + 1, 1])
                {
                    dolars -= s.timeLevels[s.timeLevel + 1, 1];
                    s.timeLevel++;
                    s.requiredTime=s.timeLevels[s.timeLevel,0];
                    ActiveMenu("rcenter");
                }
            }
        }
        else{
            Shop s = buildings[2] as Shop;
            Debug.Log(s.timeLevels[s.timeLevel,1]);
            if (s.timeLevel < 4)
            {
                if (dolars >= s.timeLevels[s.timeLevel + 1, 1])
                {
                    dolars -= s.timeLevels[s.timeLevel + 1, 1];
                    s.timeLevel++;
                    s.requiredTime=s.timeLevels[s.timeLevel,0];
                    ActiveMenu("shop");
                }
            }
        }
    }

}