using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestObject : MonoBehaviour
{
    public void Open()
    {
        transform.parent.GetComponent<ChestManager>().OpenChest(this);
    }
}
