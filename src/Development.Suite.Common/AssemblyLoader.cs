using System.Reflection;

namespace Development.Suite.Common;

public static class AssemblyLoader
{
    private static readonly Dictionary<string, Assembly> AssembliesByName;

    static AssemblyLoader()
    {
        AssembliesByName = new Dictionary<string, Assembly>();
    }

    public static Assembly Load(byte[] bytes)
    {
        var assembly = Assembly.Load(bytes);

        if (assembly.FullName != null)
            AssembliesByName[assembly.FullName] = assembly;

        return assembly;
    }

    public static Assembly? Resolve(object? sender, ResolveEventArgs args)
    {
        return AssembliesByName.TryGetValue(args.Name, out var assembly) ? assembly : null;
    }
}