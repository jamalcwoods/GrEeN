using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUIOption : MonoBehaviour
{
    public passiveData data;
    public Image icon;
    public Text title;
    public Text description;
    public Text cost;
    public Button button;
    void Update()
    {
        icon.sprite = data.icon;
        title.text = data.title;
        description.text = data.description;
        cost.text = "Cost: " + data.cost;
        button.gameObject.SetActive(true);
        if (accountStats.passives.Contains(data.index))
        {
            cost.text = "";
            button.transform.Find("Text").GetComponent<Text>().text = "Remove";
            button.gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            if(accountStats.passives.IndexOf(0) != -1)
            {
                if (accountStats.ownedPassives.Contains(data.index))
                {
                    cost.text = "";
                    button.transform.Find("Text").GetComponent<Text>().text = "Equip";
                    button.gameObject.GetComponent<Image>().color = Color.yellow;
                }
                else
                {
                    if(accountStats.resource >= data.cost)
                    {
                        button.transform.Find("Text").GetComponent<Text>().text = "Purchase";
                        button.gameObject.GetComponent<Image>().color = Color.green;
                    } else
                    {
                        button.transform.Find("Text").GetComponent<Text>().text = "Need More Points";
                        button.gameObject.GetComponent<Image>().color = new Color(1f,140f/255f,0);
                    }
                }
            } else
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public void processPassiveSetting()
    {
        if (accountStats.passives.Contains(data.index))
        {
            accountStats.passives[accountStats.passives.IndexOf(data.index)] = 0;
        } else
        {
            if (accountStats.ownedPassives.Contains(data.index))
            {
                accountStats.passives[accountStats.passives.IndexOf(0)] = data.index;
            }
            else
            {
                if(accountStats.resource >= data.cost)
                {
                    accountStats.ownedPassives.Add(data.index);
                    accountStats.passives[accountStats.passives.IndexOf(0)] = data.index;
                    accountStats.resource -= data.cost;
                }
            }
        }
    }
}
