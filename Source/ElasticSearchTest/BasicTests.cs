using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchTest
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void TestSave()
        {
            var conn = new ElasticSearchSharp.ElasticSearchConnection();
            var collection = new ElasticSearchSharp.ElasticSearchCollection<TestObject>(conn);


            var result = collection.Save("vlad", new TestObject() { Name = "vlad", Text = "hello world", Child = new TestObject { Name="john", Text="hello world 2" } });
            Assert.IsTrue(result.Success);

            var deleteResult = collection.Remove("vlad");
            Assert.IsTrue(deleteResult.Success);
            Assert.IsTrue(deleteResult.Found);
        }

        [TestMethod]
        public void TestGetById()
        {
            var conn = new ElasticSearchSharp.ElasticSearchConnection();
            var collection = new ElasticSearchSharp.ElasticSearchCollection<TestObject>(conn);


            var result = collection.Save("vlad", new TestObject() { Name = "vlad", Text = "hello world", Child = new TestObject { Name = "john", Text = "hello world 2" } });
            Assert.IsTrue(result.Success);

            var obj = collection.GetById("vlad");
            Assert.AreEqual(obj.Name, "vlad");


            var deleteResult = collection.Remove("vlad");
            Assert.IsTrue(deleteResult.Success);
            Assert.IsTrue(deleteResult.Found);

        }

        [TestMethod]
        public void TestSearchChild()
        {
            var conn = new ElasticSearchSharp.ElasticSearchConnection();
            var collection = new ElasticSearchSharp.ElasticSearchCollection<TestObject>(conn);


            var result = collection.Save("vlad", new TestObject() { Name = "vlad", Text = "hello world", Child = new TestObject { Name = "john", Text = "hello world 2" } });
            Assert.IsTrue(result.Success);


            var query = new
            {
                query = new
                {
                    text = new Dictionary<string, object> {
                        { "Child.Text", "hello" } 
                    }
                }
            };

            var objCollection = collection.Find(query);
        }
    }

    public class TestObject
    {
        public string Name { get; set; }
        public TestObject Child { get; set; }
        public string Text { get; set; }
    }
}
