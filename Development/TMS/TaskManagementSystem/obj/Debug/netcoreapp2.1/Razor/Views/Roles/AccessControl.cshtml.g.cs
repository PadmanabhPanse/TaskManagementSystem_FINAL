#pragma checksum "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a94597899f11a02858c4815b7fb54e54ed0aaee6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Roles_AccessControl), @"mvc.1.0.view", @"/Views/Roles/AccessControl.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Roles/AccessControl.cshtml", typeof(AspNetCore.Views_Roles_AccessControl))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\_ViewImports.cshtml"
using TaskManagementSystem.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a94597899f11a02858c4815b7fb54e54ed0aaee6", @"/Views/Roles/AccessControl.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bd4fddfc24a1160bfa02d517b0e02ead34e70a12", @"/_ViewImports.cshtml")]
    public class Views_Roles_AccessControl : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TaskManagementSystem.Models.AccessControlViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AssignRoles", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/assets/js/core/jquery.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
  
    ViewData["Title"] = "Access Control";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(151, 328, true);
            WriteLiteral(@"
<div class=""row"">
    <div class=""col-md-12"">
        <div class=""card"">
            <div class=""card-header card-header-info"">
                <h4 class=""card-title "">Access Control</h4>
                <p class=""card-category""> Assign Pages to Roles</p>
            </div>
            <div class=""card-body"">
                ");
            EndContext();
            BeginContext(479, 4485, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d7d126711e1645069ec5c0ea4b2f0c3e", async() => {
                BeginContext(524, 190, true);
                WriteLiteral("\n                    <div class=\"table-responsive\">\n                        <table class=\"table\">\n                            <thead class=\" text-info\">\n                                <tr>\n");
                EndContext();
#line 20 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                     if (Model != null && Model.roles.Any())
                                    {
                                        

#line default
#line hidden
#line 22 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                         foreach (var role in Model.roles)
                                        {

#line default
#line hidden
                BeginContext(946, 97, true);
                WriteLiteral("                                            <th>\n                                                ");
                EndContext();
                BeginContext(1044, 4, false);
#line 25 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                           Write(role);

#line default
#line hidden
                EndContext();
                BeginContext(1048, 51, true);
                WriteLiteral("\n                                            </th>\n");
                EndContext();
#line 27 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                        }

#line default
#line hidden
#line 27 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                         
                                    }
                                    else
                                    {

#line default
#line hidden
                BeginContext(1258, 150, true);
                WriteLiteral("                                        <th>\n                                            No Roles added\n                                        </th>\n");
                EndContext();
#line 34 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                    }

#line default
#line hidden
                BeginContext(1446, 112, true);
                WriteLiteral("                                </tr>\n                            </thead>\n                            <tbody>\n\n");
                EndContext();
#line 39 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                 if (Model != null && Model.menuItems.Any())
                                {
                                    for (int i = 0; i < Model.menuItems.Count; i++)
                                    {

#line default
#line hidden
                BeginContext(1791, 45, true);
                WriteLiteral("                                        <tr>\n");
                EndContext();
#line 44 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                             if (Model != null && Model.roles.Any())
                                            {
                                                

#line default
#line hidden
#line 46 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                 foreach (var role in Model.roles)
                                                {

#line default
#line hidden
                BeginContext(2100, 113, true);
                WriteLiteral("                                                    <td>\n                                                        ");
                EndContext();
                BeginContext(2214, 56, false);
#line 49 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                   Write(Html.Hidden($"menuItems[{i}].Id", Model.menuItems[i].Id));

#line default
#line hidden
                EndContext();
                BeginContext(2270, 57, true);
                WriteLiteral("\n                                                        ");
                EndContext();
                BeginContext(2328, 76, false);
#line 50 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                   Write(Html.Hidden($"menuItems[{i}].ParentMenuId", Model.menuItems[i].ParentMenuId));

#line default
#line hidden
                EndContext();
                BeginContext(2404, 57, true);
                WriteLiteral("\n                                                        ");
                EndContext();
                BeginContext(2462, 60, false);
#line 51 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                   Write(Html.Hidden($"menuItems[{i}].Name", Model.menuItems[i].Name));

#line default
#line hidden
                EndContext();
                BeginContext(2522, 57, true);
                WriteLiteral("\n                                                        ");
                EndContext();
                BeginContext(2580, 72, false);
#line 52 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                   Write(Html.Hidden($"menuItems[{i}].ActionName", Model.menuItems[i].ActionName));

#line default
#line hidden
                EndContext();
                BeginContext(2652, 57, true);
                WriteLiteral("\n                                                        ");
                EndContext();
                BeginContext(2710, 80, false);
#line 53 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                   Write(Html.Hidden($"menuItems[{i}].ControllerName", Model.menuItems[i].ControllerName));

#line default
#line hidden
                EndContext();
                BeginContext(2790, 57, true);
                WriteLiteral("\n                                                        ");
                EndContext();
                BeginContext(2848, 58, false);
#line 54 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                   Write(Html.Hidden($"menuItems[{i}].Url", Model.menuItems[i].Url));

#line default
#line hidden
                EndContext();
                BeginContext(2906, 57, true);
                WriteLiteral("\n                                                        ");
                EndContext();
                BeginContext(2964, 107, false);
#line 55 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                   Write(Html.Hidden($"menuItems[{i}].Roles", Model.menuItems[i].Roles, new { id = $"role{Model.menuItems[i].Id}" }));

#line default
#line hidden
                EndContext();
                BeginContext(3071, 240, true);
                WriteLiteral("\n                                                        <div class=\"form-check \">\n                                                            <label class=\"form-check-label\">\n                                                                ");
                EndContext();
                BeginContext(3312, 145, false);
#line 58 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                           Write(Html.CheckBox($"{Model.menuItems[i].Id}", Model.menuItems[i].Roles?.Split(',').Contains(role), new { @class = "form-check-input", value = role }));

#line default
#line hidden
                EndContext();
                BeginContext(3457, 332, true);
                WriteLiteral(@"
                                                                <span class=""form-check-sign"">
                                                                    <span class=""check""></span>
                                                                </span>
                                                            </label>");
                EndContext();
                BeginContext(3790, 23, false);
#line 62 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                               Write(Model.menuItems[i].Name);

#line default
#line hidden
                EndContext();
                BeginContext(3813, 122, true);
                WriteLiteral("\n                                                        </div>\n                                                    </td>\n");
                EndContext();
#line 65 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"

                                                }

#line default
#line hidden
#line 66 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                                 
                                            }
                                            else
                                            {

#line default
#line hidden
                BeginContext(4127, 77, true);
                WriteLiteral("                                                <td>No Menu Items Added</td>\n");
                EndContext();
#line 71 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                            }

#line default
#line hidden
                BeginContext(4250, 46, true);
                WriteLiteral("                                        </tr>\n");
                EndContext();
#line 73 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                    }
                                }
                                else
                                {

#line default
#line hidden
                BeginContext(4439, 233, true);
                WriteLiteral("                                    <tr>\n                                        <th>\n                                            No Roles added\n                                        </th>\n                                    </tr>\n");
                EndContext();
#line 82 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Roles\AccessControl.cshtml"
                                }

#line default
#line hidden
                BeginContext(4706, 251, true);
                WriteLiteral("\n                            </tbody>\n                        </table>\n                    </div>\n                    <button type=\"submit\" class=\"btn btn-info pull-right\">Save</button>\n                    <div class=\"clearfix\"></div>\n                ");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(4964, 55, true);
            WriteLiteral("\n            </div>\n        </div>\n    </div>\n</div>\n\n\n");
            EndContext();
            BeginContext(5019, 54, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "205a4d741c6645259a5792e882dae993", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(5073, 296, true);
            WriteLiteral(@"
<script>
    $(function () {
        $(""input:checkbox.form-check-input"").change(function () {
            $(""#role"" + $(this).attr('name')).val($('input[name=' + $(this).attr('name') + ']:checkbox:checked').map(function () { return this.value; }).get().join(','));
        });
    });
</script>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TaskManagementSystem.Models.AccessControlViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
