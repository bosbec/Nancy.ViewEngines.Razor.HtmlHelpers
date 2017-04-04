using Nancy.ViewEngines.Razor.HtmlHelpersUnofficial;

namespace Nancy.ViewEngines.Razor.HtmlHelpers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class LabelExtensionsFixture
    {
     
        public class TestDisplayNameNestedModel
        {
            [DisplayName("WithDisplayName")]
            public Guid IdWithDisplayName { get; set; }

            public Guid IdWithoutDisplayName { get; set; }

            public TestDisplayNameNestedModelLevelTwo TestDisplayNameModelLevelTwo { get; set; }
        }

        public class TestDisplayNameNestedModelLevelTwo
        {
            [DisplayName("WithDisplayName")]
            public string WithDisplayName { get; set; }

            public string WithoutDisplayName { get; set; }
        }
    }
}
