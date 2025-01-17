using Strumenta.Sharplasu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strumenta.Entity.Semantics
{
    public interface IType {
        public string Name { get; }
    }

    internal static class Typing
    {
        private static Dictionary<Node, IType> memory = new Dictionary<Node, IType>();

        public static void SetType(Node node, IType type)
        {
            if (type == null)
            {
                memory.Remove(node);
            }
            else
            {
                memory[node] = type;
            }
        }

        public static IType GetType(Node node)
        {
            IType type;
            memory.TryGetValue(node, out type);
            return type;
        }
    }

    public class UnitType : Node, IType {
        public string Name { 
            get => "Unit"; 
         }        
    }

    public static class NodeExtensions
    {
        public static IType GetTypeSemantics(this Node node)
        {
            return Typing.GetType(node);
        }

        public static void SetTypeSemantics(this Node node, IType value)
        {
            Typing.SetType(node, value);
        }
    }
}