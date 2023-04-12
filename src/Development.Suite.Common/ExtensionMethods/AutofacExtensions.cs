using Autofac;

namespace Development.Suite.Common.ExtensionMethods;

public static class AutofacExtensions
{
    public static void LoadPlugins(this ContainerBuilder builder)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        foreach (var assemblyPath in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
        {
            var assemblyBytes = File.ReadAllBytes(assemblyPath);
            var assembly = AssemblyLoader.Load(assemblyBytes);
            builder.RegisterAssemblyModules(assembly);
        }

        AppDomain.CurrentDomain.AssemblyResolve += AssemblyLoader.Resolve;
    }
}