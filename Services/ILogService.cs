using m_motors_API.DTO.Logs;

namespace m_motors_API.Services
{
    public interface ILogService
    {
        Task LogInfoAsync(
            string message,
            string? utilisateur = null,
            string? endpoint = null,
            string? methodeHttp = null
        );

        Task LogErrorAsync(
            string message,
            Exception exception,
            string? endpoint = null,
            string? methodeHttp = null,
            string? utilisateur = null
        );

        Task LogWarningAsync(
            string message,
            string? utilisateur = null,
            string? endpoint = null,
            string? methodeHttp = null
        );

        public interface ILogService
        {
            Task CreateLogAsync(
                string niveau,
                string message,
                string details
            );
        }
        Task<object> GetLogsAsync(LogFilterRequest filter);
    }
}