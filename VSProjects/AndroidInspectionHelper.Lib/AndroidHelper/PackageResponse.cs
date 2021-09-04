using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;

namespace Android.Lib.AndroidHelper
{
    public class PackageResponse
    {
        [JsonProperty("packageId")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("defaultActivity")]
        public ActivityResponse DefaultActivity { get; set; }
    }

    public class ActivityResponse
    {

        [JsonProperty("name")]
        public string Name { get; set; }

    }

    public class ViewResponse
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("className")]
        public string ClassName { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("inputType")]
        public int InputType { get; set; }

        [JsonProperty("isAccessibilityFocused")]
        public bool IsAccessibilityFocused { get; set; }

        [JsonProperty("isCheckable")]
        public bool IsCheckable { get; set; }

        [JsonProperty("isChecked")]
        public bool IsChecked { get; set; }

        [JsonProperty("isClickable")]
        public bool IsClickable { get; set; }

        [JsonProperty("isContentInvalid")]
        public bool IsContentInvalid { get; set; }

        [JsonProperty("isContextClickable")]
        public bool IsContextClickable { get; set; }

        [JsonProperty("isDismissable")]
        public bool IsDismissable { get; set; }

        [JsonProperty("isEditable")]
        public bool IsEditable { get; set; }

        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("isHeading")]
        public bool IsHeading { get; set; }

        [JsonProperty("contentDescription")]
        public string ContentDescription { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("hintText")]
        public string HintText { get; set; }

        [JsonProperty("maxTextLength")]
        public int MaxTextLength { get; set; }

        [JsonProperty("packageName")]
        public string PackageName { get; set; }

        [JsonProperty("isScrollable")]
        public bool IsScrollable { get; set; }

        [JsonProperty("tooltipText")]
        public string TooltipText { get; set; }

        [JsonProperty("boundsInParent")]
        public RectResponse BoundsInParent { get; set; }

        [JsonProperty("boundsInScreen")]
        public RectResponse BoundsInScreen { get; set; }

        [JsonProperty("paneTitle")]
        public string PaneTitle { get; set; }

        [JsonProperty("children")]
        public List<ViewResponse> Children { get; set; } = new List<ViewResponse>();

    }

    public class RectResponse
    {
        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        public static implicit operator Rectangle(RectResponse rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }

    public class ViewChangeResult
    {

        [JsonProperty("changeTypes")]
        public List<string>  ChangeTypes { get; set; }

        [JsonProperty("viewResponses")]
        public Dictionary<string,ViewResponse> ViewResponses { get; set; }
    }

}
