﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTarget : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTarget");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Turret>().Target(player.transform.position);
    }
}
