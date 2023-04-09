using Autofac;
using System.Reflection;

namespace Development.Suite.Common.ExtensionMethods;

public static class AutofacExtensions
{
    public static void LoadPlugins(this ContainerBuilder builder)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var assembliesByName = new Dictionary<string, Assembly>();

        foreach (var assemblyPath in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
        {
            var assemblyBytes = File.ReadAllBytes(assemblyPath);
            var assembly = Assembly.Load(assemblyBytes);

            if (assembly.FullName != null)
                assembliesByName[assembly.FullName] = assembly;

            builder.RegisterAssemblyModules(assembly);
        }

        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            if (assembliesByName.ContainsKey(args.Name))
                return assembliesByName[args.Name];

            return Assembly.Load(args.Name);
        };
    }
}