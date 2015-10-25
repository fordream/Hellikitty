using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/*
* All terrain objects are put into this object so any attributes can
* be stored here.
* For example, the original collider offsets (which are calculated on startup) is stored
* here and used when updating the waypoint grid.
*/
public class GridTerrain
{
    public GameObject terrain;
    public Ferr2DT_PathTerrain path_terrain;
    public float[] origin_size_offsets;

    public GridTerrain(GameObject _terrain)
    {
        terrain = _terrain;
        path_terrain = terrain.GetComponent<Ferr2DT_PathTerrain>();

        //gets the collider offsets that can be changed in the editor and stores it in an array
        origin_size_offsets = new float[path_terrain.surfaceOffset.Length];

        for (int n = 0; n < path_terrain.surfaceOffset.Length; ++n)
        {
            origin_size_offsets[n] = path_terrain.surfaceOffset[n];
        }
    }
};

/*
* A grid node that contains attributes such as its grid pos, world pos and whether
* it is walkable or not.
*/
public class WaypointNode
{
    public Vector2 grid_pos;
    public Vector3 world_pos;
    public bool walkable = true;
    public bool closed = false;
    public bool open = false;
    public bool debug_draw_path = false;

    public int g = 0;
    public int h = 0;
    public int f = 0;
    public WaypointNode parent;

    public WaypointNode(int x, int y, Vector3 world_pos)
    {
        grid_pos = new Vector2(x, y);
        this.world_pos = world_pos;
    }
};

/*
* Handles the creation of a waypoint grid.
* The grid is updated by raycasting at every node to see if there are any collisions
* with any objects. This data can then be used by enemies to move around non-walkable
* objects by using a pathfinding algorithm instead of performing mesh collisions.
*
* Pathfinding should really be handled without a grid, but ain't nobody got time for that
* right now!
*/
public class WaypointGrid
{
    bool has_init = false;
    bool has_recalc = false;
    GameObject waypoint_bg;
    GameObject waypoint_debug_box;
    GameObject waypoint_debug_group;
    Bounds waypoint_bg_bounds;
    Vector3 waypoint_node_start;

    public List<WaypointNode> waypoint_nodes = new List<WaypointNode>();
    public int grid_width;
    public int grid_height;
    public float world_grid_width;
    public float world_grid_height;

    List<GridTerrain> grid_terrain = new List<GridTerrain>();
    List<GameObject> debug_boxes = new List<GameObject>();
    
    public WaypointGrid()
    {
        waypoint_bg = GameObject.Find("waypoint_bg");
        waypoint_debug_box = (GameObject)Resources.Load("misc/waypoint_debug_box");
        waypoint_debug_group = GameObject.Find("waypoint_debug_group");

        //sets the start point position at the top left point of the waypoint bg
        waypoint_bg_bounds = waypoint_bg.GetComponent<Renderer>().bounds;
        waypoint_node_start = new Vector3(waypoint_bg_bounds.min.x, waypoint_bg_bounds.max.y, -.2f);

        //gets all terrain objects, makes them a grid terrain and adds them to the grid terrain list
        GameObject terrain_group = GameObject.Find("terrain_group");
        for (int n = 0; n < terrain_group.transform.childCount; ++n)
        {
            grid_terrain.Add(new GridTerrain(terrain_group.transform.GetChild(n).gameObject));
        }

        recreate_grid();

        recalc_waypoint_nodes();
        has_init = true;
    }

    public void recreate_grid()
    {
        waypoint_nodes.Clear();

        Vector3 start_pos = waypoint_node_start;
        start_pos.x += Debug.config.grid_point_sep;
        start_pos.y -= Debug.config.grid_point_sep;

        grid_width = (int)(waypoint_bg_bounds.size.x / Debug.config.grid_point_sep - 1);
        grid_height = (int)(waypoint_bg_bounds.size.y / Debug.config.grid_point_sep - 1);
        world_grid_width = waypoint_bg_bounds.size.x - Debug.config.grid_point_sep - 1;
        world_grid_height = waypoint_bg_bounds.size.y - Debug.config.grid_point_sep - 1;

        //creates the waypoint grid nodes with the width and height of the grid
        int row = 0;
        int column = 0;
        Vector3 world_pos = start_pos;
        world_pos.z = -25;
        for (int n = 0; n < grid_width * grid_height; ++n)
        {
            waypoint_nodes.Add(new WaypointNode(row, column, world_pos));

            world_pos.x += Debug.config.grid_point_sep;
            ++row;
            if (row >= grid_width)
            {
                world_pos.x = start_pos.x;
                world_pos.y -= Debug.config.grid_point_sep;

                row = 0;
                ++column;
            }
        }
        Debug.Log("waypoint grid created (" + grid_width + "x" + grid_height + ")");

        //creates a debug box sprite for every node on the grid
        remove_debug_boxes();
        if (Debug.config.grid_debug_display)
        {
            for (int y = 0; y < grid_height; ++y)
            {
                for (int x = 0; x < grid_width; ++x)
                {
                    Vector3 pos = get_node(x, y).world_pos;
                    pos.z = -20;
                    GameObject box = (GameObject)GameObject.Instantiate(waypoint_debug_box, pos, Quaternion.identity);
                    box.transform.parent = waypoint_debug_group.transform;
                    debug_boxes.Add(box);
                }
            }
            if (has_init) refresh_debug_boxes();
        }

        if (has_init) recalc_waypoint_nodes();
    }

