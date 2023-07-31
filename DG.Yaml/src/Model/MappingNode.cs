using DG.Common.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace DG.Yaml.Model
{
    public class MappingNode : Node
    {
        public KeyValuePair<string, Node>[] Nodes { get; set; }

        public IEnumerable<string> Keys => Nodes.Select(n => n.Key);

        public Node this[string index]
        {
            get
            {
                ThrowIf.Collection(Nodes, nameof(Nodes)).None(n => n.Key == index, $"No node with tag {index} could be found in this mapping.");
                return Nodes.Single(n => n.Key == index).Value;
            }
        }
    }
}
