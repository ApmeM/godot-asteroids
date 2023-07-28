using Godot;

namespace GodotRts.Presentation.Utils
{
    public static class TileMapExt
    {
        public static Vector2 ArrangeGlobalPositionToCell(this TileMap tileMap, Vector2 position)
        {
            var cell = tileMap.GlobalToMap(position);
            return tileMap.MapToGlobal(cell);
        }

        public static Vector2 GlobalToMap(this TileMap tileMap, Vector2 world)
        {
            var localPosition = tileMap.ToLocal(world);
            return tileMap.WorldToMap(localPosition);

        }

        public static Vector2 MapToGlobal(this TileMap tileMap, Vector2 map)
        {
            var localPosition = tileMap.MapToWorld(map);
            return tileMap.ToGlobal(localPosition);
        }


        public static System.Numerics.Vector2 MapToWorld(this TileMap tileMap, System.Numerics.Vector2 v)
        {
            var gv = new Vector2(v.X,v.Y);
            var rgv = tileMap.MapToWorld(gv);
            var rv = new System.Numerics.Vector2(rgv.x, rgv.y);
            return rv;
        }

        public static System.Numerics.Vector2 WorldToMap(this TileMap tileMap, System.Numerics.Vector2 v)
        {
            var gv = new Vector2(v.X,v.Y);
            var rgv = tileMap.WorldToMap(gv);
            var rv = new System.Numerics.Vector2(rgv.x, rgv.y);
            return rv;
        }
    }
}
