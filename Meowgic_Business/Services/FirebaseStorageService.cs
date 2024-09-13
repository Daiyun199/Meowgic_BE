using Meowgic.Business.Interface;
using Meowgic.Shares.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;

namespace Meowgic.Business.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly IConfiguration _configuration;

        public FirebaseStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> Push(IFormFile file, FileStream data)
        {
            var apiKey = _configuration["Firebase:apiKey"];
            var authEmail = _configuration["Firebase:authEmail"];
            var authPassword = _configuration["Firebase:authPassword"];
            var storageBucket = _configuration["Firebase:bucket"];

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(authEmail, authPassword);

            var cancellationToken = new CancellationTokenSource();

            var storage = new FirebaseStorage(
                storageBucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken),
                    ThrowOnCancel = true
                });

            var task = storage
                .Child("assets")
                .Child($"{file.FileName}.{Path.GetExtension(file.FileName).Substring(1)}")
                .PutAsync(data, cancellationToken.Token);

            try
            {
                var downloadUrl = await task;
                Console.WriteLine($"File uploaded to Firebase Storage. Download URL: {downloadUrl}");
                return downloadUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload file to Firebase Storage: {ex.Message}");
                return "";
            }
        }
    }
}
