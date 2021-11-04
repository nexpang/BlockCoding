using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStickInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    [HideInInspector]
    public bool joyStick = false;
    [HideInInspector]
    public bool joyStickHold = false;

    public static float horizontalRaw = 0;
    public static float verticalRaw = 0;

    public Sprite[] joystickes;

    private Image img;

    private int touchId;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if(joyStickHold)
        {
            Vector2 touchPos = Vector2.zero;
            #if UNITY_EDITOR
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            #else
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if(Input.touches[i].fingerId == touchId)
                {
                    touchPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                }
            }
            #endif
            Vector2 direct = (touchPos - (Vector2)transform.position).normalized;

            //horizontal = direct.y;
            //vertical = direct.x;

            if (direct.x >= -0.7f && direct.y >= 0.7f)
            {
                //up
                img.sprite = joystickes[1];
                horizontalRaw = 0;
                verticalRaw = 1;
            }
            else if(direct.x< -0.7f && direct.y > -0.7f && direct.y < 0.7f)
            {
                //left
                img.sprite = joystickes[3];
                horizontalRaw = -1;
                verticalRaw = 0;
            }
            else if (direct.x > -0.7f && direct.y > -0.7f && direct.y < 0.7f)
            {
                // right
                img.sprite = joystickes[4];
                horizontalRaw = 1;
                verticalRaw = 0;
            }
            else
            {
                //down
                img.sprite = joystickes[2];
                horizontalRaw = 0;
                verticalRaw = -1;
            }

            //print(touchPos);
            //print((Vector2)transform.position);
            //print(direct);

            //float dist = Vector2.Distance(touchPos, transform.position);

            //if(dist< radius)
            //{
            //    fSqr = dist;
            //}
            //else
            //{
            //    fSqr = radius;
            //}
            //lever.position = (Vector2)transform.position + direct * fSqr;
            

        }
        else
        {
            if (horizontalRaw != 0 || verticalRaw != 0)
            {
                horizontalRaw = 0;
                verticalRaw = 0;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touchId = eventData.pointerId;
        joyStick = true;
        joyStickHold = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joyStick = false;
        joyStickHold = false;
        img.sprite = joystickes[0];
    }
}
