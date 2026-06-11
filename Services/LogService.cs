using Microsoft.EntityFrameworkCore;
using m_motors_API.Data;
using m_motors_API.DTO.Logs;
using m_motors_API.Models;

namespace m_motors_API.Services
{
    public class LogService : ILogService
    {
        private readonly MMotorsContext _context;

        public LogService(MMotorsContext context)
        {
            _context = context;
        }
        public async Task LogInfoAsync(
            string message,
            string? utilisateur = null,
            string? endpoint = null,
            string? methodeHttp = null)
        {
            var log = new ApplicationLog
            {
                Niveau = "INFO",
                Message = message,
                Utilisateur = utilisateur,
                Endpoint = endpoint,
                MethodeHttp = methodeHttp,
                DateLog = DateTime.UtcNow
            };

            _context.ApplicationLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task LogWarningAsync(
            string message,
            string? utilisateur = null,
            string? endpoint = null,
            string? methodeHttp = null)
        {
            var log = new ApplicationLog
            {
                Niveau = "WARNING",
                Message = message,
                Utilisateur = utilisateur,
                Endpoint = endpoint,
                MethodeHttp = methodeHttp,
                DateLog = DateTime.UtcNow
            };

            _context.ApplicationLogs.Add(log);

            await _context.SaveChangesAsync();
        }

        public async Task LogInfoAsync(string message, string? utilisateur = null)
        {
            var log = new ApplicationLog
            {
                Niveau = "INFO",
                Message = message,
                Utilisateur = utilisateur
            };

            _context.ApplicationLogs.Add(log);

            await _context.SaveChangesAsync();
        }

        public async Task LogErrorAsync(
            string message,
            Exception exception,
            string? endpoint = null,
            string? methodeHttp = null,
            string? utilisateur = null)
        {
            var log = new ApplicationLog
            {
                Niveau = "ERROR",
                Message = message,
                Exception = exception.Message,
                StackTrace = exception.StackTrace,
                Endpoint = endpoint,
                MethodeHttp = methodeHttp,
                Utilisateur = utilisateur
            };

            _context.ApplicationLogs.Add(log);

            await _context.SaveChangesAsync();
        }

        public async Task<object> GetLogsAsync(LogFilterRequest filter)
        {
            var query = _context.ApplicationLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Niveau))
            {
                query = query.Where(l => l.Niveau == filter.Niveau);
            }

            if (!string.IsNullOrWhiteSpace(filter.Recherche))
            {
                query = query.Where(l =>
                    l.Message.Contains(filter.Recherche));
            }

            if (filter.DateDebut.HasValue)
            {
                query = query.Where(l =>
                    l.DateLog >= filter.DateDebut.Value);
            }

            if (filter.DateFin.HasValue)
            {
                query = query.Where(l =>
                    l.DateLog <= filter.DateFin.Value);
            }

            var total = await query.CountAsync();

            var logs = await query
                .OrderByDescending(l => l.DateLog)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(l => new LogResponseDto
                {
                    IdLog = l.IdLog,
                    Niveau = l.Niveau,
                    Message = l.Message,
                    Endpoint = l.Endpoint,
                    Utilisateur = l.Utilisateur,
                    DateLog = l.DateLog
                })
                .ToListAsync();

            return new
            {
                total,
                page = filter.Page,
                pageSize = filter.PageSize,
                data = logs
            };
        }
    }
}