using Godot;
using Godot.Collections;

namespace OpenTDE
{
    public class Utils
    {
        public static Array<T> GetChildrenOfType<[MustBeVariant] T>(Node parent)
            where T : Node
        {
            Array<T> items = new Array<T>();
            foreach (Node child in parent.GetChildren())
            {
                if (child is T)
                {
                    items.Add(child as T);
                }
            }

            return items;
        }

        public static Node2D GetClosestNodeInGroup(SceneTree tree, Vector2 position, string group)
        {
            Array<Node> nodes = tree.GetNodesInGroup(group);

            Node2D closest = null;
            if (nodes.Count > 0)
            {
                float closestDist = float.MaxValue;
                foreach (Node node in nodes)
                {
                    float distSqr = ((node as Node2D).Position - position).LengthSquared();
                    if (distSqr < closestDist)
                    {
                        closestDist = distSqr;
                        closest = node as Node2D;
                    }
                }
            }

            return closest;
        }

        // Maps a float that is between [fromMin, fromMax] to the range [toMin, toMax].
        // For example, MapFloat(10,20, 0,10, 15) => 5
        public static float MapFloat(
            float fromMin,
            float fromMax,
            float toMin,
            float toMax,
            float fromValue
        )
        {
            float percent = (fromValue - fromMin) / (fromMax - fromMin);
            return toMin + (toMax - toMin) * percent;
        }
    }
}
