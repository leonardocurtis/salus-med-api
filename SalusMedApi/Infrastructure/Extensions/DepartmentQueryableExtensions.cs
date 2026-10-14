using SalusMedApi.Domain.Entities;
using SalusMedApi.Domain.Enums;

namespace SalusMedApi.Infrastructure.Extensions;

public static class DepartmentQueryableExtensions
{
    extension(IQueryable<Department> query)
    {
        public IQueryable<Department> Active() =>
            query.Where(d => d.Status == DepartmentStatus.Active);

        public IQueryable<Department> ByPublicId(Guid publicId) =>
            query.Where(d => d.PublicId == publicId);

        public IQueryable<Department> ActiveByPublicId(Guid publicId) =>
            query.Active().ByPublicId(publicId);
    }
}
