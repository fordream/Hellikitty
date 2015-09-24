using UnityEngine;

class EnemySpawner : Singleton<EnemySpawner> {

    public void init()
    {
        spawn_enemy();
    }

    void spawn_enemy()
    {
        GameObject enemy = (GameObject)GameObject.Instantiate(Resources.Load("enemy"));
    }

    public void update()
    {

    }
}
