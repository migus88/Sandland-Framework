namespace Sandland.PackageManager.Editor.Data
{
    public class PackageFolderStructureModel
    {
        public string Root { get; set; }
        public string RuntimeCodePath { get; set; }
        public string SharedCodePath { get; set; }
        public string EditorCodePath { get; set; }
        public string TestsCodePath { get; set; }
        public string AssetsPath { get; set; }
    }
}