    public void update()
    {
        //recalc waypoint nodes after all other scripts have been initialised
        //this is because raycast2d cannot run in the start call
        if (!has_recalc)
        {
            has_recalc = true;
            recalc_waypoint_nodes();
        }

        if (Debug.config.grid_debug_display)
        {
            recalc_waypoint_nodes();
            refresh_debug_boxes();
        }else {
            remove_debug_boxes();
        }
    }

    public void reset_surface_offset(float offset)
    {
        //sets the collider offset to all grid terrain surface collider offsets
        foreach (GridTerrain t in grid_terrain)
        {
            for (int n = 0; n < t.origin_size_offsets.Length; ++n)
            {
                t.path_terrain.surfaceOffset[n] = offset;
                if (offset == 0 && n == 0) t.path_terrain.surfaceOffset[n] = 1.0f;
            }
            t.path_terrain.RecreateCollider();
        }
    }

    public void recalc_waypoint_nodes()
    {
        reset_surface_offset(Debug.config.grid_terrain_offset);

        //updates the waypoint grid
        for (int y = 0; y < grid_height; ++y)
        {
            for (int x = 0; x < grid_width; ++x)
            {
                WaypointNode node = get_node(x, y);
                RaycastHit2D hit = Physics2D.Raycast(node.world_pos, Vector2.zero, 
                    float.MaxValue, Debug.config.grid_collidable_layers);
                node.walkable = !hit;
            }
        }

        reset_surface_offset(0);
    }

    public void remove_debug_boxes()
    {
        if (debug_boxes.Count == 0) return;

        foreach (GameObject box in debug_boxes)
        {
            GameObject.Destroy(box);
        }
        debug_boxes.Clear();
    }

    public void refresh_debug_boxes()
    {
        //creates a debug box sprite for every node on the grid
        for (int y = 0; y < grid_height; ++y)
        {
            for (int x = 0; x < grid_width; ++x)
            {
                WaypointNode node = get_node(x, y);
                Color colour = Color.blue;
                if (!node.walkable) colour = Color.red;
                if (node.debug_draw_path) colour = Color.yellow;

                debug_boxes[(y * grid_width) + x].GetComponent<SpriteRenderer>().color = colour;
            }
        }
    }

    static readonly int[] square_neighbours = { -1, 0, 0, -1, 1, 0, 0, 1 };
    static readonly int[] diag_neighbours = { -1, 0, -1, -1, 0, -1, 1, -1, 1, 0, 1, 1, 0, 1, -1, 1 };
    static readonly int[] neighbours = diag_neighbours;

    public List<WaypointNode> find_path(int start_x, int start_y, int end_x, int end_y, float timeout_ms = 16)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        List<WaypointNode> open_list = new List<WaypointNode>();
        List<WaypointNode> closed_list = new List<WaypointNode>();
        List<WaypointNode> path = new List<WaypointNode>();

        WaypointNode start_node = get_node(start_x, start_y);
        WaypointNode end_node = get_node(end_x, end_y);
        if (start_node == null || !start_node.walkable) {
            Debug.LogVerbose("could not find path: start node is not walkable.");
            return path;
        }else if (end_node == null || !end_node.walkable) {
            Debug.LogVerbose("could not find path: end node is not walkable.");
            return path;
        }else if (start_node == end_node) {
            Debug.LogVerbose("empty path: start node equals end node");
            return path;
        }
        closed_list.Add(start_node);
        start_node.closed = true;

        WaypointNode close_node = start_node;

