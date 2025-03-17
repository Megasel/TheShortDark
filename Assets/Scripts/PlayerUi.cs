using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUi : MonoBehaviour
{
    public Tutorial tutorial;
    [SerializeField] GameObject metrics;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Pause pause;
    private GameObject currentPanel;
    bool uiIsActive;
    [SerializeField] PlayerMoveController moveController;
    public bool isUiBlocked;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (isUiBlocked) return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(!(inventory.gameObject.activeSelf && inventory.isUsing))
                Toggle(inventory.gameObject);
        }
        else if (Input.GetKeyUp(KeyCode.Escape)) {
            Toggle(pause.gameObject);
        
        }
    }
    public void Toggle(GameObject obj)
    {
        if (!obj.activeSelf && !uiIsActive)
        {
            print("Loot");

            currentPanel = obj;
            uiIsActive = true;
            obj.SetActive(true);
            moveController.isMoving = false;
            moveController.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else if (currentPanel == obj)
        {

            uiIsActive = false;
            obj.SetActive(false);
            moveController.enabled = true;


            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
