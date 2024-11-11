using CafeManager.Core.Data;
using CafeManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client.NativeInterop;
using System;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using CafeManager.WPF.Stores;

namespace CafeManager.WPF.Services
{
    public class AppUserServices
    {
        private readonly IServiceProvider _provider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration configuration;
        public string VerificationCode { get; private set; }
        public string NewPassWord { get; private set; }

        public AppUserServices(IServiceProvider provider)
        {
            _provider = provider;
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            configuration = provider.GetRequiredService<IConfiguration>();
        }

        public async Task<Appuser> Register(Appuser appuser)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Appuser? exisiting = await _unitOfWork.AppUserList.GetAppUserByUserName(appuser.Username);
                if (exisiting != null)
                {
                    throw new InvalidOperationException("Tài khoản đã tồn tại ");
                }
                appuser.Password = _provider.GetRequiredService<EncryptionHelper>().EncryptAES(appuser.Password);
                Appuser newAppuser = await _unitOfWork.AppUserList.Create(appuser);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return newAppuser;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Thêm tài khoản thất bại");
            }
        }

        public async Task<(bool, int?)> Login(string username, string password)
        {
            try
            {
                Appuser? exisiting = await _unitOfWork.AppUserList.GetAppUserByUserName(username);
                if (exisiting == null)
                {
                    return (false, null);
                }
                bool isPasswordMatch = _provider.GetRequiredService<EncryptionHelper>().DecryptAES(exisiting.Password ?? string.Empty)?.Equals(password) ?? false;
                return (isPasswordMatch, exisiting?.Role);
            }
            catch
            {
                throw new InvalidOperationException("Lỗi");
            }
        }

        public async Task<bool> HasAppUser(string userName, string email)
        {
            try
            {
                Appuser? res = await _unitOfWork.AppUserList.GetAppUserByUserName(userName);
                if (res != null)
                {
                    return res.Username == userName && res.Email == email;
                }
                return false;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Lỗi");
            }
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); ;
        }

        public async Task SendVerificationEmail(string recipientEmail, string userName)
        {
            try
            {
                VerificationCode = GenerateVerificationCode();

                var assembly = Assembly.GetExecutingAssembly();
                var file = "CafeManager.WPF.Stores.email_template.html";
                string htmlTemplate;
                using (Stream stream = assembly.GetManifestResourceStream(file))
                {
                    if (stream == null)
                        throw new FileNotFoundException("Tài nguyên không tìm thấy.", file);

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        htmlTemplate = reader.ReadToEnd();
                    }
                }

                string emailBody = htmlTemplate
                    .Replace("{{UserName}}", userName)
                    .Replace("{{VerificationCode}}", VerificationCode);

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(configuration["Email:AdminAccount"], configuration["Email:PassWord"]);
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(configuration["Email:AdminAccount"], "Cafe Manager App"),
                        Subject = "Xác thực tài khoản",
                        Body = emailBody,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(recipientEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Email không tồn tại");
            }
        }

        public async Task SendResetPassWord(string recipientEmail, string userName)
        {
            try
            {
                NewPassWord = GenerateVerificationCode();

                var assembly = Assembly.GetExecutingAssembly();
                var file = "CafeManager.WPF.Stores.reset_email_template.html";
                string htmlTemplate;
                using (Stream stream = assembly.GetManifestResourceStream(file))
                {
                    if (stream == null)
                        throw new FileNotFoundException("Tài nguyên không tìm thấy.", file);

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        htmlTemplate = reader.ReadToEnd();
                    }
                }

                string emailBody = htmlTemplate
                    .Replace("{{UserName}}", userName)
                    .Replace("{{AdminEmail}}", configuration["Email:AddminAccount"])
                    .Replace("{{NewPassWord}}", NewPassWord);

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(configuration["Email:AddminAccount"], configuration["Email:PassWord"]);
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(configuration["Email:AddminAccount"], "Cafe Manager App"),
                        Subject = "Đặt lại mật khẩu",
                        Body = emailBody,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(recipientEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Email không tồn tại");
            }
        }

        public async Task<bool> ResetPassWord(string userName)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Appuser? res = await _unitOfWork.AppUserList.GetAppUserByUserName(userName);
                if (res == null)
                {
                    return false;
                }
                res.Password = _provider.GetRequiredService<EncryptionHelper>().EncryptAES(NewPassWord);
                _unitOfWork.AppUserList.Update(res);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi");
            }
        }
    }
}