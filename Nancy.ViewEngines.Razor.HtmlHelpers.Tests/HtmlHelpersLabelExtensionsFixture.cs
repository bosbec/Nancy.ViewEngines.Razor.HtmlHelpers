using Nancy.ViewEngines.Razor.HtmlHelpersUnofficial;

namespace Nancy.ViewEngines.Razor.HtmlHelpers.Tests
{
    using System;

    using Xunit;

    public class HtmlHelpersLabelExtensionsFixture
    {
        private LabelTestModel model;
        private HtmlHelpers<LabelTestModel> helpers;
        private string defaultOption;

        public HtmlHelpersLabelExtensionsFixture()
        {
            this.model = new LabelTestModel
            { TestDisplayName = new SelectListItemExtensionsFixture.TestDisplayNameNestedModel {
                    TestDisplayNameModelLevelTwo = new SelectListItemExtensionsFixture.TestDisplayNameNestedModelLevelTwo
                    {
                    WithDisplayName = "WithDisplayName",
                    WithoutDisplayName = "WithoutDisplayName"
                },
                IdWithDisplayName = Guid.NewGuid(),
                IdWithoutDisplayName = Guid.NewGuid()
                }
            };
            this.helpers = new HtmlHelpers<LabelTestModel>(null, null, model);
            this.defaultOption = SelectListItemExtensionsFixture.TestEnum.Three.ToString();
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

        public class LabelTestModel
        {
            public SelectListItemExtensionsFixture.TestDisplayNameNestedModel TestDisplayName { get; set; }
        }
    }
}
