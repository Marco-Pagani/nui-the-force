using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparentSelect : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FindAll(){
        Object[] tempList = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        List<GameObject> realList = new List<GameObject>();
        GameObject temp;
        Color tempcolor;

        foreach(Object obj in tempList){
            if(obj is GameObject){
                temp = (GameObject)obj;
                if(temp.hideFlags == HideFlags.None)

                //change the opacity
                tempcolor = GetComponent<Renderer>().material.color;
                tempcolor.a = 0.5f;
                GetComponent<Renderer>().material.color = tempcolor;

              //realList.Add((GameObject)obj);
            }
        }
      //  Selection.objects = realList.ToArray();
    }

}
