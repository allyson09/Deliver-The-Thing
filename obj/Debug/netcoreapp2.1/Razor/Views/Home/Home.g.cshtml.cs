#pragma checksum "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a1011ee410dac2dda641342c5fab1d67547ab865"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Home), @"mvc.1.0.view", @"/Views/Home/Home.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Home.cshtml", typeof(AspNetCore.Views_Home_Home))]
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
#line 1 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\_ViewImports.cshtml"
using DeliverTheThing;

#line default
#line hidden
#line 2 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\_ViewImports.cshtml"
using DeliverTheThing.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1011ee410dac2dda641342c5fab1d67547ab865", @"/Views/Home/Home.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"53143caef40a070bce9e9484a2529abfaa3f91d0", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Home : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 986, true);
            WriteLiteral(@"<script>
  $(document).ready(function(){
    $('.remove').click(function() {
        console.log('in click')
        var data = {
            rateId: $(this).val(),
            page: ""home""
        }
        $.ajax({
            data: data,
            type: 'POST',
            url: 'Rates/RemoveRate',
            success: function(response) {
                console.log('yay!!!')
                location.reload();
            }
        })
        return false;
    });
    $('.print').click(function(e) {
        var element = $(this);
        var data = {
            rateId: $(this).val()
        }
        $.ajax({
            data: data,
            type: 'POST',
            url: 'Label',
            success: function(response) {
                element.html(""Removed"");
                element.removeClass(""print"");
            }
        })
        return false;
    });
  });
</script>
<header id=""home"">
<h1>Here are your saved rates ");
            EndContext();
            BeginContext(987, 28, false);
#line 39 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
                         Write(ViewBag.User[0]["firstName"]);

#line default
#line hidden
            EndContext();
            BeginContext(1015, 485, true);
            WriteLiteral(@"!</h1>
<button class=""btn btn-primary""><a href=""/Rates"" >New Shipment Inquery</a></button>
</header>
<section class=""tableScroll"">
    <table class=""table"">
        <thead>
            <tr>
            <th scope=""col"">Service</th>
            <th scope=""col"">Delivery Time</th>
            <th scope=""col"">Shipping Price</th>
            <th scope=""col"">View Shipping Label</th>
            <th scope=""col"">Remove</th>
            </tr>
        </thead>
        <tbody>
");
            EndContext();
#line 54 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
         foreach (var Rate in ViewBag.Rates)
        {

#line default
#line hidden
            BeginContext(1557, 46, true);
            WriteLiteral("            <tr>\r\n            <td scope=\"row\">");
            EndContext();
            BeginContext(1604, 19, false);
#line 57 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
                       Write(Rate["serviceType"]);

#line default
#line hidden
            EndContext();
            BeginContext(1623, 7, true);
            WriteLiteral("</td>\r\n");
            EndContext();
#line 58 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
             if (@Rate["deliveryDays"] > 1)
            {

#line default
#line hidden
            BeginContext(1690, 20, true);
            WriteLiteral("                <td>");
            EndContext();
            BeginContext(1711, 20, false);
#line 60 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
               Write(Rate["deliveryDays"]);

#line default
#line hidden
            EndContext();
            BeginContext(1731, 12, true);
            WriteLiteral(" Days</td>\r\n");
            EndContext();
#line 61 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
            } else {

#line default
#line hidden
            BeginContext(1765, 20, true);
            WriteLiteral("                <td>");
            EndContext();
            BeginContext(1786, 20, false);
#line 62 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
               Write(Rate["deliveryDays"]);

#line default
#line hidden
            EndContext();
            BeginContext(1806, 11, true);
            WriteLiteral(" Day</td>\r\n");
            EndContext();
#line 63 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
            }

#line default
#line hidden
            BeginContext(1832, 17, true);
            WriteLiteral("            <td>$");
            EndContext();
            BeginContext(1850, 22, false);
#line 64 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
            Write(Rate["shippingAmount"]);

#line default
#line hidden
            EndContext();
            BeginContext(1872, 60, true);
            WriteLiteral("</td>\r\n            <td><button class=\"print btn btn-primary\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 1932, "\"", 1962, 1);
#line 65 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
WriteAttributeValue("", 1940, $"{Rate["rateId"]}", 1940, 22, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(1963, 76, true);
            WriteLiteral(">Print</button></td>\r\n            <td><button class=\"remove btn btn-primary\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 2039, "\"", 2069, 1);
#line 66 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
WriteAttributeValue("", 2047, $"{Rate["rateId"]}", 2047, 22, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(2070, 42, true);
            WriteLiteral(">Delete</button></td>\r\n            </tr>\r\n");
            EndContext();
#line 68 "C:\Users\allys\Desktop\Projects\Stores\DeliverTheThing\Views\Home\Home.cshtml"
        }

#line default
#line hidden
            BeginContext(2123, 42, true);
            WriteLiteral("        </tbody>\r\n    </table>\r\n</section>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591