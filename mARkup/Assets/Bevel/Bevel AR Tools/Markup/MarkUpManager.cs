using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bevel;
using System;

namespace Bevel
{
    public class MarkUpManager : MonoBehaviour
    {
        public GameObject markupPrefab;

        private BevelInput bevelInput;

        private bool isWaitingForClick;

        // Use this for initialization
        void Start()
        {
            bevelInput = FindObjectOfType<BevelInput>();

            DatabaseManager.instance.recievedMarkup += LoadMarkUp;
        }

        private void OnDisable()
        {
            DatabaseManager.instance.recievedMarkup -= LoadMarkUp;
        }

        // Update is called once per frame
        void Update()
        {
            if (isWaitingForClick)
            {
                testAddClick();
            }
        }

        //Just to initialize the process. Turn on that listeners for that. 
        public void StartMarkup()
        {
            isWaitingForClick = true;
        }

        void testAddClick()
        {
            if (BevelInput.isSomethingPressed())
            {
                Ray clickRay = BevelInput.GetAnyClickRay();
                RaycastHit hit;
                if (Physics.Raycast(clickRay, out hit, 1000f, bevelInput.defaultClickLayers))
                {
                    isWaitingForClick = false;
                    AddMarkUp(hit);
                }
            }

        }

        //for adding new markup by spatial interface
        public void AddMarkUp(RaycastHit hit)
        {
            GameObject markupObject = Instantiate(markupPrefab);
            MarkUp markUp = markupObject.GetComponent<MarkUp>();

            markupObject.transform.position = hit.point;
            markupObject.transform.parent = transform;

            markUp.markupData.location = markupObject.transform.position;
        }

        //for loading markups that are saved to the server
        public void LoadMarkUp(MarkUpRecord.MarkUpType type, MarkUpRecord markUpInstance)
        {
            GameObject markupObject = Instantiate(markupPrefab);
            MarkUp markUp = markupObject.GetComponent<MarkUp>();
            markupObject.transform.parent = transform;

            markUp.LoadRecord(markUpInstance);
        }

    }

}
