using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Vector3 pos;
    public float angle;

    public void update_gun_motion(GameObject base_obj, Vector2 target)
    {
        Vector3 rota = transform.localEulerAngles;
        angle = Mathf.Atan2(target.y - base_obj.transform.position.y, 
                                  target.x - base_obj.transform.position.x);
        angle *= base_obj.transform.localScale.x / base_obj.transform.localScale.x;
        rota.z = angle * (180.0f / Mathf.PI);
        transform.localEulerAngles = rota;

        Vector3 scale = transform.localScale;
        float scale_y = Mathf.Abs(scale.y);
        if (rota.z <= -90 || rota.z >= 90) scale_y = -scale_y;
        scale.y = scale_y;
        transform.localScale = scale;

        pos = base_obj.transform.position;
        pos.x += Mathf.Cos(angle) * 1;
        pos.y += Mathf.Sin(angle) * 1;
        transform.position = pos;
    }
}
