using System.Collections.Generic;

namespace WebApplication
{
    public class TestObject
    {
        public int Number { get; set; }

        public string Suturingu { get; set; }

        public TestObject Foo { get; set; }

        public List<string> StringList { get; set; }

        public List<TestObject> TestList { get; set; }
    }
}