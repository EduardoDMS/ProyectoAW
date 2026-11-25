using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProyectoAW.Infraestructure.MVC;
using ProyectoAW.Infrastructure.Data;

// Guias
using ProyectoAW.Modules.Guias.Repositories;
using ProyectoAW.Modules.Guias.Services;
// Almacenes
using ProyectoAW.Modules.Maestros.Almacenes.Repositories;
using ProyectoAW.Modules.Maestros.Almacenes.Services;
// Categorias
using ProyectoAW.Modules.Maestros.Categorias.Repositories;
using ProyectoAW.Modules.Maestros.Categorias.Services;
// Clientes
using ProyectoAW.Modules.Maestros.Clientes.Repositories;
using ProyectoAW.Modules.Maestros.Clientes.Services;
// Productos
using ProyectoAW.Modules.Maestros.Productos.Repositories;
using ProyectoAW.Modules.Maestros.Productos.Services;
// Proveedores
using ProyectoAW.Modules.Maestros.Proveedores.Repositories;
using ProyectoAW.Modules.Maestros.Proveedores.Services;
// TipoDocumentos
using ProyectoAW.Modules.Maestros.TipoDocumentos.Repositories;
using ProyectoAW.Modules.Maestros.TipoDocumentos.Services;
// Ubicaciones
using ProyectoAW.Modules.Maestros.Ubicaciones.Repositories;
using ProyectoAW.Modules.Maestros.Ubicaciones.Services;
// Unidad de Medida
using ProyectoAW.Modules.Maestros.UnidadesMedida.Repositories;
using ProyectoAW.Modules.Maestros.UnidadesMedida.Services;
// Rol
using ProyectoAW.Modules.Seguridad.Roles.Repositories;
using ProyectoAW.Modules.Seguridad.Roles.Services;
using ProyectoAW.Modules.Seguridad.Perfiles.Repositories;
using ProyectoAW.Modules.Seguridad.Perfiles.Services;
//Security(ni se por que lo pongo)
using ProyectoAW.Modules.Seguridad.Security_Session.Permisos.HttpAuth;
using ProyectoAW.Modules.Seguridad.Security_Session.Repositories;
using ProyectoAW.Modules.Seguridad.Security_Session.Services;
// Usuarios
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;
using ProyectoAW.Modules.Seguridad.Usuarios.Repositories;
using ProyectoAW.Modules.Seguridad.Usuarios.Services;
//Stocks
using ProyectoAW.Modules.Stocks.Repositories;
using ProyectoAW.Modules.Stocks.Services;
using ProyectoAW.Modules.Stocks.Validacion;
//Fecha_Hora
//Tipo ComportamientoGuia
using ProyectoAW.Modules.Maestros.Operaciones.Entities;
using ProyectoAW.Modules.Maestros.TipoComportamientos.Repositories;
using ProyectoAW.Modules.Maestros.TipoComportamientos.Services;
//System
using System.Text;
using System.Transactions;
using ProyectoAW.Modules.Maestros.MotivosGuia.Services;
using ProyectoAW.Modules.Maestros.MotivosGuia.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();
// MVC
builder.Services.AddControllersWithViews().AddRazorOptions(options =>
{
    options.ViewLocationExpanders.Add(new ModuleViewLocationExpander());
});

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Ingrese: Bearer {token}"
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                new List<string>()
        });
});

// EF Core
builder.Services.AddDbContext<ProyectoAW.Infrastructure.Data.AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// DI
builder.Services.AddScoped<IAlmacenRepository, AlmacenRepository>();
builder.Services.AddScoped<IAlmacenService, AlmacenService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();
builder.Services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
builder.Services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();
builder.Services.AddScoped<IUnidadMedidaRepository,UnidadMedidaRepository>();
builder.Services.AddScoped<IUnidadMedidaService, UnidadMedidaService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IGuiaRepository, GuiaRepository>();
builder.Services.AddScoped<IGuiaService, GuiaService>();
builder.Services.AddScoped<IMotivoGuiaRepository, MotivoGuiaRepository>();
builder.Services.AddScoped<IMotivoGuiaService, MotivoGuiaService>();
builder.Services.AddScoped<IMovimientoStockRepository, MovimientoStockRepository>();
builder.Services.AddScoped<IMovimientoStockService, MovimientoStockService>();
builder.Services.AddScoped<IStockBalanceRepository, StockBalanceRepository>();
builder.Services.AddScoped<ILecturaGuiaRepository, LecturaGuiaRepository>();
builder.Services.AddScoped<ICurrentUserService,CurrentUserService>();
builder.Services.AddScoped<ITransactionManager, TransactionManager>();
builder.Services.AddScoped<IGuiaStockProcessor, EntradaStockProcessor>();
builder.Services.AddScoped<IGuiaStockProcessor, SalidaStockProcessor>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ITipoComportamientosRepository, TipoComportamientosRepository>();
builder.Services.AddScoped<ITipoComportamientoService, TipoComportamientoService>();

builder.Services.AddScoped<ValidacionesBase>();
builder.Services.AddScoped<ValidacionesGuia>();
builder.Services.AddScoped<ValidacionesStock>();
builder.Services.AddScoped<ValidacionesUbicacion>();

builder.Services.AddHttpContextAccessor(); // no


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Smart";
        options.DefaultChallengeScheme = "Smart";
    })
    .AddPolicyScheme("Smart", "JWT o Cookie", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            var authorization = context.Request.Headers.Authorization.ToString();

            if (authorization.StartsWith(
                "Bearer ",
                StringComparison.OrdinalIgnoreCase))
            {
                return JwtBearerDefaults.AuthenticationScheme;
            }

            return CookieAuthenticationDefaults.AuthenticationScheme;
        };
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccesoDenegado";

        options.Cookie.Name = "ProyectoAW.Session";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.AddPolicy("Administrador", policy =>
        policy.RequireAssertion(context =>
            CatalogoPermisos.EsAdministrador(context.User)));

    options.AddPolicy("Operador", policy =>
        policy.RequireRole("Operador"));

    foreach (var codigoPermiso in CatalogoPermisos.ListarCodigosPermiso())
    {
        var permiso = codigoPermiso;
        options.AddPolicy(permiso, policy =>
            policy.RequireAssertion(context =>
                CatalogoPermisos.EsAdministrador(context.User) ||
                context.User.HasClaim("Permiso", permiso)));
    }
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await UsuarioConfiguration.CrearAdminSiUsuariosVacioAsync(dbContext);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// Publica controladores con rutas por atributos, como /api/guias.
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();


