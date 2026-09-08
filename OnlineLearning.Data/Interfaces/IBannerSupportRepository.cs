using OnlineLearning.Core.Common;
using OnlineLearning.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineLearning.Data.Interfaces
{
    public interface IBannerSupportRepository
    {
        // Banners
        Task<DbResult> CreateBanner(CreateBannerRequest request);
        Task<IEnumerable<BannerDto>> GetBanners();
        Task<BannerDto?> GetBannerById(int bannerId);
        Task<DbResult> UpdateBanner(UpdateBannerRequest request);
        Task<DbResult> DeleteBanner(int bannerId);

        // Support Tickets
        Task<DbResult> CreateSupportTicket(CreateSupportTicketRequest request);
        Task<IEnumerable<SupportTicketDto>> GetSupportTickets(int? userId, string? status);
        Task<SupportTicketDto?> GetSupportTicketById(int ticketId);
        Task<DbResult> UpdateSupportTicket(UpdateSupportTicketRequest request);
        Task<DbResult> CloseSupportTicket(int ticketId);
        Task<DbResult> CreateTicketReply(CreateTicketReplyRequest request);
        Task<IEnumerable<TicketReplyDto>> GetTicketReplies(int ticketId);
    }
}
