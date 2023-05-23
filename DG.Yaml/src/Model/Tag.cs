namespace DG.Yaml.Model
{
    public class Tag
    {
        public static Tag Sequence { get; } = new Tag("tag:yaml.org,2002:seq");
        public static Tag Map { get; } = new Tag("tag:yaml.org,2002:map");
        public static Tag Null { get; } = new Tag("tag:yaml.org,2002:null");
        public static Tag Bool { get; } = new Tag("tag:yaml.org,2002:bool");
        public static Tag Int { get; } = new Tag("tag:yaml.org,2002:int");
        public static Tag Float { get; } = new Tag("tag:yaml.org,2002:float");
        public static Tag String { get; } = new Tag("tag:yaml.org,2002:str");
        public static Tag Timestamp { get; } = new Tag("tag:yaml.org,2002:timestamp");

        public string Name { get; set; }

        public Tag(string name)
        {
            Name = name;
        }
    }
}
