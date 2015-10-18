using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour {

    public Enemy enemy;
    float rate_timer;

    void Start()
    {
        GameObject obj = transform.parent.FindChild("base").gameObject;
        if (obj == null) Debug.LogError("Enemies require a child named 'base'");
        enemy = obj.GetComponent<Enemy>();
    }

    void Update()
    {
        Vector3 rota = transform.localEulerAngles;
        float angle = Mathf.Atan2(Entities.player.transform.position.y - transform.position.y,
                                    Entities.player.transform.position.x - transform.position.x);
        angle *= enemy.transform.localScale.x / enemy.transform.localScale.x;
        rota.z = angle * (180.0f / Mathf.PI);
        transform.localEulerAngles = rota;

        Vector3 scale = transform.localScale;
        float scale_y = Mathf.Abs(scale.y);
        if (rota.z <= -90 || rota.z >= 90) scale_y = -scale_y;
        scale.y = scale_y;
        transform.localScale = scale;

        Vector3 pos = enemy.transform.position;
        pos.x += Mathf.Cos(angle) * 1;
        pos.y += Mathf.Sin(angle) * 1;
        transform.position = pos;

        if (enemy.general_ai_state == GeneralAIState.SHOOTING)
        {
            rate_timer += Time.deltaTime;
            if (rate_timer >= .1f)
            {
                rate_timer = 0;
                Bullet.spawn(pos).add_logic<BasicBullet>().init(angle);
            }
        }
	}
}
