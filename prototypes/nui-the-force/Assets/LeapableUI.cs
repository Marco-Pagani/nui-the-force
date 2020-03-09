using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

[RequireComponent(typeof(InteractionBehaviour))]
[RequireComponent(typeof(BoxCollider))]
public class LeapableUI : MonoBehaviour
{
    private InteractionBehaviour ib;

    private UnityEngine.UI.Button button;
    private RectTransform rectTransform;

    private BoxCollider collie;

    private Color normieBoi;
    private Color pressedBoi;

    // Start is called before the first frame update
    void Start()
    {
        ib = GetComponent<InteractionBehaviour>();
        ib.rigidbody.useGravity = false;
        ib.rigidbody.isKinematic = true;
        ib.moveObjectWhenGrasped = false;
        button = GetComponent<UnityEngine.UI.Button>();
        rectTransform = GetComponent<RectTransform>();
        //collie = gameObject.AddComponent<BoxCollider>();
        collie = GetComponent<BoxCollider>();
        collie.size = new Vector3(rectTransform.rect.width, rectTransform.rect.height, .1f);
        normieBoi = button.colors.normalColor;
        pressedBoi = button.colors.highlightedColor;
        button.image.color = normieBoi;
        ib.OnContactBegin += delegate ()
        {
            button.image.color = pressedBoi;
            button.onClick?.Invoke();
            Debug.Log("Touching...");
        };
        ib.OnContactEnd += delegate ()
        {
            button.image.color = normieBoi;
            Debug.Log("NO Touching...");
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
