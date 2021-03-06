#pragma checksum "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b7d2e218b165fc90c59ee3a7bd63e03074f573c8"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b7d2e218b165fc90c59ee3a7bd63e03074f573c8", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bd4fddfc24a1160bfa02d517b0e02ead34e70a12", @"/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Dashboard>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Dashboard";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(110, 451, true);
            WriteLiteral(@"
<div class=""row"">
    <div class=""col-lg-3 col-md-6 col-sm-6"">
        <div class=""card card-stats"">
            <div class=""card-header card-header-success card-header-icon"">
                <div class=""card-icon"">
                    <i class=""material-icons"">work</i>
                </div>
                <p class=""card-category"">Completed Tasks</p>
                <h3 id=""completedTask-count"" class=""card-title"">
                    ");
            EndContext();
            BeginContext(563, 31, false);
#line 16 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                Write(Model?.CompletedTasksCount ?? 0);

#line default
#line hidden
            EndContext();
            BeginContext(595, 764, true);
            WriteLiteral(@"
                </h3>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
                    <i class=""material-icons text-success"">work</i>
                    <a href=""/tasks/completed"">View Completed Tasks</a>
                </div>
            </div>
        </div>
    </div>
    <div class=""col-lg-3 col-md-6 col-sm-6"">
        <div class=""card card-stats"">
            <div class=""card-header card-header-warning card-header-icon"">
                <div class=""card-icon"">
                    <i class=""material-icons"">work</i>
                </div>
                <p class=""card-category"">Ongoing Tasks</p>
                <h3 id=""inprogressTask-count"" class=""card-title"">
                    ");
            EndContext();
            BeginContext(1361, 32, false);
#line 35 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                Write(Model?.InprogressTasksCount ?? 0);

#line default
#line hidden
            EndContext();
            BeginContext(1394, 756, true);
            WriteLiteral(@"
                </h3>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
                    <i class=""material-icons text-warning"">work</i>
                    <a href=""/tasks/Ongoing"">View Ongoing Tasks</a>
                </div>
            </div>
        </div>
    </div>
    <div class=""col-lg-3 col-md-6 col-sm-6"">
        <div class=""card card-stats"">
            <div class=""card-header card-header-danger card-header-icon"">
                <div class=""card-icon"">
                    <i class=""material-icons"">work</i>
                </div>
                <p class=""card-category"">Overdue Tasks</p>
                <h3 id=""overdueTask-count"" class=""card-title"">
                    ");
            EndContext();
            BeginContext(2152, 29, false);
#line 54 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                Write(Model?.OverDueTasksCount ?? 0);

#line default
#line hidden
            EndContext();
            BeginContext(2182, 751, true);
            WriteLiteral(@"
                </h3>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
                    <i class=""material-icons text-danger"">work</i>
                    <a href=""/tasks/Pending"">View Overdue Tasks</a>
                </div>
            </div>
        </div>
    </div>
    <div class=""col-lg-3 col-md-6 col-sm-6"">
        <div class=""card card-stats"">
            <div class=""card-header card-header-info card-header-icon"">
                <div class=""card-icon"">
                    <i class=""material-icons"">work</i>
                </div>
                <p class=""card-category"">Closed Tasks</p>
                <h3 id=""closedTask-count"" class=""card-title"">
                    ");
            EndContext();
            BeginContext(2935, 28, false);
#line 73 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                Write(Model?.ClosedTasksCount ?? 0);

#line default
#line hidden
            EndContext();
            BeginContext(2964, 1539, true);
            WriteLiteral(@"
                </h3>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
                    <i class=""material-icons text-info"">work</i>
                    <a href=""/tasks/Closed"">View Closed Tasks</a>
                </div>
            </div>
        </div>
    </div>
</div>
<div class=""row"">
    <div class=""col-md-3"">
        <div class=""card card-chart"">
            <div class=""card-header card-header-success"">
                <div class=""ct-chart"" id=""dailyCompletedTasksChart""></div>
            </div>
            <div class=""card-body"">
                <h4 class=""card-title"">Completed Tasks</h4>
                <p class=""card-category"">Last Campaign Performance</p>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
                    <i class=""material-icons text-success"">work</i>
                    <a href=""/tasks/Completed"">View Completed Tasks</a>
                </div>
            </div");
            WriteLiteral(@">
        </div>
    </div>
    <div class=""col-md-3"">
        <div class=""card card-chart"">
            <div class=""card-header card-header-warning"">
                <div class=""ct-chart"" id=""dailyOngoingTasksChart""></div>
            </div>
            <div class=""card-body"">
                <h4 class=""card-title"">Ongoing Tasks</h4>
                <p class=""card-category"">Last Campaign Performance</p>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
");
            EndContext();
            BeginContext(4595, 693, true);
            WriteLiteral(@"                    <i class=""material-icons text-warning"">work</i>
                    <a href=""/tasks/Ongoing"">View Ongoing Tasks</a>
                </div>
            </div>
        </div>
    </div>
    <div class=""col-md-3"">
        <div class=""card card-chart"">
            <div class=""card-header card-header-danger"">
                <div class=""ct-chart"" id=""dailyOverdueTasksChart""></div>
            </div>
            <div class=""card-body"">
                <h4 class=""card-title"">Overdue Tasks</h4>
                <p class=""card-category"">Last Campaign Performance</p>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
");
            EndContext();
            BeginContext(5380, 688, true);
            WriteLiteral(@"                    <i class=""material-icons text-danger"">work</i>
                    <a href=""/tasks/Pending"">View Overdue Tasks</a>
                </div>
            </div>
        </div>
    </div>
    <div class=""col-md-3"">
        <div class=""card card-chart"">
            <div class=""card-header card-header-info"">
                <div class=""ct-chart"" id=""dailyClosedTasksChart""></div>
            </div>
            <div class=""card-body"">
                <h4 class=""card-title"">Closed Tasks</h4>
                <p class=""card-category"">Last Campaign Performance</p>
            </div>
            <div class=""card-footer"">
                <div class=""stats"">
");
            EndContext();
            BeginContext(6160, 2438, true);
            WriteLiteral(@"                    <i class=""material-icons text-info"">work</i>
                    <a href=""/tasks/Closed"">View Closed Tasks</a>
                </div>
            </div>
        </div>
    </div>
</div>
<div class=""row"">
    <div class=""col-lg-6 col-md-12"">
        <div class=""card"">
            <div class=""card-header card-header-tabs card-header-primary"">
                <div class=""nav-tabs-navigation"">
                    <div class=""nav-tabs-wrapper"">
                        <span class=""nav-tabs-title"">Tasks:</span>
                        <ul class=""nav nav-tabs"" data-tabs=""tabs"">
                            <li class=""nav-item"">
                                <a class=""nav-link active"" href=""#profile"" data-toggle=""tab"">
                                    <i class=""material-icons"">work</i> Ongoing
                                    <div class=""ripple-container""></div>
                                </a>
                            </li>
                            <li class=");
            WriteLiteral(@"""nav-item"">
                                <a class=""nav-link"" href=""#messages"" data-toggle=""tab"">
                                    <i class=""material-icons"">work</i> Completed
                                    <div class=""ripple-container""></div>
                                </a>
                            </li>
                            <li class=""nav-item"">
                                <a class=""nav-link"" href=""#settings"" data-toggle=""tab"">
                                    <i class=""material-icons"">work</i> Overdue
                                    <div class=""ripple-container""></div>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class=""card-body"">
                <div class=""tab-content"">
                    <div class=""tab-pane active"" id=""profile"">
                        <table class=""table"">
                            <the");
            WriteLiteral(@"ad class=""text-warning"">
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                </tr>
                            </thead>
                            <tbody>
");
            EndContext();
#line 201 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                 if (Model != null && Model.InprogressTasks.Any())
                                {
                                    foreach (var item in Model.InprogressTasks.OrderByDescending(task => task.TaskEstimateTime).Take(5))
                                    {

#line default
#line hidden
            BeginContext(8894, 94, true);
            WriteLiteral("                                        <tr>\r\n                                            <td>");
            EndContext();
            BeginContext(8989, 11, false);
#line 206 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskId);

#line default
#line hidden
            EndContext();
            BeginContext(9000, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(9056, 13, false);
#line 207 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskName);

#line default
#line hidden
            EndContext();
            BeginContext(9069, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(9125, 41, false);
#line 208 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskStartDate.ToString("dd.MM.yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(9166, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(9222, 50, false);
#line 209 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskEstimateTime.Value.ToString("dd.MM.yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(9272, 54, true);
            WriteLiteral("</td>\r\n                                        </tr>\r\n");
            EndContext();
#line 211 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                    }
                                }

#line default
#line hidden
            BeginContext(9400, 627, true);
            WriteLiteral(@"                            </tbody>
                        </table>
                    </div>
                    <div class=""tab-pane"" id=""messages"">
                        <table class=""table"">
                            <thead class=""text-warning"">
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                </tr>
                            </thead>
                            <tbody>
");
            EndContext();
#line 227 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                 if (Model != null && Model.CompletedTasks.Any())
                                {
                                    foreach (var item in Model.CompletedTasks.OrderByDescending(task => task.TaskEstimateTime).Take(5))
                                    {

#line default
#line hidden
            BeginContext(10321, 94, true);
            WriteLiteral("                                        <tr>\r\n                                            <td>");
            EndContext();
            BeginContext(10416, 11, false);
#line 232 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskId);

#line default
#line hidden
            EndContext();
            BeginContext(10427, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(10483, 13, false);
#line 233 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskName);

#line default
#line hidden
            EndContext();
            BeginContext(10496, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(10552, 41, false);
#line 234 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskStartDate.ToString("dd.MM.yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(10593, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(10649, 50, false);
#line 235 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskEstimateTime.Value.ToString("dd.MM.yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(10699, 54, true);
            WriteLiteral("</td>\r\n                                        </tr>\r\n");
            EndContext();
#line 237 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                    }
                                }

#line default
#line hidden
            BeginContext(10827, 627, true);
            WriteLiteral(@"                            </tbody>
                        </table>
                    </div>
                    <div class=""tab-pane"" id=""settings"">
                        <table class=""table"">
                            <thead class=""text-warning"">
                                <tr>
                                    <th>ID</th>
                                    <th>Name</th>
                                    <th>Start Date</th>
                                    <th>End Date</th>
                                </tr>
                            </thead>
                            <tbody>
");
            EndContext();
#line 253 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                 if (Model != null && Model.OverdueTasks.Any())
                                {
                                    foreach (var item in Model.OverdueTasks.OrderByDescending(task => task.TaskEstimateTime).Take(5))
                                    {

#line default
#line hidden
            BeginContext(11744, 94, true);
            WriteLiteral("                                        <tr>\r\n                                            <td>");
            EndContext();
            BeginContext(11839, 11, false);
#line 258 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskId);

#line default
#line hidden
            EndContext();
            BeginContext(11850, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(11906, 13, false);
#line 259 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskName);

#line default
#line hidden
            EndContext();
            BeginContext(11919, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(11975, 41, false);
#line 260 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskStartDate.ToString("dd.MM.yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(12016, 55, true);
            WriteLiteral("</td>\r\n                                            <td>");
            EndContext();
            BeginContext(12072, 50, false);
#line 261 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.TaskEstimateTime.Value.ToString("dd.MM.yyyy"));

#line default
#line hidden
            EndContext();
            BeginContext(12122, 54, true);
            WriteLiteral("</td>\r\n                                        </tr>\r\n");
            EndContext();
#line 263 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                    }
                                }

#line default
#line hidden
            BeginContext(12250, 210, true);
            WriteLiteral("                            </tbody>\r\n                        </table>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <div class=\"col-lg-6 col-md-12\">\r\n");
            EndContext();
#line 273 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
         if (User.IsInRole("Admin"))
        {

#line default
#line hidden
            BeginContext(12509, 683, true);
            WriteLiteral(@"            <div class=""card"">
                <div class=""card-header card-header-warning"">
                    <h4 class=""card-title"">Billing Information</h4>
                    <p class=""card-category"">Subscriptions</p>
                </div>
                <div class=""card-body table-responsive"">
                    <table class=""table table-hover"">
                            <thead class=""text-warning"">
                                <tr>
                                    <th>Name</th>
                                    <th>Total Cost</th>
                                </tr>
                            </thead>
                            <tbody>
");
            EndContext();
#line 289 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                 if (Model != null && Model.subscriptions.Count() > 0)
                                {
                                    foreach (var item in Model.subscriptions.Take(5))
                                    {

#line default
#line hidden
            BeginContext(13441, 94, true);
            WriteLiteral("                                        <tr>\r\n                                            <td>");
            EndContext();
            BeginContext(13536, 9, false);
#line 294 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                           Write(item.Name);

#line default
#line hidden
            EndContext();
            BeginContext(13545, 57, true);
            WriteLiteral("</td>\r\n                                            <td>??? ");
            EndContext();
            BeginContext(13604, 14, false);
#line 295 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                              Write(item.TotalCost);

#line default
#line hidden
            EndContext();
            BeginContext(13619, 54, true);
            WriteLiteral("</td>\r\n                                        </tr>\r\n");
            EndContext();
#line 297 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                    }
                                }

#line default
#line hidden
            BeginContext(13747, 116, true);
            WriteLiteral("                            </tbody>\r\n                        </table>\r\n                </div>\r\n            </div>\r\n");
            EndContext();
#line 303 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
        }

#line default
#line hidden
            BeginContext(13874, 159, true);
            WriteLiteral("    </div>\r\n</div>\r\n<script type=\"text/javascript\">\r\n    setTimeout(function () {\r\n            // Javascript method\'s body can be found in assets/js/demos.js\r\n");
            EndContext();
#line 309 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
          
            string CompletedTasks = null, ClosedTasks = null, OngoingTasks = null, OverdueTasks = null;
            string Labels = null;
            string Series = null;
            if (Model != null)
            {
                if (Model.DaywiseCompletedTasks.Any())
                {
                    Labels = null;
                    Series = null;
                    foreach (var item in Model.DaywiseCompletedTasks)
                    {
                        if (string.IsNullOrEmpty(Labels))
                        {
                            Labels = string.Concat("\'" + item.Item1, "\'");
                            Series = string.Concat(item.Item2);
                        }
                        else
                        {
                            Labels += string.Concat(",", "\'" + item.Item1, "\'");
                            Series += string.Concat(",", item.Item2);
                        }
                    }
                    CompletedTasks = string.Concat("{labels: [", Labels, "]");
                    CompletedTasks += string.Concat(",series: [[", Series, "]]}");
                }
                else
                {
                    CompletedTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                    CompletedTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
                }
                if (Model.DaywiseInprogressTasks.Any())
                {
                    Labels = null;
                    Series = null;
                    foreach (var item in Model.DaywiseInprogressTasks)
                    {
                        if (string.IsNullOrEmpty(Labels))
                        {
                            Labels = string.Concat("\'" + item.Item1, "\'");
                            Series = string.Concat(item.Item2);
                        }
                        else
                        {
                            Labels += string.Concat(",", "\'" + item.Item1, "\'");
                            Series += string.Concat(",", item.Item2);
                        }
                    }
                    OngoingTasks = string.Concat("{labels: [", Labels, "]");
                    OngoingTasks += string.Concat(",series: [[", Series, "]]}");
                }
                else
                {
                    OngoingTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                    OngoingTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
                }
                if (Model.DaywiseOverDueTasks.Any())
                {
                    Labels = null;
                    Series = null;
                    foreach (var item in Model.DaywiseOverDueTasks)
                    {
                        if (string.IsNullOrEmpty(Labels))
                        {
                            Labels = string.Concat("\'" + item.Item1, "\'");
                            Series = string.Concat(item.Item2);
                        }
                        else
                        {
                            Labels += string.Concat(",", "\'" + item.Item1, "\'");
                            Series += string.Concat(",", item.Item2);
                        }
                    }
                    OverdueTasks = string.Concat("{labels: [", Labels, "]");
                    OverdueTasks += string.Concat(",series: [[", Series, "]]}");
                }
                else
                {
                    OngoingTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                    OngoingTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
                }
                if (Model.DaywiseClosedTasks.Any())
                {
                    Labels = null;
                    Series = null;
                    foreach (var item in Model.DaywiseClosedTasks)
                    {
                        if (string.IsNullOrEmpty(Labels))
                        {
                            Labels = string.Concat("\'" + item.Item1, "\'");
                            Series = string.Concat(item.Item2);
                        }
                        else
                        {
                            Labels += string.Concat(",", "\'" + item.Item1, "\'");
                            Series += string.Concat(",", item.Item2);
                        }
                    }
                    ClosedTasks = string.Concat("{labels: [", Labels, "]");
                    ClosedTasks += string.Concat(",series: [[", Series, "]]}");
                }
                else
                {
                    ClosedTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                    ClosedTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
                }
            }
            else
            {
                CompletedTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                CompletedTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
                OngoingTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                OngoingTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
                OngoingTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                OngoingTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
                ClosedTasks = string.Concat("{labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S']");
                ClosedTasks += string.Concat(",series: [[0, 0, 0, 0, 0, 0, 0]]}");
            }

            

#line default
#line hidden
            BeginContext(19807, 42, true);
            WriteLiteral(";\r\n            md.initDashboardPageCharts(");
            EndContext();
            BeginContext(19851, 24, false);
#line 429 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                   Write(Html.Raw(CompletedTasks));

#line default
#line hidden
            EndContext();
            BeginContext(19876, 1, true);
            WriteLiteral(",");
            EndContext();
            BeginContext(19879, 22, false);
#line 429 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                                               Write(Html.Raw(OngoingTasks));

#line default
#line hidden
            EndContext();
            BeginContext(19902, 1, true);
            WriteLiteral(",");
            EndContext();
            BeginContext(19905, 22, false);
#line 429 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                                                                         Write(Html.Raw(OverdueTasks));

#line default
#line hidden
            EndContext();
            BeginContext(19928, 1, true);
            WriteLiteral(",");
            EndContext();
            BeginContext(19931, 21, false);
#line 429 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                                                                                                   Write(Html.Raw(ClosedTasks));

#line default
#line hidden
            EndContext();
            BeginContext(19953, 2, true);
            WriteLiteral(",\"");
            EndContext();
            BeginContext(19957, 28, false);
#line 429 "D:\source\repos\TMS\TaxationQuerySystemAPI\TaskManagementSystem\Views\Home\Index.cshtml"
                                                                                                                                             Write(Model?.TotalTasksCount ?? 10);

#line default
#line hidden
            EndContext();
            BeginContext(19986, 380, true);
            WriteLiteral(@""");
            //var dataDailySalesChart = {
            //   labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
            //   series: [
            //     [12, 17, 7, 17, 23, 18, 38]
            //   ]
            // };
            //md.initDashboardPageCharts(dataDailySalesChart, dataDailySalesChart, dataDailySalesChart, dataDailySalesChart);
        },5000);
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Dashboard> Html { get; private set; }
    }
}
#pragma warning restore 1591
