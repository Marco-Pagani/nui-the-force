using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimize : MonoBehaviour
{
  private GameObject cube;
  private Renderer render;

    // Start is called before the first frame update
    void Start(){
      render = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update(){
    }

    //detect which GameObject to copy
    void OnMouseDown(){
      //to test change color to red
      render.material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
      //copy object
      Copy();
    }
    //get that Game Object

    //copy GameObject
    void Copy(){
      GameObject clone;
      clone = Instantiate(gameObject);
      clone.transform.position = new Vector3(1, 1, -7);
      // clone.transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);
      Mini(clone);
    }
    //minimize GameObjectCopy
    void Mini(GameObject clone){
      if (clone.transform.localScale.y > 0.1f){
        //make it a fourth of it's size
        clone.transform.localScale -= new Vector3(0.75f, 0.75f, 0.75f);
      }
        Renderer rend = clone.GetComponent<Renderer>();
      Wireframe(rend);
    }

    //make GameObjectCopy look like a wireframe
    void Wireframe(Renderer rend){
    ;
      // Material material = new Material(Shader.Find("PDTShaders/TestGrid"));
      Material material = new Material(Shader.Find("UI/Lit/Transparent"));
      material.color = new Color(0.0f, 0.0f, 1.0f, 0.25f);
      rend.material = material;
    }
}
