using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeManager.WPF.Services
{
    public class FirebaseStorageService
    {
        private readonly string _firebaseBucket;

        public FirebaseStorageService(string firebaseBucket)
        {
            _firebaseBucket = firebaseBucket;
        }

        public async Task<string> UploadFileAsync(string localFilePath)
        {
            // Đường dẫn tới file bạn muốn upload
            var url = new Uri(localFilePath).LocalPath;
            using (var stream = File.Open(url, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // FirebaseStorage upload logic
                var task = new FirebaseStorage(
                    _firebaseBucket)
                    .Child("Test_Image")
                    .Child(Path.GetFileName(url))
                    .PutAsync(stream);

                var downloadUrl = await task;

                return downloadUrl;
            }
        }
    }
}