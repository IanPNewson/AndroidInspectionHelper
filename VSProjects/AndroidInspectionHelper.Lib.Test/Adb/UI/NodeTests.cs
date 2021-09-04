using Android.Lib.Adb.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace AndroidLib.Test.Adb.UI
{
    public class NodeTests
    {

        [Fact]
        public void ChildrenParseTest()
        {
            var hierarchy = SampleHierarchy;
            Assert.Single(hierarchy.Children);
        }

        [Fact]
        public void Index()
        {
            var node = RootNode;
            Assert.Equal(123, node.Index);
        }

        [Fact]
        public void ResourceId()
        {
            var node = RootNode;
            Assert.Equal("the id", node.Id);
        }

        [Fact]
        public void Text()
        {
            var node = RootNode;
            Assert.Equal("the text", node.Text);
        }

        [Fact]
        public void Class()
        {
            var node = RootNode;
            Assert.Equal("the class", node.Class);
        }

        [Fact]
        public void Package()
        {
            var node = RootNode;
            Assert.Equal("the package", node.Package);
        }

        [Fact]
        public void ContentDescription()
        {
            var node = RootNode;
            Assert.Equal("the content description", node.ContentDescription);
        }

        [Fact]
        public void Checkable()
        {
            var node = RootNode;
            Assert.True(node.Checkable);
        }

        [Fact]
        public void Checked()
        {
            var node = RootNode;
            Assert.True(node.Checked);
        }

        [Fact]
        public void Clickable()
        {
            var node = RootNode;
            Assert.True(node.Clickable);
        }

        [Fact]
        public void Enabled()
        {
            var node = RootNode;
            Assert.True(node.Enabled);
        }

        [Fact]
        public void Focusable()
        {
            var node = RootNode;
            Assert.True(node.Focusable);
        }

        [Fact]
        public void Scrollable()
        {
            var node = RootNode;
            Assert.True(node.Scrollable);
        }

        [Fact]
        public void Bounds()
        {
            var node = RootNode;
            Assert.Equal(new Rectangle(1,2,2,2), node.Bounds);
        }

        [Fact]
        public void LongClickable()
        {
            var node = RootNode;
            Assert.True(node.LongClickable);
        }

        [Fact]
        public void Password()
        {
            var node = RootNode;
            Assert.True(node.Password);
        }

        [Fact]
        public void Selected()
        {
            var node = RootNode;
            Assert.True(node.Selected);
        }

        #region Sample data

        protected Node RootNode { get => SampleHierarchy.Children.Single(); }

        protected Hierarchy SampleHierarchy
        {
            get
            {
                return new Hierarchy(SampleXDoc.Root);
            }
        }

        protected XDocument SampleXDoc
        {
            get
            {
                return XDocument.Parse(TestData.HierarchyXml);
            }
        }

        #endregion

    }
}
