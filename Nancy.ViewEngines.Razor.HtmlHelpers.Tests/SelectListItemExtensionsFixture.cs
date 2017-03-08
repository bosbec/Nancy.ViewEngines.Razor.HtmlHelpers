using Nancy.ViewEngines.Razor.HtmlHelpersUnofficial;

namespace Nancy.ViewEngines.Razor.HtmlHelpers.Tests
{
    using System;
    using System.Collections.Generic;

    using Xunit;
    using System.Linq;

    public class SelectListItemExtensionsFixture
    {
        private TestEnum field;

        [Fact]
        public void Should_create_list_items_from_enum()
        {
            var items = field.ToSelectListItems().ToList();
            
            Assert.Equal(items[0].Text, "One");
            Assert.Equal(items[1].Text, "Two");
            Assert.Equal(items[2].Text, "Three");
        }

        [Fact]
        public void Selected_item_should_have_attribute_set()
        {
            var items = field.ToSelectListItems(TestEnum.Three).ToList();
            
            Assert.Equal(items[2].Text, "Three");
            Assert.Equal(items[2].Selected, true);
        }

        public enum TestEnum
        {
            One, Two, Three
        }

        public class TestNestedModel
        {
            public LevelTwo LevelTwo = new LevelTwo();
        }

        public class LevelTwo
        {
            public Guid LevelTwoGuid = Guid.NewGuid();

            public IEnumerable<SelectListItem> LevelTwoListItems = new[] { new SelectListItem { Value = "LEVEL_TWO_VALUE", Text = "LEVEL_TWO_TEXT" } };
        }
    }
}
