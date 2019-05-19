using System.Collections.Generic;
using System.Linq;
using lunge.Library.Entities;
using Microsoft.Xna.Framework;

namespace lunge.Library.Collisions
{
    public static class CollisionManager
    {
        /*private static List<Entity> Entities => EntityManager.Entities;

        public static bool Intersects(Entity e1, Entity e2)
        {
            if (!e1.IsSolid || !e2.IsSolid) return false;
            if (e1.CollisionRect.Intersects(e2.CollisionRect))
                return true;
            return false;
        }

        public static Entity IsIntersectingEntity(Entity with)
        {
            if (!with.IsSolid) return null;
            return Entities.FirstOrDefault(e => e.CollisionRect.Intersects(with.CollisionRect));
        }

        public static bool IsIntersectingAny(Entity with)
        {
            if (!with.IsSolid) return false;
            return Entities.Where(e => e != with).Where(e => e.IsSolid).Any(e => e.CollisionRect.Intersects(with.CollisionRect));
        }

        public static bool IsPlaceFree(Vector2 position)
        {
            var rect = new Rectangle((int)position.X, (int)position.Y, 1, 1);
            return Entities.Where(e => e.IsSolid).All(e => !e.CollisionRect.Intersects(rect));
        }

        public static bool IsPlaceFree(Vector2 position, Entity obj)
        {
            var rect = new Rectangle((int)position.X, (int)position.Y, 1, 1);
            return Entities.Where(e => e != obj).Where(e => e.IsSolid).All(e => !e.CollisionRect.Intersects(rect));
        }

        public static bool IsPlaceFree(Rectangle collisionRect, Entity obj)
        {
            return Entities.Where(e => e != obj).Where(e => e.IsSolid).All(e => !e.CollisionRect.Intersects(collisionRect));
        }

        public static bool IsPlaceFree(Vector2 position, Entity obj, out Entity intersectsWith)
        {
            intersectsWith = null;
            var rect = new Rectangle((int)position.X, (int)position.Y, 1, 1);
            foreach (var e in Entities)
            {
                if (e != obj)
                {
                    if (e.IsSolid)
                    {
                        if (e.CollisionRect.Intersects(rect))
                        {
                            intersectsWith = e;
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        public static bool IsPlaceFree(Rectangle rect, Entity obj, out Entity intersectsWith)
        {
            intersectsWith = null;
            foreach (var e in Entities)
            {
                if (e != obj)
                {
                    if (e.IsSolid)
                    {
                        if (e.CollisionRect.Intersects(rect))
                        {
                            intersectsWith = e;
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool CanMove(Vector2 to, Entity obj)
        {
            Rectangle rect = new Rectangle(
                (int)(obj.Position.X - obj.Origin.X + to.X),
                (int)(obj.Position.Y - obj.Origin.Y + to.Y),
                obj.Sprite.Width,
                obj.Sprite.Height
                );

            if (IsPlaceFree(rect, obj))
                return true;

            return false;
        }

        public static bool CanMove(Rectangle to, Entity obj)
        {
            if (IsPlaceFree(to, obj))
                return true;
            return false;
        }*/
    }
}