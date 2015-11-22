using System;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public static Player player;

    public static void init()
    {
        player = GameObject.Find("player").transform.FindChild("base").GetComponent<Player>();
        player.init();
    }

    public static void update()
    {

    }
}
