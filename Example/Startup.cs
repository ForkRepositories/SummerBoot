using Example.Models;
using Example.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using SqlOnline.Utils;
using SummerBoot.Core;
using System;
using System.Data.Common;
using System.Linq;
using Example.DbFile;
using Example.Feign;
using Microsoft.Data.Sqlite;
using SummerBoot.Feign;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSummerBoot();

            services.AddSummerBootCache(it =>
            {
                it.UseRedis("129.204.47.226,password=summerBoot");
            });

            services.AddSummerBootRepository(it =>
            {
                it.DbConnectionType = typeof(SqliteConnection);
                it.ConnectionString = "Data source=./DbFile/mydb.db";
            });

            //���feign����������
            services.AddScoped<IRequestInterceptor,MyRequestInterceptor>();
            services.AddSummerBootFeign();

            services.AddControllers().AddSummerBootMvcExtention();

            //services.AddSbScoped<Engine>(typeof(TransactionalInterceptor));

            //services.AddSbScoped<ICar, Car>(typeof(TransactionalInterceptor));

            //using (var database = new Db())    //����
            //{
            //    database.Database.EnsureCreated(); //���û�д������ݿ���Զ���������Ϊ�ؼ���һ�����
            //}


            //services.AddSbScoped<IAddOilService, AddOilService>(typeof(TransactionalInterceptor));

            //services.AddSbScoped<IWheel, WheelA>("A��̥", typeof(TransactionalInterceptor));
            //services.AddSbScoped<IWheel, WheelB>("B��̥", typeof(TransactionalInterceptor));

            services.AddSbScoped<IPersonService, PersonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
