using Nancy.ViewEngines.Razor.HtmlHelpersUnofficial;

namespace Nancy.ViewEngines.Razor.HtmlHelpers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

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
            [DisplayName("TestName")]
            public LevelTwo LevelTwo { get; set; } =  new LevelTwo();
        }

        public class LevelTwo
        {
            public Guid LevelTwoGuid
            {
                get
                {
                    if (LevelTwoGuid == Guid.Empty)
                    {
                        LevelTwoGuid = Guid.NewGuid();
                        return LevelTwoGuid;
                    }

                    return LevelTwoGuid;
                }

                set
                {
                    LevelTwoGuid = value;
                }
            }


            public IEnumerable<SelectListItem> LevelTwoListItems = new[] { new SelectListItem { Value = "LEVEL_TWO_VALUE", Text = "LEVEL_TWO_TEXT" } };
        }

        public class TestDisplayNameModel
        {
            [DisplayName("WithDisplayName")]
            public Guid IdWithDisplayName { get; set; }

            public Guid IdWithoutDisplayName { get; set; }

            public TestDisplayNameModelLevelTwo TestDisplayNameModelLevelTwo { get; set; }

        }

        public class TestDisplayNameModelLevelTwo
        {
            [DisplayName("WithDisplayName")]
            public string WithDisplayName { get; set; }

            public string WithoutDisplayName { get; set; }
        }
    }

    
}
