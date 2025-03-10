using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace UploadHelper.Helpers
{
    public static class Upload
    {

        private static string GetRandomName()
        {
            List<int> numbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            List<char> characters = new List<char>()
                {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n',
                'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B',
                'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                'Q', 'R', 'S',  'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '-', '_'};

            string Name = "";
            Random rand = new Random();
            // run the loop till I get a string of 10 characters
            for (int i = 0; i < 25; i++)
            {
                // Get random numbers, to get either a character or a number...
                int random = rand.Next(0, 3);
                if (random == 1)
                {
                    // use a number
                    random = rand.Next(0, numbers.Count);
                    Name += numbers[random].ToString();
                }
                else
                {
                    random = rand.Next(0, characters.Count);
                    Name += characters[random].ToString();
                }
            }
            return Name;
        }
        public static void DeleteFile(string URL)
        {
            if (File.Exists(URL))
            {
                File.Delete(URL);
            }
        }
        public static async Task<string> UploadFiles(IFormFile file, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hosting, string pathName, string property)
        {
            if (!string.IsNullOrEmpty(property))
            {
                string path = Path.Combine(hosting.WebRootPath, "Uploads", property, pathName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var filesNames = Directory.GetFiles(path);
                var isNotFinished = true;
                var newFileName = "";
                while (isNotFinished)
                {
                    newFileName = GetRandomName() + "." + file.FileName.Split(".").Last();
                    if (!filesNames.Contains(newFileName))
                    {
                        isNotFinished = false;
                    }
                }
                using (FileStream stream = new FileStream(Path.Combine(path, newFileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return ("wwwroot/" + "Uploads/" + property + "/" + pathName + "/" + newFileName);
            }
            else
            {
                return await UploadFiles(file, hosting, pathName);
            }
        }
        public static async Task<string> UploadFiles(IFormFile file, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hosting, string pathName)
        {
            string path = Path.Combine(hosting.WebRootPath, "Uploads", pathName.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filesNames = Directory.GetFiles(path);
            var isNotFinished = true;
            var newFileName = "";
            while (isNotFinished)
            {
                newFileName = GetRandomName() + "." + file.FileName.Split(".").Last();
                if (!filesNames.Contains(newFileName))
                {
                    isNotFinished = false;
                }
            }
            using (FileStream stream = new FileStream(Path.Combine(path, newFileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return ("wwwroot/" + "Uploads/" + pathName + "/" + newFileName);
        }
        public static async Task<string> UploadImagess(IFormFile file, IWebHostEnvironment hosting, string pathName)
        {
            string path = Path.Combine(hosting.WebRootPath, "Uploads", pathName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var newFileName = Guid.NewGuid().ToString() + fileExtension;
            string fullPath = Path.Combine(path, newFileName);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 🔥 إرجاع رابط يمكن فتحه من المتصفح
            return $"http://localhost:5024/Uploads/{pathName}/{newFileName}";
        }


        public static async Task<string> UploadImage(IFormFile file, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hosting, string pathName)
        {
            // Allowed image MIME types and extensions
            var allowedExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var allowedMimeTypes = new HashSet<string> { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };

            // Validate file type
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(file.ContentType))
            {
                throw new InvalidOperationException("Only image files are allowed.");
            }
            string path = Path.Combine(hosting.WebRootPath, "Uploads", pathName.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filesNames = Directory.GetFiles(path);
            var isNotFinished = true;
            var newFileName = "";
            while (isNotFinished)
            {
                newFileName = GetRandomName() + "." + file.FileName.Split(".").Last();
                if (!filesNames.Contains(newFileName))
                {
                    isNotFinished = false;
                }
            }
            using (FileStream stream = new FileStream(Path.Combine(path, newFileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return ("wwwroot/" + "Uploads/" + pathName + "/" + newFileName);


        }


    }
}
