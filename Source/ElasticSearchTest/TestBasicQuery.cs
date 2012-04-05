using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchSharp.Query;
using System.IO;
using ElasticSearchSharp;
using ElasticSearchSharp.Query.Queries;

namespace ElasticSearchTest
{
    [TestClass]
    public class TestBasicQuery
    {
        //[TestMethod]
        public void TestTextQuery()
        {
            var conn = new ElasticSearchSharp.ElasticSearchConnection();
            var collection = new ElasticSearchSharp.ElasticSearchCollection<TestObject>(conn);


            collection.Save("hello", new TestObject() { Name = "hello", Text = "Hello" });
            collection.Save("world", new TestObject() { Name = "world", Text = "world" });

            ElasticSearchQuery query = new ElasticSearchQuery();

            query.Query = new TextQuery()
            {
                Operator = BooleanOperator.And,
                Type = TextQueryType.Phrase,
                Fields = new Dictionary<string, string> 
                { 
                    {"Text", "hello"},
                    {"Name", "hello"}
                 }
            };

            var items = collection.Find(query);
            Assert.IsTrue(items.Any());
            foreach (var item in items)
            {
                Assert.IsNotNull(item);
            }

        }


        //[TestMethod]
        public void TestBoolQuery()
        {
            var conn = new ElasticSearchSharp.ElasticSearchConnection();
            var collection = new ElasticSearchSharp.ElasticSearchCollection<TestObject>(conn);


            ElasticSearchQuery query = new ElasticSearchQuery();
            var boolQuery = new BoolQuery
            {
                Must = new MatchAllQuery(),
                MustNot = new TextQuery()
                 {
                     Fields = new Dictionary<string, string>(){
                             {"Text", "hi"}
                         }
                 }


            };

            query.Query = boolQuery;

            var items = collection.Find(query);
            Assert.IsTrue(items.Any());
            foreach (var item in items)
            {
                Assert.IsNotNull(item);
            }

        }

        [TestMethod]
        public void TestMatchAllQuery()
        {
            var conn = new ElasticSearchSharp.ElasticSearchConnection();
            var collection = new ElasticSearchSharp.ElasticSearchCollection<TestObject>(conn);


            ElasticSearchQuery query = new ElasticSearchQuery();
            query.Query = new MatchAllQuery();

            var items = collection.Find(query);
            Assert.IsTrue(items.Any());
            foreach (var item in items)
            {
                Assert.IsNotNull(item);
            }

        }

        [TestMethod]
        public void TestIdsQuery()
        {
            var conn = new ElasticSearchSharp.ElasticSearchConnection();
            var collection = new ElasticSearchSharp.ElasticSearchCollection<TestObject>(conn);


            ElasticSearchQuery query = new ElasticSearchQuery();


            query.Query = new IdsQuery() { Ids = new List<string>(new [] {"vlad", "john"}) };

            var items = collection.Find(query);
            Assert.IsTrue(items.Any());
            foreach (var item in items)
            {
                Assert.IsNotNull(item);
            }

        }


    }
}
