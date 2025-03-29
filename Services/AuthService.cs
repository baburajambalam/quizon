public interface IAuthService
{
    Task<bool> SendOtpAsync(string email);
    Task<bool> VerifyOtpAsync(string email, string otp);
}

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly Dictionary<string, string> _otpStore = new();

    public AuthService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<bool> SendOtpAsync(string email)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        _otpStore[email] = otp;
        return await _emailService.SendEmailAsync(email, "Your OTP", $"Your OTP is {otp}");
    }

    public Task<bool> VerifyOtpAsync(string email, string otp)
    {
        return Task.FromResult(_otpStore.TryGetValue(email, out var storedOtp) && storedOtp == otp);
    }
}
