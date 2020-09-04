using System;
using aaatemp_blazorwebassembly.Server.Data;
using aaatemp_blazorwebassembly.Server.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(aaatemp_blazorwebassembly.Server.Areas.Identity.IdentityHostingStartup))]
namespace aaatemp_blazorwebassembly.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}