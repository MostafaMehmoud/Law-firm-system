using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.CORE.ViewModels;

namespace Law.BL.Services.IServices
{
    public interface IReportService
    {
        public Task<List<ClientReportVm>> GetClientsReportViewModel(FilterDto filter);
            public Task<List<CourtReportVm>> GetPartysReportViewModel(FilterDto filter);
            public Task<List<CourtReportDto>> GetCourtsReportViewModel();
            public Task<List<CenterReportDto>> GetCentersReportViewModel();
        Task<List<IssueReportDto>> GetIssuesReportAsync(FilterDto filter);
        public  Task<List<ClientReceiptReportDto>> GetReceiptIssueGroupedByClient(FilterDto filter);
        Task<AdminReportViewModel> GetAdminReportAsync();
        public Task<List<TodaySessionViewModel>> GetTodaySessionsViewModelAsync();
        Task<List<AdminLawyerActivityViewModel>> GetAllLawyerActivityAsync();




    }
}
