using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace RSAInteractJS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages(options => {
                // 修改起始页（默认是Index）
                // 方法1(推荐)：把 Index.cshtml 改名；然后 AddPageRoute("/XXX", "")
                options.Conventions.AddPageRoute("/Home", "");
                // 方法2：实现 IPageRouteModelConvention 接口
                //options.Conventions.Add(new DefaultPageRouteModelConvention("/RSA"));
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.IncludeXmlComments($"{AppContext.BaseDirectory}/{Assembly.GetExecutingAssembly().GetName().Name}.xml", true);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            // 设置静态页面为起始页
            //DefaultFilesOptions defaultFilesOptions = new();
            //defaultFilesOptions.DefaultFileNames.Add("rsa.html");
            //app.UseDefaultFiles(defaultFilesOptions);

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.MapGet("/", context =>
            //{
            //    context.Response.Redirect("/rsa");
            //    return Task.CompletedTask;
            //});

            //app.Use(async (context, next) =>
            //{
            //    if (context.Request.Path == "/")
            //    {
            //        context!.Response.Redirect("/RSA");
            //        await context.Response.CompleteAsync();
            //        return;
            //    }
            //    await next();
            //});

            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }

    public class DefaultPageRouteModelConvention : IPageRouteModelConvention
    {
        public string _defaultPath;

        public DefaultPageRouteModelConvention(string defaultPath)
        {
            if (string.IsNullOrWhiteSpace(defaultPath))
            {
                throw new ArgumentNullException(nameof(defaultPath));
            }

            if (!defaultPath.StartsWith('/'))
            {
                throw new ArgumentException(nameof(defaultPath) + "必须以/开头");
            }

            _defaultPath = defaultPath;
        }

        public void Apply(PageRouteModel model)
        {
            if (model.ViewEnginePath.Equals("/Index", StringComparison.OrdinalIgnoreCase))
            {
                model.Selectors.Clear();
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = "/Index"
                    }
                });
            }
            else if (model.ViewEnginePath.Equals(_defaultPath, StringComparison.OrdinalIgnoreCase))
            {
                model.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel
                    {
                        Template = "/"
                    }
                });
            }
        }
    }

}
