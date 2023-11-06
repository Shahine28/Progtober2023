using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TableManager : MonoBehaviour
{
    public TextMeshProUGUI quantityRequired;
    public TextMeshProUGUI quantitPossed;
    public TextMeshProUGUI requiredItemName;
    public TextMeshProUGUI totalQuantityRequired;



    void Start()
    {
        quantityRequired.text = "";
        quantitPossed.text = "";
        requiredItemName.text = "";
        totalQuantityRequired.text = "";
    }


}
