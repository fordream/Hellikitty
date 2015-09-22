using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
* All terrain objects are put into this object so any attributes can
* be stored here.
* For example, the original collider offsets (which are calculated on startup) is stored
* here and used when updating the waypoint grid.
*/
class GridTerrain
{
    public GameObject terrain;
    public Ferr2DT_PathTerrain path_terrain;
    public float[] origin_collider_offsets;

    public GridTerrain(GameObject _terrain)
    {
        terrain = _terrain;
        path_terrain = terrain.GetComponent<Ferr2DT_PathTerrain>();

        //gets the collider offsets that can be changed in the editor and stores it in an array
        origin_collider_offsets = new float[path_terrain.surfaceOffset.Length];

        for (int n = 0; n < path_terrain.surfaceOffset.Length; ++n)
        {
            origin_collider_offsets[n] = path_terrain.surfaceOffset[n];
        }
    }
};

/*
* A grid node that contains attributes such as its grid pos, world pos and whether
* it is walkable or not.
*/
class WaypointNode
{
    public Vector2 grid_pos;
    public Vector3 world_pos;
    public bool walkable = true;

    public WaypointNode(int _x, int _y, Vector3 _world_pos)
    {
        grid_pos = new Vector2(_x, _y);
        world_pos = _world_pos;
    }
};

/*
* Handles the creation of a waypoint grid.
* The grid is updated by raycasting at every node to see if there are any collisions
* with any objects. This data can then be used by enemies to move around non-walkable
* objects by using a pathfinding algorithm instead of performing mesh collisions.
*/
public class WaypointGrid : MonoBehaviour
{

    bool init = false;

    GameObject waypoint_bg;
    GameObject waypoint_debug_box;
    GameObject waypoint_debug_group;
    Bounds waypoint_bg_bounds;
    Vector3 waypoint_node_start;

    List<GridTerrain> grid_terrain = new List<GridTerrain>();
    List<WaypointNode> waypoint_nodes = new List<WaypointNode>();
    private int grid_width;
    private int grid_height;

    //editor variables
    public float point_seperation;          //the seperation value between each point in the grid
    public float collider_offset;           //the larger the collider offset, the larger objects will be when calculating grid points

    void Start()
    {
        waypoint_bg = GameObject.Find("waypoint_bg");
        waypoint_debug_box = (GameObject)Resources.Load("waypoint_debug_box");
        waypoint_debug_group = GameObject.Find("waypoint_debug_group");

        //sets the start point position at the top left point of the waypoint bg
        waypoint_bg_bounds = waypoint_bg.GetComponent<Renderer>().bounds;
        waypoint_node_start = new Vector3(waypoint_bg_bounds.min.x, waypoint_bg_bounds.max.y, -.2f);
        waypoint_node_start.x += point_seperation;
        waypoint_node_start.y -= point_seperation;

        grid_width = (int)(waypoint_bg_bounds.size.x / point_seperation - 2);
        grid_height = (int)(waypoint_bg_bounds.size.y / point_seperation - 1);

        //creates the waypoint grid nodes with the width and height of the grid
        int row = 0;
        int column = 0;
        Vector3 world_pos = waypoint_node_start;
        for (int n = 0; n < grid_width * grid_height; ++n)
        {
            waypoint_nodes.Add(new WaypointNode(row, column, world_pos));

            world_pos.x += point_seperation;
            ++row;
            if (row >= grid_width)
            {
                world_pos.x = waypoint_node_start.x;
                world_pos.y -= point_seperation;

                row = 0;
                ++column;
            }
        }
        Debug.Log("waypoint grid created (" + grid_width + "x" + grid_height + ")");

        //gets all terrain objects, makes them a grid terrain and adds them to the grid terrain list
        GameObject terrain_group = GameObject.Find("terrain_group");
        for (int n = 0; n < terrain_group.transform.childCount; ++n)
        {
            grid_terrain.Add(new GridTerrain(terrain_group.transform.GetChild(n).gameObject));
        }
    }

    void Update()
    {
        //raycast2d must be performed once the game is updating, it does not work in start, so only init once
        if (!init)
        {
            init = true;

            recalc_waypoint_nodes();
        }

        recalc_waypoint_nodes();
        recreate_debug_grid();
    }

    void recalc_waypoint_nodes()
    {
        //sets the collider offset to all grid terrain surface collider offsets
        foreach (GridTerrain t in grid_terrain)
        {
            for (int n = 0; n < t.origin_collider_offsets.Length; ++n)
            {
                t.path_terrain.surfaceOffset[n] = collider_offset;
            }
            t.path_terrain.RecreateCollider();
        }

        //updates the waypoint grid
        for (int y = 0; y < grid_height; ++y)
        {
            for (int x = 0; x < grid_width; ++x)
            {
                WaypointNode node = get_node(x, y);
                RaycastHit2D hit = Physics2D.Raycast(node.world_pos, Vector2.zero);
                node.walkable = !hit;
            }
        }

        //sets all grid terrain surface collider offsets to their original values
        foreach (GridTerrain t in grid_terrain)
        {
            for (int n = 0; n < t.origin_collider_offsets.Length; ++n)
            {
                t.path_terrain.surfaceOffset[n] = t.origin_collider_offsets[n];
            }
            t.path_terrain.RecreateCollider();
        }
    }

    void recreate_debug_grid()
    {
        //destroys all debug box sprites
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("waypoint_debug_box"))
        {
            GameObject.Destroy(obj);
        }

        //creates a debug box sprite for every node on the grid
        for (int y = 0; y < grid_height; ++y)
        {
            for (int x = 0; x < grid_width; ++x)
            {
                WaypointNode node = get_node(x, y);
                Color colour = Color.blue;
                if (!node.walkable) colour = Color.red;

                GameObject box = (GameObject)GameObject.Instantiate(waypoint_debug_box, node.world_pos, Quaternion.identity);
                box.transform.parent = waypoint_debug_group.transform;
                box.GetComponent<SpriteRenderer>().color = colour;
            }
        }
    }

    /*
    * Returns a waypoint node from the grid if x and y is in bounds, if one of them is not, then return null
    */
    WaypointNode get_node(int _x, int _y)
    {
        if (_x >= grid_width || _x < 0)
        {
            Debug.Log(_x + " is out of bounds of grid width (" + grid_width + ")");
            return null;
        }
        if (_y >= grid_height || _y < 0)
        {
            Debug.Log(_y + " is out of bounds of grid height (" + grid_height + ")");
            return null;
        }
        return waypoint_nodes[(_y * grid_width) + _x];
    }

    /*
    * Returns a waypoint node from the grid if x and y is in bounds, if one of them is not, then return null
    */
    WaypointNode get_node(float _x, float _y)
    {
        return get_node((int)_x, (int)_y);
    }

    /*
    * Returns a waypoint node from the grid if x and y is in bounds, if one of them is not, then return null
    */
    WaypointNode get_node(Vector3 v)
    {
        return get_node((int)v.x, (int)v.y);
    }

}
