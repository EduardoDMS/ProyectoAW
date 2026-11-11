using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoAW.Infraestructure.MVC;
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
using ProyectoAW.Modules.Seguridad.Usuarios.Entitites;
// Usuarios
using ProyectoAW.Modules.Seguridad.Usuarios.Repositories;
using ProyectoAW.Modules.Seguridad.Usuarios.Services;


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
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IGuiaRepository, GuiaRepository>();
builder.Services.AddScoped<IGuiaService, GuiaService>();


var app = builder.Build();

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

app.UseAuthorization();

app.MapStaticAssets();

// Publica controladores con rutas por atributos, como /api/guias.
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
