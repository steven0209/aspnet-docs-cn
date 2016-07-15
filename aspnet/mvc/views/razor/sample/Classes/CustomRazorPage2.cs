using Microsoft.AspNetCore.Mvc.Razor;

namespace RazorSample.Classes
{
    public abstract class CustomRazorPage<TModel> : RazorPage<TModel>
    {
        public string CustomText { get; } = "Hello model and custom.";
    }

}
