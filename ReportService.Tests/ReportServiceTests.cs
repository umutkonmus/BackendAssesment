using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ReportService.DatabaseContext;
using ReportService.DTOs.Person;
using ReportService.DTOs.Report;
using ReportService.Models;
using ReportService.Services;
using ReportService.Services.Abstract;
using ReportService.Utils;
using Xunit;

namespace ReportService.Tests
{
    public class ReportServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IDirectoryServiceClient> _directoryClientMock;
        private readonly Mock<ILogger<ReportService.Services.ReportService>> _loggerMock;

        public ReportServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Report, ReportDTO>();
                cfg.CreateMap<Report, ReportWithDetailsDTO>();
                cfg.CreateMap<ReportDetail, ReportDetailDTO>();
            });

            _mapper = config.CreateMapper();
            _directoryClientMock = new Mock<IDirectoryServiceClient>();
            _loggerMock = new Mock<ILogger<ReportService.Services.ReportService>>();
        }

        private PostgresDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PostgresDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new PostgresDbContext(options);
        }

        [Fact]
        public async Task GetAllReportsAsync_ShouldReturnReports_WhenReportsExist()
        {
            var dbContext = GetInMemoryDbContext("GetAllReports_WithData");
            dbContext.Reports.Add(new Report { ID = Guid.NewGuid(), Status = ReportStatus.Preparing });
            await dbContext.SaveChangesAsync();

            var service = new ReportService.Services.ReportService(dbContext, _mapper, _directoryClientMock.Object, _loggerMock.Object);

            var result = await service.GetAllReportsAsync();

            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.Data);
            Assert.Equal((int)StatusCode.Success, result.Status);
        }

        [Fact]
        public async Task GetAllReportsAsync_ShouldReturnNoContent_WhenNoReportsExist()
        {
            var dbContext = GetInMemoryDbContext("GetAllReports_Empty");
            var service = new ReportService.Services.ReportService(dbContext, _mapper, _directoryClientMock.Object, _loggerMock.Object);

            var result = await service.GetAllReportsAsync();

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.Data);
            Assert.Equal((int)StatusCode.NoContent, result.Status);
        }

        [Fact]
        public async Task GetReportByIdAsync_ShouldReturnReport_WhenReportExists()
        {
            var dbContext = GetInMemoryDbContext("GetReportById_Success");
            var reportId = Guid.NewGuid();
            dbContext.Reports.Add(new Report { ID = reportId, Status = ReportStatus.Preparing });
            await dbContext.SaveChangesAsync();

            var service = new ReportService.Services.ReportService(dbContext, _mapper, _directoryClientMock.Object, _loggerMock.Object);

            var result = await service.GetReportByIdAsync(reportId);

            Assert.True(result.IsSuccessful);
            Assert.NotNull(result.Data);
            Assert.Equal(reportId, result.Data.ID);
            Assert.Equal((int)StatusCode.Success, result.Status);
        }

        [Fact]
        public async Task GetReportByIdAsync_ShouldReturnNotFound_WhenReportDoesNotExist()
        {
            var dbContext = GetInMemoryDbContext("GetReportById_NotFound");
            var service = new ReportService.Services.ReportService(dbContext, _mapper, _directoryClientMock.Object, _loggerMock.Object);

            var result = await service.GetReportByIdAsync(Guid.NewGuid());

            Assert.False(result.IsSuccessful);
            Assert.Equal((int)StatusCode.NotFound, result.Status);
        }

        [Fact]
        public async Task CreateReportAsync_ShouldCreateReportSuccessfully()
        {
            var dbContext = GetInMemoryDbContext("CreateReport_Success");
            var service = new ReportService.Services.ReportService(dbContext, _mapper, _directoryClientMock.Object, _loggerMock.Object);

            var location = "TestLocation";

            var result = await service.CreateReportAsync(location);

            Assert.True(result.IsSuccessful);
            Assert.NotNull(result.Data);
            Assert.Equal((int)StatusCode.Created, result.Status);
        }

        [Fact]
        public async Task ProcessReportAsync_ShouldProcessReportSuccessfully()
        {
            var dbContext = GetInMemoryDbContext("ProcessReport_Success");
            var reportId = Guid.NewGuid();
            dbContext.Reports.Add(new Report { ID = reportId, Status = ReportStatus.Preparing });
            await dbContext.SaveChangesAsync();

            var contactInfos = new List<ContactInfoDTO>
            {
                new ContactInfoDTO { TypeName = "location", Value = "TestLocation" },
                new ContactInfoDTO { TypeName = "phone" }
            };

            _directoryClientMock.Setup(dc => dc.GetAllContactInfosAsync()).ReturnsAsync(contactInfos);

            var service = new ReportService.Services.ReportService(dbContext, _mapper, _directoryClientMock.Object, _loggerMock.Object);

            var result = await service.ProcessReportAsync(reportId, "TestLocation");

            Assert.True(result.IsSuccessful);
            Assert.Equal((int)StatusCode.Success, result.Status);
        }
    }
}
