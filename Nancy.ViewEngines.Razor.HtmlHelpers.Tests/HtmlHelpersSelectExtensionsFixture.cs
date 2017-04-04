using Nancy.ViewEngines.Razor.HtmlHelpersUnofficial;

namespace Nancy.ViewEngines.Razor.HtmlHelpers.Tests
{
    using System;

    using Xunit;

    public class HtmlHelpersSelectExtensionsFixture
    {
        private TestModel model;
        private HtmlHelpers<TestModel> helpers;
        private string defaultOption;

        public HtmlHelpersSelectExtensionsFixture()
        {
            this.model = new TestModel { TestEnum = SelectListItemExtensionsFixture.TestEnum.Two,
                TestNested = new SelectListItemExtensionsFixture.TestNestedModel {LevelTwo = new SelectListItemExtensionsFixture.LevelTwo { LevelTwoGuid = Guid.NewGuid() } },
                TestDisplayName = new SelectListItemExtensionsFixture.TestDisplayNameModel { TestDisplayNameModelLevelTwo = new SelectListItemExtensionsFixture.TestDisplayNameModelLevelTwo
                {
                    WithDisplayName = "WithDisplayName",
                    WithoutDisplayName = "WithoutDisplayName"
                },
                IdWithDisplayName = Guid.NewGuid(),
                IdWithoutDisplayName = Guid.NewGuid()
            }
            };
            this.helpers = new HtmlHelpers<TestModel>(null, null, model);
            this.defaultOption = SelectListItemExtensionsFixture.TestEnum.Three.ToString();
        }

        [Fact]
        public void When_enum_provided_with_selected_value_value_is_selected_in_markup()
        {
            var items = model.TestEnum.ToSelectListItems(model.TestEnum);

            var output = helpers.DropDownListFor(x => x.TestEnum, defaultOption, items);

            Assert.Contains("<option selected=\"selected\" value=\"Two\">Two</option>", output.ToHtmlString());
        }

        [Fact]
        public void When_enum_provided_items_generated_from_enum()
        {
            var output = helpers.DropDownListFor(x => x.TestEnum, new {  });

            Assert.Contains("<select id=\"TestEnum\" name=\"TestEnum\">", output.ToHtmlString());
            Assert.Contains("<option selected=\"selected\" value=\"Two\">Two</option>", output.ToHtmlString());
        }

        [Fact]
        public void When_provided_items_generated_from_nested()
        {
            var output = helpers.DropDownListFor(x => x.TestNested.LevelTwo.LevelTwoGuid, model.TestNested.LevelTwo.LevelTwoListItems, new { });

            Assert.Contains("<select id=\"TestNested_LevelTwo_LevelTwoGuid\" name=\"TestNested.LevelTwo.LevelTwoGuid\">", output.ToHtmlString());
            Assert.Contains("<option value=\"LEVEL_TWO_VALUE\">LEVEL_TWO_TEXT</option>", output.ToHtmlString());
        }

        [Fact]
        public void When_provided_item_with_displayname_label()
        {
            var output = helpers.LabelFor(x => x.TestDisplayName.IdWithDisplayName, new { });
            var outputLvlTwo = helpers.LabelFor(x => x.TestDisplayName.TestDisplayNameModelLevelTwo.WithDisplayName, new { });

            Assert.Contains("<label for=\"TestDisplayName.IdWithDisplayName\">WithDisplayName</label>", output.ToHtmlString());
            Assert.Contains("<label for=\"TestDisplayName.TestDisplayNameModelLevelTwo.WithDisplayName\">WithDisplayName</label>", outputLvlTwo.ToHtmlString());

        }

        [Fact]
        public void When_provided_item_without_displayname_label()
        {
            var output = helpers.LabelFor(x => x.TestDisplayName.IdWithoutDisplayName, new { });
            var outputLvlTwo = helpers.LabelFor(x => x.TestDisplayName.TestDisplayNameModelLevelTwo.WithoutDisplayName, new { });

            Assert.Contains("<label for=\"TestDisplayName.IdWithoutDisplayName\">TestDisplayName.IdWithoutDisplayName</label>", output.ToHtmlString());
            Assert.Contains("<label for=\"TestDisplayName.TestDisplayNameModelLevelTwo.WithoutDisplayName\">TestDisplayName.TestDisplayNameModelLevelTwo.WithoutDisplayName</label>", outputLvlTwo.ToHtmlString());
        }

        public class TestModel
        {
            public SelectListItemExtensionsFixture.TestEnum TestEnum { get; set; }
            public SelectListItemExtensionsFixture.TestNestedModel TestNested { get; set; }
            public SelectListItemExtensionsFixture.TestDisplayNameModel TestDisplayName { get; set; }
        }
    }
}