        while (true)
        {
            if ((watch.ElapsedTicks / 10000.0f) >= timeout_ms)
            {
                Debug.LogVerbose("Find path timeout after " + (watch.ElapsedTicks / 10000.0f) + " seconds");
                break;
            }

            for (int n = 0; n < neighbours.Length; n += 2)
            {
                int x = (int)close_node.grid_pos.x + neighbours[n];
                int y = (int)close_node.grid_pos.y + neighbours[n + 1];

                if (x < 0 || x >= grid_width) continue;
                if (y < 0 || y >= grid_height) continue;

                WaypointNode node = get_node(x, y);
                if (!node.walkable || node.closed) continue;

                int h = (int)(Mathf.Abs(end_x - node.grid_pos.x) + Mathf.Abs(end_y - node.grid_pos.y));
                if (!node.open)
                {
                    node.parent = close_node;
                    node.g = node.parent.g + (n % 4 == 2 ? 2 : 1);
                    node.h = h;
                    node.f = node.g + node.h;
                    open_list.Add(node);
                    node.open = true;
                }else {
                    if (node.g < close_node.g)
                    {
                        node.parent = close_node;
                        node.g = node.parent.g + (n % 4 == 2 ? 2 : 1);
                        node.f = node.g + node.h;
                    }
                }
            }

            if (open_list.Count == 0)
            {
                Debug.LogVerbose("no path can be found");
                break;
            }

            //finds lowest cost node sorted by most recently added
            close_node = null;
            float lowf = float.MaxValue;
            for (int i = open_list.Count - 1; i > -1; --i)
            {
                WaypointNode n = open_list[i];
                if (n.f < lowf)
                {
                    lowf = n.f;
                    close_node = n;
                }
            }
            open_list.Remove(close_node);
            closed_list.Add(close_node);
            close_node.closed = true;
            close_node.open = false;

            if (close_node == end_node)
            {
                WaypointNode parent = close_node;
                while (parent != null)
                {
                    if (path.Find(item => item == parent) != null) {
                        Debug.LogError("infinite loop parent detected (should not happen)");
                        break;
                    }
					path.Insert(0, parent);
					if (parent == start_node) break;
					parent = parent.parent;
                }
                break;
            }
        }

        foreach (WaypointNode n in closed_list)
        {
            n.closed = false;
        }
        foreach (WaypointNode n in open_list)
        {
            n.open = false;
        }

        watch.Stop();
        Debug.LogVerbose("took " + (watch.ElapsedTicks / 10000.0f) + " ms to find path");

        return path;
    }

    public List<WaypointNode> find_path(Vector2 world_pos_start, Vector2 world_pos_end, float timeout_ms = 16)
    {
        return find_path(worldx_to_gridx(world_pos_start.x), worldy_to_gridy(world_pos_start.y), 
                         worldx_to_gridx(world_pos_end.x),   worldy_to_gridy(world_pos_end.y), timeout_ms);
    }

    public List<WaypointNode> find_path(WaypointNode start_node, WaypointNode end_node, float timeout_ms = 16)
    {
        return find_path((int)start_node.grid_pos.x, (int)start_node.grid_pos.y,
                         (int)end_node.grid_pos.x, (int)end_node.grid_pos.y, timeout_ms);
    }

    public int worldx_to_gridx(float x)
    {
        x = Mathf.Clamp(x, waypoint_node_start.x, (waypoint_bg_bounds.max.x - Debug.config.grid_point_sep - 1)) - waypoint_node_start.x;
        x /= Debug.config.grid_point_sep;
        return (int)x;
    }

    public int worldy_to_gridy(float y)
    {
        y = waypoint_node_start.y - Mathf.Clamp(y, (waypoint_bg_bounds.min.y + Debug.config.grid_point_sep + 1), waypoint_node_start.y);
        y /= Debug.config.grid_point_sep;
        return (int)y;
    }

    public Vector2 world_to_grid(Vector2 world_pos)
    {
        world_pos.x = worldx_to_gridx(world_pos.x);
        world_pos.y = worldy_to_gridy(world_pos.y);
        return world_pos;
    }

    public Vector2 grid_to_world(Vector2 grid_pos)
    {
        return get_node(grid_pos).world_pos;
    }

    public WaypointNode find_neighbour_node(WaypointNode start_node)
    {
        if (start_node.walkable) return start_node;
        for (int n = 0; n < neighbours.Length; n += 2)
        {
            int x = (int)start_node.grid_pos.x + neighbours[n];
            int y = (int)start_node.grid_pos.y + neighbours[n + 1];

            if (x < 0 || x >= grid_width) continue;
            if (y < 0 || y >= grid_height) continue;

            WaypointNode node = get_node(x, y);
            if (node.walkable) return node;
        }
        return start_node;
    }

    /*
    * Returns a waypoint node from the grid if x and y is in bounds, if one of them is not, then return null
    */
    public WaypointNode get_node(int _x, int _y)
    {
        if (_x >= grid_width || _x < 0)
        {
            Debug.LogError(_x + " is out of bounds of grid width (" + grid_width + ")");
            return null;
        }
        if (_y >= grid_height || _y < 0)
        {
            Debug.LogError(_y + " is out of bounds of grid height (" + grid_height + ")");
            return null;
        }
        return waypoint_nodes[(_y * grid_width) + _x];
    }

    /*
    * Returns a waypoint node from the grid if x and y is in bounds, if one of them is not, then return null
    */
    public WaypointNode get_node(float _x, float _y)
    {
        return get_node((int)_x, (int)_y);
    }

    /*
    * Returns a waypoint node from the grid if x and y is in bounds, if one of them is not, then return null
    */
    public WaypointNode get_node(Vector3 v)
    {
        return get_node((int)v.x, (int)v.y);
    }
}
