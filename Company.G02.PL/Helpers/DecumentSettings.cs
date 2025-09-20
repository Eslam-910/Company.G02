namespace Company.G02.PL.Helpers
{
    public static class DecumentSettings
    {
        //1.Upload
        //ImageName
        public static string UploadFile(IFormFile file,string FolderName)
        {
            //1.Get Folder Location
            
            // string folderpath = "E:\\route\\2-c# course 2\\07 ASP.NET Core MVC-20250430T120847Z-003\\Project\\Company.G02.PL\\wwwroot\\files\\"+FolderName;
            
            //var folderPath= Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + FolderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/files", FolderName);

            //2.Get File Name And Make It Unique

            var filename=$"{Guid.NewGuid()}{file.FileName}";

            //File Path

            var filepath=Path.Combine(folderPath, filename);

            using var filestream = new FileStream(filepath, FileMode.Create);
           
            file.CopyTo(filestream);
            return filename;
        }
        //2.Delete
        public static void FileDelete(string filename, string foldername)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", foldername, filename);
            if (File.Exists(folderPath))
            {
                File.Delete(folderPath);
            }

        }
    }
}
