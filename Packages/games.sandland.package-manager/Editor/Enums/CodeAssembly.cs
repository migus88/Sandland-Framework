namespace Sandland.PackageManager.Editor.Enums
{
    internal enum CodeAssembly
    {
        Editor = 1 << 1,
        Runtime = 1 << 2,
        Shared = 1 << 3,
        Tests = 1 << 4
    }
}