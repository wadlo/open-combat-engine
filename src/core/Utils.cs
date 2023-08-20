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
    }
}
