using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLineInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.currentSelectCircle != null)
            {
                GameManager.Instance.currentSelectCircle.CheckDistance(GameManager.ScreenToWorldPoint());
            }
        }
    }
}
