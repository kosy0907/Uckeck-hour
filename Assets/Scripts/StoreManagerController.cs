using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StoreManagerController : MonoBehaviour
{
    public StoreData[] storeDatas;
    public Text coinText;
    public int stars;
    // Start is called before the first frame update
    void Start()
    {
        coinText = GameObject.Find("coin_text").GetComponent<Text>();
        stars = SaveSystem.LoadStars().stars;
        stars = 2;
        Debug.Log(stars);

        coinText.text = "coin: " + stars.ToString();

        int numOfItem = 2;
        createStoreData(numOfItem);

        createStoreObject(numOfItem);
    }

    void createStoreData(int num)
    {
        storeDatas = new StoreData[num];
        string[] itemName = new string[] {"police", "mint car"};
        for (int nn = 0; nn < num; nn++)
        {
            storeDatas[nn] = new StoreData("item_" + nn.ToString(), itemName[nn], 1);
        }
    }

    void createStoreObject(int num)
    {
        for (int nn = 0; nn < num; nn++)
        {
            GameObject itemButton = GameObject.Find("item_button_" + nn.ToString());
            Button button = itemButton.GetComponent<Button>();
            Text priceText = GameObject.Find("price_text_" + nn.ToString()).GetComponent<Text>();
            Text itemText = GameObject.Find("item_name_" + nn.ToString()).GetComponent<Text>();
            int temp = nn;
            button.onClick.AddListener(() => buyItem(temp));

            StoreData data = storeDatas[nn];
  
            priceText.text = data.price.ToString() + "coin";
            itemText.text = data.itemName;
        }
    }

    void buyItem(int itemNumber)
    {
        StoreData data = storeDatas[itemNumber];
        if (stars >= data.price)
        {
            SaveSystem.SaveMyCar(data.resourceName);
            stars -= data.price;
            coinText.text = "coin: " + stars.ToString();
            Debug.Log("구매 완료");
        } else 
        {
            Debug.Log("돈 부족");
        }
    }
}
