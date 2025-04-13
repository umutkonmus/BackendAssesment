using AutoMapper;
using ReportService.DatabaseContext;
using ReportService.Models;
using ReportService.Services.Abstract;
using ReportService.Utils;
using Microsoft.EntityFrameworkCore;
using ReportService.DTOs.Report;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace ReportService.Services
{
    public sealed class ReportService : IReportService
    {
        private readonly PostgresDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IDirectoryServiceClient _directoryClient;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            PostgresDbContext dbContext,
            IMapper mapper,
            IDirectoryServiceClient directoryClient,
            ILogger<ReportService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _directoryClient = directoryClient;
            _logger = logger;
        }

        public async Task<Response<List<ReportDTO>>> GetAllReportsAsync()
        {
            var reports = await _dbContext.Reports.ToListAsync();

            if (reports == null || !reports.Any())
            {
                return Response<List<ReportDTO>>.Success(new List<ReportDTO>(), (int)StatusCode.NoContent);
            }

            var reportDtos = _mapper.Map<List<ReportDTO>>(reports);
            return Response<List<ReportDTO>>.Success(reportDtos, (int)StatusCode.Success);
        }

        public async Task<Response<ReportWithDetailsDTO>> GetReportByIdAsync(Guid id)
        {
            var report = await _dbContext.Reports
                .Include(r => r.ReportDetails)
                .FirstOrDefaultAsync(r => r.ID == id);

            if (report == null)
            {
                return Response<ReportWithDetailsDTO>.Fail("Report not found", (int)StatusCode.NotFound);
            }

            var reportDto = _mapper.Map<ReportWithDetailsDTO>(report);
            return Response<ReportWithDetailsDTO>.Success(reportDto, (int)StatusCode.Success);
        }

        public async Task<Response<ReportDTO>> CreateReportAsync(string location)
        {
            var report = new Report
            {
                ID = Guid.NewGuid(),
                RequestedAt = DateTime.UtcNow,
                Status = ReportStatus.Preparing
            };

            await _dbContext.Reports.AddAsync(report);
            await _dbContext.SaveChangesAsync();

            var reportDto = _mapper.Map<ReportDTO>(report);
            Console.WriteLine($"Report created with ID: {reportDto.ID} for location: {location}");
            return Response<ReportDTO>.Success(reportDto, (int)StatusCode.Created);
        }

        public async Task<Response<ReportDTO>> ProcessReportAsync(Guid reportId, string location)
        {
            try
            {
                var report = await _dbContext.Reports.FindAsync(reportId);

                if (report == null)
                {
                    return Response<ReportDTO>.Fail("Report not found", (int)StatusCode.NotFound);
                }

                var contactInfos = await _directoryClient.GetAllContactInfosAsync();

                var locationContacts = contactInfos
                    .Where(c => c.TypeName == "location" && c.Value == location)
                    .ToList();

                var phoneNumberCount = contactInfos
                    .Where(c => c.TypeName == "phone")
                    .Count();

                var reportDetail = new ReportDetail
                {
                    ID = Guid.NewGuid(),
                    ReportID = reportId,
                    Location = location,
                    PersonCount = locationContacts.Count,
                    PhoneNumberCount = phoneNumberCount
                };

                report.Status = ReportStatus.Completed;

                await _dbContext.ReportDetails.AddAsync(reportDetail);
                await _dbContext.SaveChangesAsync();

                var reportDto = _mapper.Map<ReportDTO>(report);
                return Response<ReportDTO>.Success(reportDto, (int)StatusCode.Success);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing report");
                return Response<ReportDTO>.Fail($"Error processing report: {ex.Message}", (int)StatusCode.BadRequest);
            }
        }
    }
}
