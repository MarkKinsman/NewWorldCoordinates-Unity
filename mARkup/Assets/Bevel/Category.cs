using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Category : MonoBehaviour
{
    public enum CategoryOption
    {
        photospheres, entourage, slabs, walls, openings, structure,
        mep, site, planting, special, ceilings, areas, rooms, furniture, circulation, misc
    }

    public Category.CategoryOption categoryName;
    [Tooltip("Drag in all non-child contents you want in the category. Children will be included automatically.")]
    public List<GameObject> contents;
    public Toggle categoryToggle;
    public bool isOn = false;

    // Use this for initialization
    void Start()
    {
        //add in the children to make things easier
        Transform[] children = gameObject.GetComponentsInChildren<Transform>(true);
        for (int i = 1; i < children.Length; i++)
        {
            contents.Add(children[i].gameObject);
        }

        //make sure the state is right
        if (isOn)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }


    public static void TurnOffCategory(Category category)
    {
        category.TurnOff();
    }

    public static void TurnOnCategory(Category category)
    {
        category.TurnOn();
    }

    public void TurnOn()
    {
        categoryToggle.isOn = true;
        foreach (var item in contents)
        {
            item.SetActive(true);
        }
        isOn = true;
    }
    public void TurnOff()
    {
        categoryToggle.isOn = false;
        foreach (var item in contents)
        {
            item.SetActive(false);
        }
        isOn = false;
    }
    public void Toggle()
    {
        if (isOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

}