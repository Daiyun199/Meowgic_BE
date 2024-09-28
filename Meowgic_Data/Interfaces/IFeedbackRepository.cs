using Meowgic.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Interfaces
{
    public interface IFeedbackRepository
    {
        /// <summary>
        /// Lấy tất cả các feedbacks.
        /// </summary>
        /// <returns>Danh sách các feedback.</returns>
        Task<IEnumerable<Feedback>> GetAllAsync();

        /// <summary>
        /// Lấy một feedback theo ID.
        /// </summary>
        /// <param name="id">ID của feedback.</param>
        /// <returns>Đối tượng feedback hoặc null nếu không tìm thấy.</returns>
        Task<Feedback?> GetByIdAsync(string id);

        Task<List<Feedback>> GetAllByServiceId(string serviceId);

        /// <summary>
        /// Thêm một feedback mới.
        /// </summary>
        /// <param name="feedback">Đối tượng feedback cần thêm.</param>
        /// <returns>Đối tượng feedback đã được thêm.</returns>
        Task<Feedback> AddAsync(Feedback feedback);

        /// <summary>
        /// Cập nhật một feedback hiện có.
        /// </summary>
        /// <param name="feedback">Đối tượng feedback cần cập nhật.</param>
        /// <returns>Đối tượng feedback đã được cập nhật.</returns>
        Task<Feedback> UpdateAsync(Feedback feedback);

        /// <summary>
        /// Xóa một feedback theo ID.
        /// </summary>
        /// <param name="id">ID của feedback cần xóa.</param>
        /// <returns>Trả về true nếu xóa thành công, ngược lại trả về false.</returns>
        Task<bool> DeleteAsync(string id);
    }
}
