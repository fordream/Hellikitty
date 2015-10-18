using System;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public static Player player;

    public static void init()
    {
        player = GameObject.Find("playerBase").GetComponent<Player>();
        player.init();
    }
}
