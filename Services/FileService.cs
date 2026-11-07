using System;
using System.IO;
using System.Linq;

namespace Car_Catalogue_Desktop.Services {
    public class FileService {
        public static string GetProjectRoot() {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !dir.GetFiles("*.csproj").Any()) {
                dir = dir.Parent;
            }
            return dir?.FullName ?? AppContext.BaseDirectory;
        }

        public static string GetImagesDirectory() {
            var root = GetProjectRoot();
            var imgDir = Path.Combine(root, "Images");
            if (!Directory.Exists(imgDir)) {
                Directory.CreateDirectory(imgDir);
            }
            return imgDir;
        }

        public static string GetImagePath(int carId) {
            return Path.Combine(GetImagesDirectory(), $"{carId}.png");
        }

        public static void SaveImage(string sourceFilePath, int carId) {
            if (string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath)) return;
            
            var destPath = GetImagePath(carId);
            if (sourceFilePath != destPath) {
                File.Copy(sourceFilePath, destPath, overwrite: true);
            }
        }

        public static void DeleteImage(int carId) {
            var path = GetImagePath(carId);
            if (File.Exists(path)) {
                File.Delete(path);
            }
        }
    }
}
