using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionText : MonoBehaviour
{
    public static void spawn_damage_text(Vector2 pos, int damage)
    {
        GameObject text = (GameObject)Instantiate(Resources.Load("ui/damage_text"));
        text.transform.position = new Vector3(pos.x, pos.y, -40.0f);
        text.GetComponent<TextMesh>().text = "-" + damage;
    }
}
