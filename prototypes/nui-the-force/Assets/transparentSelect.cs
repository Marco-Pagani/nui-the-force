using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparentSelect : MonoBehaviour{
  private GameObject sphere;
  private Renderer render;
  private Color tempcolor;

    // Start is called before the first frame update
    void Start(){
      Debug.Log("transparency script");
      render = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update(){
      GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

      foreach(GameObject go in allObjects){
        if (go.activeInHierarchy)
           print(go +" is an active object");
           Renderer rend =  go.GetComponent<Renderer>();
           Material material = gameObject.GetComponent<Renderer>().material;
           tempcolor = gameObject.GetComponent<Renderer>().material.color;
           tempcolor.a = 0.4f;
           material.color = tempcolor;
           rend.material = material;
      }
    }

}
