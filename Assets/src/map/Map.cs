using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Map
{
    public static WaypointGrid grid;

    public static void init()
    {
        grid = new WaypointGrid();
    }
    
    public static void update()
    {
        grid.update();
    }
}
