using CafeManager.Core.Data;
using CafeManager.Core.DTOs;
using CafeManager.Core.Services;
using CafeManager.WPF.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace CafeManager.WPF.Services
{
    public class AppUserServices(IUnitOfWork unitOfWork, IConfiguration configuration, EncryptionHelper encryptionHelper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IConfiguration _configuration = configuration;
        private readonly EncryptionHelper _encryptionHelper = encryptionHelper;
        public string VerificationCode { get; private set; } = string.Empty;
        public string NewPassWord { get; private set; } = string.Empty;

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
                appuser.Password = _encryptionHelper.EncryptAES(appuser.Password);
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

        public async Task<(Appuser?, bool, int?)> Login(string username, string password)
        {
            try
            {
                Appuser? exisiting = await _unitOfWork.AppUserList.GetAppUserByUserName(username);
                if (exisiting == null)
                {
                    return (null, false, null);
                }
                bool isPasswordMatch = _encryptionHelper.DecryptAES(exisiting.Password ?? string.Empty)?.Equals(password) ?? false;
                return (exisiting, isPasswordMatch, exisiting?.Role);
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

        private string GenerateVerificationCode
        {
            get
            {
                Random random = new();
                return random.Next(100000, 999999).ToString();
            }
        }

        public async Task SendVerificationEmail(string recipientEmail, string userName)
        {
            try
            {
                VerificationCode = GenerateVerificationCode;

                var assembly = Assembly.GetExecutingAssembly();
                var file = "CafeManager.WPF.Stores.email_template.html";
                string htmlTemplate;
                using (Stream? stream = assembly.GetManifestResourceStream(file))
                {
                    if (stream == null)
                        throw new FileNotFoundException("Tài nguyên không tìm thấy.", file);

                    using StreamReader reader = new(stream);
                    htmlTemplate = reader.ReadToEnd();
                }

                string emailBody = htmlTemplate
                    .Replace("{{UserName}}", userName)
                    .Replace("{{VerificationCode}}", VerificationCode);

                using SmtpClient smtpClient = new("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(_configuration["Email:AdminAccount"], configuration["Email:PassWord"]);
                smtpClient.EnableSsl = true;

                MailMessage mailMessage = new()
                {
                    From = new MailAddress(_configuration["Email:AdminAccount"], "Cafe Manager App"),
                    Subject = "Xác thực tài khoản",
                    Body = emailBody,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipientEmail);

                await smtpClient.SendMailAsync(mailMessage);
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
                NewPassWord = GenerateVerificationCode;

                var assembly = Assembly.GetExecutingAssembly();
                var file = "CafeManager.WPF.Stores.reset_email_template.html";
                string htmlTemplate;
                using (Stream? stream = assembly.GetManifestResourceStream(file))
                {
                    if (stream == null)
                        throw new FileNotFoundException("Tài nguyên không tìm thấy.", file);

                    using StreamReader reader = new(stream);
                    htmlTemplate = reader.ReadToEnd();
                }

                string emailBody = htmlTemplate
                    .Replace("{{UserName}}", userName)
                    .Replace("{{AdminEmail}}", configuration["Email:AddminAccount"])
                    .Replace("{{NewPassWord}}", NewPassWord);

                using SmtpClient smtpClient = new("smtp.gmail.com");
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(_configuration["Email:AdminAccount"], configuration["Email:PassWord"]);
                smtpClient.EnableSsl = true;

                MailMessage mailMessage = new()
                {
                    From = new MailAddress(_configuration["Email:AdminAccount"], "Cafe Manager App"),
                    Subject = "Đặt lại mật khẩu",
                    Body = emailBody,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipientEmail);

                await smtpClient.SendMailAsync(mailMessage);
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
                res.Password = _encryptionHelper.EncryptAES(NewPassWord);
                await _unitOfWork.AppUserList.Update(res);
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

        public async Task<Appuser?> GetAppUserByUserName(string userName)
        {
            return await _unitOfWork.AppUserList.GetAppUserByUserName(userName);
        }

        public async Task<Appuser?> UpdateAppUser(Appuser user)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var res = await _unitOfWork.AppUserList.Update(user);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return res;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _unitOfWork.ClearChangeTracker();
                throw new InvalidOperationException("Lỗi");
            }
        }

        public async Task<bool> DeleteAppUser(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var deleted = await _unitOfWork.AppUserList.Delete(id);
                if (deleted == false)
                {
                    throw new InvalidOperationException("Lỗi.");
                }
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return deleted;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new InvalidOperationException("Xoá tài khoản thất bại.", ex);
            }
        }

        // ===================== Phan trang =======================
        public async Task<(IEnumerable<Appuser>?, int)> GetSearchPaginateListAppuser(Expression<Func<Appuser, bool>>? searchPredicate = null, int skip = 0, int take = 20)
        {
            _unitOfWork.ClearChangeTracker();
            var listPage = await _unitOfWork.AppUserList.GetByPageAsync(skip, take, searchPredicate);

            return listPage;
        }
    }
}