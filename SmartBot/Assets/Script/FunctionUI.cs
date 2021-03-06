﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FunctionUI : MonoBehaviour {

    public GameObject Fct_background;
    public GameObject Sub_Fct_background;
    

    public string[] instractStack;
    public string[] subInstractStack;
    public Button[] instractBtns;

    public string[] currentStack;

    private void Awake()
    {
        int currentInstractStackNumber = Fct_background.transform.childCount;
        instractStack = new string[currentInstractStackNumber];
        int current_Sub_InstractStackNumber = Sub_Fct_background.transform.childCount;
        subInstractStack = new string[current_Sub_InstractStackNumber];

        foreach (var btn in instractBtns)
        {
            btn.onClick.AddListener(() => instractRespond(btn.name));
        }
        //可能会变动------------------------------------------------
            for(var i = 0;i < Fct_background.transform.GetComponentsInChildren<Button>().Length; i++)
            {
                if (Fct_background.transform.GetComponentsInChildren<Button>()[i].gameObject != Fct_background)
                {
                    var current = Fct_background.transform.GetComponentsInChildren<Button>()[i];
                    current.onClick.AddListener(() => 
                    popStack(Fct_background, Sub_Fct_background, instractStack, current.name));
                }
            }
            for(var i = 0; i < Sub_Fct_background.transform.GetComponentsInChildren<Button>().Length;i++)
            {
                if(Sub_Fct_background.transform.GetComponentsInChildren<Button>()[i].gameObject != Sub_Fct_background)
                {
                Button current = Sub_Fct_background.transform.GetComponentsInChildren<Button>()[i];
                current.onClick.AddListener(() =>
                    popStack(Sub_Fct_background, Fct_background, subInstractStack,current.name));
                }
            }
        //----------------------------------------------------------

        Fct_background.GetComponent<Button>().onClick.AddListener(() 
            => chooseCurrentinstractStack(Fct_background, Sub_Fct_background, instractStack));

        Sub_Fct_background.GetComponent<Button>().onClick.AddListener(()
            => chooseCurrentinstractStack(Sub_Fct_background, Fct_background, subInstractStack));

        this.currentStack = instractStack;
    }


    private void popStack(GameObject gameObj,GameObject setGameObjectColor, string[] CurrentStack, string name)
    {
        chooseCurrentinstractStack(gameObj, setGameObjectColor, CurrentStack);
        Debug.Log(name);
        var index = int.Parse(name.Substring(name.Length-1, 1));
        popInCurrentStack(index);
    }

    private void popInCurrentStack(int index)
    {
        this.currentStack[index]=null;
        for(var i = 0; i < this.currentStack.Length-1 ;i++)
        {
            if(this.currentStack[i] == null)
            {
                if(this.currentStack[i+1] != null)
                {

                    this.currentStack[i] = this.currentStack[i + 1];
                    this.currentStack[i + 1] = null;
                }
            }
        }
    }

    private void chooseCurrentinstractStack(GameObject gameObj,
        GameObject setGameObjectColor, string [] CurrentStack)
    {
        this.currentStack = CurrentStack;
        foreach (var image in gameObj.transform.GetComponentsInChildren<Image>())
        {
            if(image.gameObject == gameObj)
            {
                continue;
            }
            //image.color = new Color(0.74f,0.72f,0,1f); //被选中的颜色
            image.color = new Color(241.0f/255.0f,228.0f/255.0f,153.0f/255.0f);
        }

        foreach (var image in setGameObjectColor.transform.GetComponentsInChildren<Image>())
        {
            if (image.gameObject == setGameObjectColor)
            {
                continue;
            }
            //image.color = new Color( 0.76f , 0.76f, 0.76f, 1);  //没有被选择的颜色
            image.color = new Color(204.0f/255.0f,189.0f/255.0f,144.0f/255.0f,1);
        }
    }
    
    private void instractRespond(string name)
    {
        if(GetCurrentInstractNumber() >= this.currentStack.Length)
        {
            return;
        }
        else
        {
            for(int i = 0; i < this.currentStack.Length; i++)
            {
                if(this.currentStack[i] == null)
                {
                    this.currentStack[i] = name;
                    break;
                }
            }
        }
    }
    
    private int GetCurrentInstractNumber()
    {
        var i = 0;
        foreach(var stack in this.currentStack)
        {
            if(stack == "" || stack == null)
            {
                i++;
            }
        }
        return (this.currentStack.Length - i);
    }


    private void FixedUpdate()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        var current_Fct_CImages = Fct_background.transform.GetComponentsInChildren<CImage>();
        for(int i = 0; i < current_Fct_CImages.Length; i++)
        {
            current_Fct_CImages[i].displaySprite(instractStack[i]);
        }

        var current_SubFct_CImages = Sub_Fct_background.transform.GetComponentsInChildren<CImage>();
        for (int i = 0; i < current_SubFct_CImages.Length; i++)
        {
            current_SubFct_CImages[i].displaySprite(subInstractStack[i]);
           
        }

    }


}
