using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomach : MonoBehaviour
{
    public event EventHandler onHungry;
    
    private bool _isHungry;
}
