﻿namespace ClickMarket.AppMvc.Configurations
{
    public static class MvcConfig
    {
        public static WebApplicationBuilder AddMvcConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllersWithViews(options =>
            {
                MvcOptionsConfig.ConfigurarMensagensModelBinding(options.ModelBindingMessageProvider);
            
            });
            builder.Services.AddRazorPages();

            return builder;
        }
    }
}
