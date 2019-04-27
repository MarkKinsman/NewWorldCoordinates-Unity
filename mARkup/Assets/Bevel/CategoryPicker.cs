using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//currently unused. Still need to re-architect categories sometime. 
public class CategoryPicker : MonoBehaviour
{

    public enum CategoryOption
    {
        photospheres, entourage, slabs, walls, openings, structure,
        mep, landscape, special, ceilings, areas, rooms, furniture, circulation, casework
    }
    public Category photospheres;
    public Category entourage;
    public Category slabs;
    public Category walls;
    public Category openings;
    public Category structure;
    public Category mep;
    public Category landscape;
    public Category special;
    public Category ceilings;
    public Category areas;
    public Category rooms;
    public Category furniture;
    public Category circulation;
    public Category casework;

    //public Toggle photospheresToggle;
    //public Toggle entourageToggle;
    //public Toggle slabsToggle;
    //public Toggle wallsToggle;
    //public Toggle openingsToggle;
    //public Toggle structureToggle;
    //public Toggle mepToggle;
    //public Toggle landscapeToggle;
    //public Toggle specialToggle;
    //public Toggle ceilingsToggle;
    //public Toggle areasToggle;
    //public Toggle roomsToggle;
    //public Toggle furnitureToggle;
    //public Toggle circulationToggle;
    //public Toggle caseworkToggle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCategories(bool photospheresBool, bool entourageBool, bool slabsBool,
    bool wallsBool, bool openingsBool, bool structureBool, bool mepBool, bool landscapeBool,
    bool specialBool, bool ceilingsBool, bool areasBool, bool roomsBool, bool furnitureBool,
    bool circulationBool, bool caseworkBool)
    {

        if (photospheresBool){TurnOnCategory(photospheres);}
        if (entourageBool) { TurnOnCategory(entourage); }
        if (slabsBool) { TurnOnCategory(slabs); }
        if (wallsBool) { TurnOnCategory(walls); }
        if (openingsBool) { TurnOnCategory(openings); }
        if (structureBool) { TurnOnCategory(structure); }
        if (mepBool) { TurnOnCategory(mep); }
        if (landscapeBool) { TurnOnCategory(landscape); }
        if (specialBool) { TurnOnCategory(special); }
        if (ceilingsBool) { TurnOnCategory(ceilings); }
        if (areasBool) { TurnOnCategory(areas); }
        if (roomsBool) { TurnOnCategory(rooms); }
        if (furnitureBool) { TurnOnCategory(furniture); }
        if (circulationBool) { TurnOnCategory(circulation); }
        if (caseworkBool) { TurnOnCategory(casework); }


    }
    public void SetCategories(CategoryOption[] categoriesInView)
    {
        foreach (var item in categoriesInView)
        {
            
        }
    }

    public static void SelectCategories(Category[] categories)
    {
        Category[] availableCategories = FindObjectsOfType<Category>();
        foreach (var item in availableCategories)
        {
            TurnOffCategory(item);
        }
        foreach (var item in categories)
        {
            TurnOnCategory(item);
        }
    }

    public static void TurnOffCategory(Category category)
    {
        category.categoryToggle.isOn = false;
        foreach (var item in category.contents)
        {

            item.SetActive(false);
        }
    }

    public static void TurnOnCategory(Category category)
    {
        category.categoryToggle.isOn = false;
        foreach (var item in category.contents)
        {
            item.SetActive(true);
        }
    }

}
