using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.BL.Services.IServices;
using Law.CORE.Entities;
using Law.CORE.ViewModels;
using Law.DAL.Data;
using Law.DAL.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Law.BL.Services
{
    public class ReportService:IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly LawAppDbContext _context;
        public ReportService(IUnitOfWork unitOfWork, LawAppDbContext context)
        {
            _context = context;
            _unitOfWork= unitOfWork;    
        }
        public async Task<List<OfferAdminViewModel>> GetAllOffersForAdminAsync()
        {
            var offers = await _unitOfWork.OfferRepository.GetAll();
            var cases = await _unitOfWork.cases.GetAll();
            var users = await _unitOfWork.auth.GetAllUsers();

            return offers.Select(o => new OfferAdminViewModel
            {
                OfferId = o.Id,
                LawyerName = users.FirstOrDefault(u => u.Id == o.UserId)?.UserName ?? "غير معروف",
                PhoneNumber = o.PhoneNumber,
                OfficeNumber = o.OfficeNumber,
                ProposedSolution = o.ProposedSolution,
                ProposedFees = o.ProposedFees,
                SubmittedAt = o.SubmittedAt,
                CaseId = o.CaseId,
                CaseTitle = cases.FirstOrDefault(c => c.Id == o.CaseId)?.Title ?? "غير معروفة",
                IsAwarded = o.IsAwarded  // يجب أن تضيف هذا الحقل في كيان Offer
            }).ToList();
        }

        public async Task<bool> AwardOfferAsync(int offerId)
        {
            var offer = await _unitOfWork.OfferRepository.GetById(offerId);
            if (offer == null) return false;

            // إلغاء ترسية أي عرض سابق لنفس القضية
            var allOffers = await _unitOfWork.OfferRepository.GetAll();
            foreach (var o in allOffers.Where(x => x.CaseId == offer.CaseId))
            {
                o.IsAwarded = false;
            }

            offer.IsAwarded = true;
             _unitOfWork.Complete();
            return true;
        }

        public async Task<List<CenterReportDto>> GetCentersReportViewModel()
        {
            var centers = await _unitOfWork.centers.GetAll(); // كل المراكز
            var courts = await _unitOfWork.courts.GetAll();   // كل المحاكم

            // ربط كل مركز بالمحكمة الخاصة به داخل الذاكرة (بدون await داخل حلقة)
            var result = centers.Select((c, index) =>
            {
                var court = courts.FirstOrDefault(x => x.Id == c.CourtId);
                return new CenterReportDto
                {
                    Index = index + 1,
                    Id = c.Code,
                    Name = c.Name,
                    courtName = court?.CourtName ?? ""
                };
            }).ToList();

            return result;
        }

        public async Task<List<IssueReportDto>> GetIssuesReportAsync(FilterDto filter)
        {
            var query = await _unitOfWork.issuesFile.GetAll();

            if (filter.FromClientId.HasValue)
                query = query.Where(x => x.ClientId >= filter.FromClientId.Value);

            if (filter.ToClientId.HasValue)
                query = query.Where(x => x.ClientId <= filter.ToClientId.Value);

            if (filter.FromIssueId.HasValue)
                query = query.Where(x => x.Id >= filter.FromIssueId.Value);

            if (filter.ToIssueId.HasValue)
                query = query.Where(x => x.Id <= filter.ToIssueId.Value);

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.DateNow >=DateOnly.FromDateTime( filter.FromDate.Value));

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.DateNow <= DateOnly.FromDateTime(filter.ToDate.Value));

            var result = query.ToList();

           
            int index = 1;
            var clientDict = (await _unitOfWork.clients.GetAll()).ToDictionary(c => c.Id);
            var partyDict = (await _unitOfWork.parties.GetAll()).ToDictionary(p => p.Id);
            var courtDict = (await _unitOfWork.courts.GetAll()).ToDictionary(c => c.Id);
            var centerDict = (await _unitOfWork.centers.GetAll()).ToDictionary(c => c.Id);

            var fullData = result.Select((item, index) => new IssueReportDto
            {
                Index = index + 1,
                IssueNumber = item.IssueNumber,
                IssueDate = item.DateNow.ToString("yyyy-MM-dd"),
                IssueTitle = item.IssueName,
                ClientName = clientDict.TryGetValue(item.ClientId, out var client) ? client.ClientName : "",
                PartyName = partyDict.TryGetValue(item.PartyId, out var party) ? party.PartyName : "",
                CourtName = courtDict.TryGetValue(item.CourtId, out var court) ? court.CourtName : "",
                CircleName = centerDict.TryGetValue(item.CenterId, out var center) ? center.Name : "",
                JudgeName = courtDict.TryGetValue(item.CourtId, out var court2) ? court2.JudgeName : "",
                IssueValueFees=item.IssueValueFees
            }).ToList();


            return fullData;
        }

        public async Task<List<ClientReportVm>> GetClientsReportViewModel(FilterDto filter)
        {
            var query =await _unitOfWork.clients.GetAll();

            if (filter.FromClientId.HasValue)
                query = query.Where(c => c.Id >= filter.FromClientId.Value);
            if (filter.ToClientId.HasValue)
                query = query.Where(c => c.Id <= filter.ToClientId.Value);

          
            if (filter.FromDate.HasValue)
                query = query.Where(c => c.IssueDate >= DateOnly.FromDateTime( filter.FromDate.Value));
            if (filter.ToDate.HasValue)
                query = query.Where(c => c.IssueDate <= DateOnly.FromDateTime(filter.ToDate.Value));

            var clients =  query.ToList();

            var result = clients.Select((c, index) => new ClientReportVm
            {
                Index = index + 1,
                Id = c.Code,
                Name = c.ClientName,
                NationalId = c.NationalNumber,
                Phone = c.MobileNumber,
                Address = c.Address
            }).ToList();

            return result;
        }

        public async Task<List<CourtReportDto>> GetCourtsReportViewModel()
        {
            var query = await _unitOfWork.courts.GetAll();
            var clients = query.ToList();

            var result = clients.Select((c, index) => new CourtReportDto
            {
                Index = index + 1,
                Id = c.Code,
                Name = c.CourtName,
                Responsible=c.JudgeName,
                Phone = c.PhoneNumber,
                
            }).ToList();

            return result;
        }

        public async Task<List<CourtReportVm>> GetPartysReportViewModel(FilterDto filter)
        {
            var query = await _unitOfWork.parties.GetAll();

            if (filter.FromClientId.HasValue)
                query = query.Where(c => c.Id >= filter.FromClientId.Value);
            if (filter.ToClientId.HasValue)
                query = query.Where(c => c.Id <= filter.ToClientId.Value);


            if (filter.FromDate.HasValue)
                query = query.Where(c => c.IssueDate >= DateOnly.FromDateTime(filter.FromDate.Value));
            if (filter.ToDate.HasValue)
                query = query.Where(c => c.IssueDate <= DateOnly.FromDateTime(filter.ToDate.Value));

            var clients = query.ToList();

            var result = clients.Select((c, index) => new CourtReportVm
            {
                Index = index + 1,
                Id = c.Code,
                Name = c.PartyName,
                NationalId = c.NationalNumber,
                Phone = c.MobileNumber,
                Address = c.Address
            }).ToList();

            return result;
        }

        //public async Task<List<ReceiptIssueDto>> GetReceiptIssueAsync(FilterDto filter)
        //{
        //    var receipts = (await _unitOfWork.receipts.GetAll()).AsQueryable();
        //    var payments = (await _unitOfWork.payments.GetAll()).AsQueryable();

        //    // فلترة...
        //    // [كما تم شرحه سابقاً]

        //    var result = new List<ReceiptIssueDto>();

        //    // تحويل الكل إلى قائمة موحدة مؤقتًا
        //    var combined = new List<(string Type, DateOnly Date, decimal Amount, string IssueTitle, string Description)>
        //    ();

        //    foreach (var r in receipts)
        //        combined.Add(("قبض", r.DateReceipt, r.Amount, r.Purpose, r.ProjectName));

        //    foreach (var p in payments)
        //        combined.Add(("صرف", p.DatePayment, p.Amount, p.Purpose, p.ProjectName));

        //    // ترتيب حسب التاريخ
        //    combined = combined.OrderBy(x => x.Date).ToList();

        //    decimal runningBalance = 0;
        //    int index = 1;

        //    foreach (var entry in combined)
        //    {
        //        decimal debit = 0;
        //        decimal credit = 0;

        //        if (entry.Type == "صرف")
        //        {
        //            debit = entry.Amount;
        //            runningBalance -= entry.Amount;
        //        }
        //        else if (entry.Type == "قبض")
        //        {
        //            credit = entry.Amount;
        //            runningBalance += entry.Amount;
        //        }

        //        result.Add(new ReceiptIssueDto
        //        {
        //            Index = index++,
        //            InvoiceOrBond = entry.Type == "قبض" ? "سند قبض" : "سند صرف",
        //            Date = entry.Date,
        //            IssueTitle = entry.IssueTitle,
        //            DebtorAmount = debit,
        //            CreditorAmount = credit,
        //            Balance = runningBalance,
        //            Description = entry.Description
        //        });
        //    }

        //    return result;
        //}
        public async Task<List<ClientReceiptReportDto>> GetReceiptIssueGroupedByClient(FilterDto filter)
        {
            var receipts = (await _unitOfWork.receipts.GetAll()).AsQueryable();
            var payments = (await _unitOfWork.payments.GetAll()).AsQueryable();
            var clients = await _unitOfWork.clients.GetAll();

            // فلترة حسب التاريخ (نؤجل فلترة حسب Client الآن)
            if (filter.FromDate.HasValue)
            {
                receipts = receipts.Where(r => r.DateReceipt <=DateOnly.FromDateTime( filter.ToDate.Value));
                payments = payments.Where(p => p.DatePayment <= DateOnly.FromDateTime(filter.ToDate.Value));
            }

            if (filter.ToDate.HasValue)
            {
                receipts = receipts.Where(r => r.DateReceipt <= DateOnly.FromDateTime(filter.ToDate.Value));
                payments = payments.Where(p => p.DatePayment <= DateOnly.FromDateTime(filter.ToDate.Value));
            }

            var result = new List<ClientReceiptReportDto>();

            // فلترة الوكلاء
            var filteredClients = clients
                .Where(c => (!filter.FromClientId.HasValue || c.Id >= filter.FromClientId)
                         && (!filter.ToClientId.HasValue || c.Id <= filter.ToClientId))
                .ToList();

            foreach (var client in filteredClients)
            {
                var clientReceipts = receipts.Where(r => r.ClientId == client.Id).ToList();
                var clientPayments = payments.Where(p => p.ClientId == client.Id).ToList();

                var items = new List<ReceiptIssueDto>();

                // رصيد افتتاحي
                decimal openingBalance = 0;
                if (filter.FromDate.HasValue)
                {
                    var receiptsBefore = clientReceipts.Where(r => r.DateReceipt < DateOnly.FromDateTime(filter.FromDate.Value)).ToList();
                    var paymentsBefore = clientPayments.Where(p => p.DatePayment < DateOnly.FromDateTime(filter.FromDate.Value)).ToList();

                    openingBalance = receiptsBefore.Sum(r => r.Amount) - paymentsBefore.Sum(p => p.Amount);
                }

                // بيانات داخل الفترة فقط
                var receiptsInRange = filter.FromDate.HasValue
                    ? clientReceipts.Where(r => r.DateReceipt >= DateOnly.FromDateTime(filter.FromDate.Value)).ToList()
                    : clientReceipts.ToList();

                var paymentsInRange = filter.FromDate.HasValue
                    ? clientPayments.Where(p => p.DatePayment >= DateOnly.FromDateTime(filter.FromDate.Value)).ToList()
                    : clientPayments.ToList();

                foreach (var r in receiptsInRange)
                {
                    items.Add(new ReceiptIssueDto
                    {
                        InvoiceOrBond = "سند قبض",
                        Date = r.DateReceipt,
                        ReceiptNumber = r.Code,
                        IssueTitle = r.Purpose,
                        DebtorAmount = 0,
                        CreditorAmount = r.Amount,
                        Description = r.ProjectName
                    });
                }

                foreach (var p in paymentsInRange)
                {
                    items.Add(new ReceiptIssueDto
                    {
                        InvoiceOrBond = "سند صرف",
                        Date = p.DatePayment,
                        ReceiptNumber = p.Code,
                        IssueTitle = p.Purpose,
                        DebtorAmount = p.Amount,
                        CreditorAmount = 0,
                        Description = p.ProjectName
                    });
                }

                // ترتيب وحساب الرصيد
                var ordered = items.OrderBy(i => i.Date).ThenBy(i => i.InvoiceOrBond).ToList();
                decimal runningBalance = openingBalance;

                if (filter.FromDate.HasValue)
                {
                    ordered.Insert(0, new ReceiptIssueDto
                    {
                        Index = 0,
                        InvoiceOrBond = "رصيد افتتاحي",
                        Date = DateOnly.FromDateTime(filter.FromDate.Value.AddDays(-1)),
                        ReceiptNumber = 0,
                        IssueTitle = "",
                        DebtorAmount = 0,
                        CreditorAmount = 0,
                        Balance = openingBalance,
                        Description = ""
                    });
                }

                int i = 1;
                foreach (var item in ordered)
                {
                    if (item.InvoiceOrBond != "رصيد افتتاحي")
                        runningBalance += item.CreditorAmount - item.DebtorAmount;

                    item.Balance = runningBalance;
                    item.Index = i++;
                }

                // إضافة نتيجة العميل فقط لو لديه بيانات
                if (ordered.Count > 0)
                {
                    result.Add(new ClientReceiptReportDto
                    {
                        ClientName = client.ClientName,
                        Items = ordered
                    });
                }
            }

            return result;
        }

        public async Task<AdminReportViewModel> GetAdminReportAsync()
        {
            var totalCases = await _context.cases.CountAsync();
            var totalOffers = await _context.offers.CountAsync();
            var totalOpinions = await _context.opinsion.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            var mostOfferedCase = await _context.offers
                .GroupBy(o => o.Case)
                .OrderByDescending(g => g.Count())
                .Select(g => new { g.Key.Title, Count = g.Count() })
                .FirstOrDefaultAsync();

            var topLawyer = await _context.offers
                .GroupBy(o => o.User)
                .OrderByDescending(g => g.Count())
                .Select(g => new { g.Key.UserName, Count = g.Count() })
                .FirstOrDefaultAsync();

            var mostOpinionatedCase = await _context.opinsion
                .GroupBy(o => o.Case)
                .OrderByDescending(g => g.Count())
                .Select(g => new { g.Key.Title, Count = g.Count() })
                .FirstOrDefaultAsync();

            return new AdminReportViewModel
            {
                TotalCases = totalCases,
                TotalOffers = totalOffers,
                TotalOpinions = totalOpinions,
                TotalUsers = totalUsers,
                MostOfferedCaseTitle = mostOfferedCase?.Title ?? "لا يوجد",
                MostOfferedCaseCount = mostOfferedCase?.Count ?? 0,
                TopLawyerName = topLawyer?.UserName ?? "لا يوجد",
                TopLawyerOfferCount = topLawyer?.Count ?? 0,
                MostOpinionatedCaseTitle = mostOpinionatedCase?.Title ?? "لا يوجد",
                MostOpinionatedCaseCount = mostOpinionatedCase?.Count ?? 0
            };
        }
        public async Task<List<TodaySessionViewModel>> GetTodaySessionsViewModelAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var sessions =( await _unitOfWork.courtsSession
                                            .GetAll()).
                                            Where(s=>s.DateNextSession==today).ToList();
            var clients = await _unitOfWork.clients.GetAll();
            var parties = await _unitOfWork.parties.GetAll();
            var courts = await _unitOfWork.courts.GetAll();
            var centers=await _unitOfWork.centers.GetAll();
            var result = sessions.Select(s => new TodaySessionViewModel
            {
                IssueName = s.IssueName,
                ClientName = clients.FirstOrDefault(i => i.Id == s.ClientId) != null ? clients.FirstOrDefault(i => i.Id == s.ClientId).ClientName : "غير محدد",
                ClientPhone = s.ClientPhoneNumber,
                PartyName = parties.FirstOrDefault(i => i.Id == s.PartyId) != null ? parties.FirstOrDefault(i => i.Id == s.PartyId).PartyName : "غير محدد",
               
                CourtName = courts.FirstOrDefault(i=>i.Id==s.CourtId)!=null? courts.FirstOrDefault(i => i.Id == s.CourtId).CourtName:"",
                CenterName = centers.FirstOrDefault(i => i.Id == s.CenterId) != null ? centers.FirstOrDefault(i => i.Id == s.CenterId).Name : "",
                ClaimNumber = s.Code.ToString(),
                Notes = s.WhatHappenedInTheCourtSession
            }).ToList();

            return result;
        }

        public async Task<List<AdminLawyerActivityViewModel>> GetAllLawyerActivityAsync()
        {
            var offers = await _unitOfWork.OfferRepository.GetAll();
            var opinions = await _unitOfWork.OpinionRepository.GetAll();
            var cases = await _unitOfWork.cases.GetAll();
            var users = await _unitOfWork.auth.GetAllUsers();

            var result = new List<AdminLawyerActivityViewModel>();

            foreach (var offer in offers)
            {
                var caseEntity = cases.FirstOrDefault(c => c.Id == offer.CaseId);
                var user = users.FirstOrDefault(u => u.Id == offer.UserId);

                result.Add(new AdminLawyerActivityViewModel
                {
                    UserId = offer.UserId,
                    LawyerName = user?.UserName ?? "غير معروف",
                    CaseId = offer.CaseId,
                    CaseTitle = caseEntity?.Title ?? "غير معروفة",
                    OfferPhoneNumber = offer.PhoneNumber,
                    OfferOfficeNumber = offer.OfficeNumber,
                    ProposedSolution = offer.ProposedSolution,
                    ProposedFees = offer.ProposedFees,
                    OfferSubmittedAt = offer.SubmittedAt
                });
            }

            foreach (var opinion in opinions)
            {
                var caseEntity = cases.FirstOrDefault(c => c.Id == opinion.CaseId);
                var user = users.FirstOrDefault(u => u.Id == opinion.UserId);

                result.Add(new AdminLawyerActivityViewModel
                {
                    UserId = opinion.UserId,
                    LawyerName = user?.UserName ?? "غير معروف",
                    CaseId = opinion.CaseId,
                    CaseTitle = caseEntity?.Title ?? "غير معروفة",
                    OpinionComment = opinion.Comment,
                    OpinionPostedAt = opinion.PostedAt
                });
            }

            return result.OrderByDescending(r => r.OfferSubmittedAt ?? r.OpinionPostedAt).ToList();
        }



    }
}
