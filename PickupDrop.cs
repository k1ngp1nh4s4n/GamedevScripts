using UnityEngine;
using UnityEngine.InputSystem;

public class PickupDrop : MonoBehaviour
{
    public LayerMask pickupMask;
    public float pickupRange = 3f;
    public bool autoPickupEnabled = false;

    public Transform weaponHoldPoint; // now also holds objects like torch

    private GameObject heldItem;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (autoPickupEnabled && heldItem == null)
        {
            TryPickup();
        }
    }

    public void OnPickup(InputAction.CallbackContext context)
    {
        if (context.performed && !autoPickupEnabled && heldItem == null)
        {
            TryPickup();
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed && heldItem != null)
        {
            DropItem();
        }
    }

    void TryPickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupMask))
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Consumable"))
            {
                Debug.Log(target.name + " consumed!");
                Destroy(target);
            }
            else if (target.CompareTag("Pickable"))
            {
                heldItem = target;
                heldItem.transform.SetParent(weaponHoldPoint);

                if (heldItem.TryGetComponent(out ObjectOffset offset))
                {
                    offset.ApplyOffset();
                }
                else
                {
                    heldItem.transform.localPosition = Vector3.zero;
                    heldItem.transform.localRotation = Quaternion.identity;
                    heldItem.transform.localScale = Vector3.one;
                }

                Collider col = heldItem.GetComponent<Collider>();
                if (col) col.enabled = false;

                if (heldItem.TryGetComponent(out ObjectItem objItem))
                {
                    objItem.OnPickUp();
                }

                Debug.Log(heldItem.name + " picked up (Pickable).");
            }
        }
    }

    void DropItem()
    {
        heldItem.transform.SetParent(null);

        Collider col = heldItem.GetComponent<Collider>();
        if (col) col.enabled = true;

        if (heldItem.TryGetComponent(out ObjectItem objItem))
        {
            objItem.OnDrop();
        }

        Debug.Log(heldItem.name + " dropped.");
        heldItem = null;
    }
}

